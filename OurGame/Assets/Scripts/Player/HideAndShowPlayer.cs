using UnityEngine;

public class HideAndShowPlayer : MonoBehaviour
{
    #region Varibles

    public GameObject Player;
    private RaycastHit _hitHideObj;
    private Interactor _interactor;
    private GameObject _HideObj;
    private bool _isPlayerHidden;
    private float _interactionDelay;
    private LookFunction lookFunction;

    #endregion



    void Awake()
    {
        _interactor = GetComponent<Interactor>();
        lookFunction = GetComponentInParent<LookFunction>();
    }

    void Update()
    {
        _hitHideObj = _interactor.hitHideObj;
        _interactionDelay = _interactor._interactionDelay;
    }


    public void HidePlayer()
    {

        if (_hitHideObj.collider)
        {
            Player.transform.SetParent(_hitHideObj.collider.gameObject.transform, true); //

            Player.GetComponent<PlayerMovement>().enabled = false;
            Player.transform.localPosition = Vector3.zero;
            _HideObj = _hitHideObj.collider.gameObject;
            _interactor._interactionDelay = 1f;


            lookFunction.cameraTransform.GetComponent<Camera>().fieldOfView = 45;
            lookFunction.verticalLookLimit = 20;

        }
        else return;


    }

    public void ShowPlayer()
    {

        Player.transform.localPosition = _HideObj.transform.GetChild(0).localPosition;
        Player.transform.SetParent(null, true);
        lookFunction.cameraTransform.GetComponent<Camera>().fieldOfView = 60;
        lookFunction.verticalLookLimit = 90f;
        Player.GetComponent<PlayerMovement>().enabled = true;

    }
}
