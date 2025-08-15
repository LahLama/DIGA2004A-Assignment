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
    private TextMeshProUGUI _tooltipText;
    void Start()
    {
        _interactor = GetComponent<Interactor>();
        _toolTip = GameObject.FindWithTag("Tooltip").GetComponent<Image>();
        _tooltipText = _toolTip.gameObject.transform.GetChild(0).GetComponent<TextMeshProUGUI>();
    }

    void Update()
    {
        //Gets what the player is interacting with
        _isGenericObject = _interactor._isGenericObject;
        _isPickUpObject = _interactor._isPickUpObject;
        _isHideObject = _interactor._isHideObject;
        _isDoorObject = _interactor._isDoorObject;
    }
    public void HandleTooltip()
    {
        if (!(_isGenericObject || _isPickUpObject || _isHideObject || _isDoorObject) || _interactor._interactionDelay > 0)
        {
            //Nothing to interact with 
            _toolTip.color = new Color(1f, 1, 1f, 0.5f);
            _tooltipText.gameObject.SetActive(false);
        }
        else
        {
            //Can interact with something
            _toolTip.color = new Color(1f, 1, 1f, 1f);


            //Depending on the object, present a different tooltip
            if (_isGenericObject)
            {
                _tooltipText.text = "Press(E) / (LMB) / (West) to Interact";
            }
            if (_isPickUpObject)
            {
                _tooltipText.text = "Press(E) / (LMB) / (West) to Pick Up";
            }
            if (_isHideObject)
            {
                _tooltipText.text = "Press(E) / (LMB) / (West) to Hide";
            }
            if (_isDoorObject)
            {
                _tooltipText.text = "Press(E) / (LMB) / (West) to Open";
            }
            _tooltipText.gameObject.SetActive(true);
        }

        //Handle when player can leave when hiding
        if (_interactor._PlayerIsHidden && _interactor._interactionDelay < 0)
        {
            _tooltipText.text = "Press(E) / (LMB) / (West) to Get Out";
            _tooltipText.gameObject.SetActive(true);
        }
    }
}
