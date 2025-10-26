using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class ReticleManagement : MonoBehaviour
{
    private bool _isGenericObject; // unused but reserved for future cases
    private bool _isPickUpObject; // unused for now but may be used for object classification
    private bool _isHideObject; // will help detect hiding zones
    private bool _isDoorObject; // tracks door objects if needed later
    private Interactor _interactor; // core interaction logic reference
    private Image _toolTip; // tooltip UI background image
    LayerMask specifiedLayer; // holds detected object layer
    private TextMeshProUGUI _tooltipText; // tooltip label text

    void Start()
    {
        // Cache component and search tooltip UI elements once
        _interactor = GetComponent<Interactor>();
        TryFindTooltip(); // attempt to find tooltip setup at the beginning
    }

    //TEMPORRAITLY
    void Update()
    {
        // Fallback if tooltip UI loads late or is dynamically spawned
        if (_toolTip == null || _tooltipText == null)
            TryFindTooltip();

        // Store layer of the object hit by the interaction raycast
        if (_interactor.hitObj)
            specifiedLayer = _interactor.raycastHit.transform.gameObject.layer;
    }

    private bool TryFindTooltip()
    {
        // Search for tooltip background
        var go = GameObject.FindWithTag("Tooltip");
        if (go == null) return false;
        _toolTip = go.GetComponent<Image>();
        if (_toolTip == null) return false;

        // Expect tooltip text to be the first child under tooltip UI container
        if (_toolTip.transform.childCount > 0)
            _tooltipText = _toolTip.transform.GetChild(0).GetComponent<TextMeshProUGUI>();

        return _tooltipText != null; // return success state
    }

    public void HandleTooltip()
    {
        // Skip if tooltip UI unavailable
        if (_toolTip == null || _tooltipText == null)
            return;

        // If no object to interact with or cooldown is active, hide tooltip text and fade icon
        if (!_interactor.hitObj || _interactor._interactionDelay > 0)
        {
            _toolTip.color = new Color(1f, 1f, 1f, 0.5f); // half opacity
            _tooltipText.gameObject.SetActive(false);
            return;
        }

        // Show tooltip background fully when interaction is valid
        _toolTip.color = new Color(1f, 1f, 1f, 1f);

        // Convert mask id into its name string to decide UI text
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
                _tooltipText.text = ""; // fallback for unknown cases
                break;
        }

        _tooltipText.gameObject.SetActive(true); // ensure text is visible

        // Override UI when already hidden so prompt correctly reflects exit action
        if (_interactor._PlayerIsHidden && _interactor._interactionDelay < 0)
        {
            _tooltipText.text = "Press(E) / (LMB) / (West) to Get Out";
            _tooltipText.gameObject.SetActive(true);
        }
    }
}
