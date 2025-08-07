using UnityEngine;
using UnityEngine.UI;

public class ReticleManagement : MonoBehaviour
{
    bool _isGenericObject;
    bool _isPickUpObject;
    bool _isHideObject;
    private Interactor _interactor;
    public Image CanInteractToolTip;
    public GameObject CanInteractText;
    void Start()
    {
        _interactor = GetComponent<Interactor>();
    }

    void Update()
    {
        _isGenericObject = _interactor._isGenericObject;
        _isPickUpObject = _interactor._isPickUpObject;
        _isHideObject = _interactor._isHideObject;
    }
    public void HandleTooltip()
    {
        if (!(_isGenericObject || _isPickUpObject || _isHideObject))
        {
            //Nothing to interact with 
            CanInteractToolTip.color = new Color(1f, 1, 1f, 0.5f);
            CanInteractText.SetActive(false);
        }
        else
        {
            //Can interact with something
            CanInteractToolTip.color = new Color(1f, 1, 1f, 1f);
            CanInteractText.SetActive(true);
        }
    }
}
