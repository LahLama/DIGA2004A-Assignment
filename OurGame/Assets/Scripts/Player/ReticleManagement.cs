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


    public void HandleTooltip()
    {
        if (!_interactor.hitObj || _interactor._interactionDelay > 0)
        {
            //Nothing to interact with 
            _toolTip.color = new Color(1f, 1, 1f, 0.5f);
            _tooltipText.gameObject.SetActive(false);
        }
        else
        {
            //Can interact with something
            _toolTip.color = new Color(1f, 1, 1f, 1f);

            LayerMask specifiedLayer = _interactor.raycastHit.transform.gameObject.layer;
            string layerName = LayerMask.LayerToName(specifiedLayer.value);

            //Depending on the object, present a different tooltip
            switch (layerName)
            {
                case "interactionsMask":
                    _tooltipText.text = "Press(E) / (LMB) / (West) to Interact";
                    break;

                case "hideAwayMask":
                    _tooltipText.text = "Press(E) / (LMB) / (West) to Hide";
                    break;

                case "doorMask":
                    _tooltipText.text = "Press(E) / (LMB) / (West) to Open";
                    break;

                case "pickUpMask":
                    _tooltipText.text = "Press(E) / (LMB) / (West) to Pick Up";
                    break;
                default:
                    break;
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
