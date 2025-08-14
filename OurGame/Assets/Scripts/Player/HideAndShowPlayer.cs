using System.Runtime.CompilerServices;
using UnityEngine;

public class HideAndShowPlayer : MonoBehaviour
{
    #region Varibles

    private Transform player;
    private RaycastHit _hitHideObj;
    private Interactor _interactor;
    private Vector3 _playerOGpos;
    private Quaternion _playerOGrot;
    private GameObject _HideObj;
    private bool _isplayerHidden;
    private float _interactionDelay;
    private LookFunction lookFunction;
    private float playerHeight;

    #endregion



    void Awake()
    {
        player = this.transform.parent;
        playerHeight = player.GetComponent<CharacterController>().height;
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
            _playerOGpos = player.transform.localPosition;
            _playerOGrot = player.transform.rotation;


            player.gameObject.transform.SetParent(_hitHideObj.collider.gameObject.transform, true); //

            player.GetComponent<PlayerMovement>().enabled = false;
            // player.GetComponent<CharacterController>().detectCollisions = false;

            player.transform.localPosition = Vector3.zero;
            _HideObj = _hitHideObj.collider.gameObject;
            _interactor._interactionDelay = 0.5f;


            lookFunction.cameraTransform.GetComponent<Camera>().fieldOfView = 45;
            lookFunction.verticalLookLimit = 20;

        }
        else return;


    }

    public void ShowPlayer()
    {
        player.transform.SetParent(null, true);
        player.GetComponent<PlayerMovement>().enabled = true;
        player.transform.rotation = Quaternion.Euler(_playerOGrot.x, _playerOGrot.y + 180f, _playerOGrot.z);
        player.transform.localPosition = _playerOGpos;


        // player.GetComponent<CharacterController>().detectCollisions = true;


        lookFunction.cameraTransform.GetComponent<Camera>().fieldOfView = 60;
        lookFunction.verticalLookLimit = 90f;


    }
}
