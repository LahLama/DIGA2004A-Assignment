using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class ReticleManagement : MonoBehaviour
{
    private bool _isGenericObject;
    private bool _isPickUpObject;
    private bool _isHideObject;
    private bool _isDoorObject;
    private Interactor _interactor;
    private Image _toolTip;
    LayerMask specifiedLayer;
    private TextMeshProUGUI _tooltipText;
    void Start()
    {
        _interactor = GetComponent<Interactor>();
        TryFindTooltip(); // attempt once at start
    }

    //TEMPORRAITLY
    void Update()
    {
        if (_toolTip == null || _tooltipText == null)
            TryFindTooltip();

        if (_interactor.hitObj)
            specifiedLayer = _interactor.raycastHit.transform.gameObject.layer;
    }

    private bool TryFindTooltip()
    {
        var go = GameObject.FindWithTag("Tooltip");
        if (go == null) return false;
        _toolTip = go.GetComponent<Image>();
        if (_toolTip == null) return false;

        // assume first child holds the TextMeshProUGUI (preserve your original structure)
        if (_toolTip.transform.childCount > 0)
            _tooltipText = _toolTip.transform.GetChild(0).GetComponent<TextMeshProUGUI>();

        return _tooltipText != null;
    }
    public void HandleTooltip()
    {
        if (_toolTip == null || _tooltipText == null)
            return; // tooltip not ready yet

        if (!_interactor.hitObj || _interactor._interactionDelay > 0)
        {
            // Nothing to interact with 
            _toolTip.color = new Color(1f, 1f, 1f, 0.5f);
            _tooltipText.gameObject.SetActive(false);
            return;
        }

        // Can interact with something
        _toolTip.color = new Color(1f, 1f, 1f, 1f);

        string layerName = (specifiedLayer >= 0) ? LayerMask.LayerToName(specifiedLayer) : "";

        switch (layerName)
        {
            case "interactionsMask":
                _tooltipText.text = "Press(E) / (LMB) / (West) to Interact";
                break;
            case "hidePlacesMask":
                _tooltipText.text = "Press(E) / (LMB) / (West) to Hide";
                break;
            case "doorMask":
                _tooltipText.text = "Press(E) / (LMB) / (West) to Open";
                break;
            case "pickUpMask":
                _tooltipText.text = "Press(E) / (LMB) / (West) to Pick Up";
                break;
            default:
                _tooltipText.text = "";
                break;
        }

        _tooltipText.gameObject.SetActive(true);

        // Handle when player can leave when hiding
        if (_interactor._PlayerIsHidden && _interactor._interactionDelay < 0)
        {
            _tooltipText.text = "Press(E) / (LMB) / (West) to Get Out";
            _tooltipText.gameObject.SetActive(true);
        }
    }
}
