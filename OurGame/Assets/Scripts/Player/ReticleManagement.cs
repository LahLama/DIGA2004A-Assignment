using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class ReticleManagement : MonoBehaviour
{
    bool _isGenericObject;
    bool _isPickUpObject;
    bool _isHideObject;
    bool _isDoorObject;
    private Interactor _interactor;
    public Image CanInteractToolTip;
    public TextMeshProUGUI CanInteractText;
    void Start()
    {
        _interactor = GetComponent<Interactor>();
    }

    void Update()
    {
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
            CanInteractToolTip.color = new Color(1f, 1, 1f, 0.5f);
            CanInteractText.gameObject.SetActive(false);
        }
        else
        {
            //Can interact with something
            CanInteractToolTip.color = new Color(1f, 1, 1f, 1f);



            if (_isGenericObject)
            {
                CanInteractText.text = "Press(E) / (LMB) / (West) to Interact";
            }
            if (_isPickUpObject)
            {
                CanInteractText.text = "Press(E) / (LMB) / (West) to Pick Up";
            }
            if (_isHideObject)
            {
                CanInteractText.text = "Press(E) / (LMB) / (West) to Hide";
            }
            if (_isDoorObject)
            {
                CanInteractText.text = "Press(E) / (LMB) / (West) to Open";
            }
            CanInteractText.gameObject.SetActive(true);
        }


        if (_interactor._PlayerIsHidden && _interactor._interactionDelay < 0)
        {
            CanInteractText.text = "Press(E) / (LMB) / (West) to Get Out";
            CanInteractText.gameObject.SetActive(true);
        }
    }
}
