using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.InputSystem;



public class Interactor : MonoBehaviour
{

    //Drop": Look in pickUp parent, look a children tha matches current enabled one on the player, if yes: diabble On player, set pos of 'drop" to be infront(only 1 mili, cause Out of bounds things) of player then enable dropped item

    // /https://www.youtube.com/watch?v=cUf7FnNqv7U
    //https://youtu.be/zEfahR66Pa8
    //https://www.youtube.com/watch?v=QYpWYZq2I6E


    [Header("Masks")]
    public LayerMask interactionsMask;
    public LayerMask pickUpMask;
    public LayerMask HoldMask;
    public LayerMask HideAwayMask;

    [Header("Tooltips")]



    public RaycastHit hitGeneric;
    public RaycastHit hitPickUp;
    public RaycastHit hitHideObj;


    public GameObject Player;
    private bool _interactionInput;
    private bool _dropInput;
    private bool _ExitHideInput;
    public bool _isGenericObject;
    public bool _isPickUpObject;
    public bool _isHideObject;
    private bool _isPlayerHidden;

    public GameObject HideOverlay;

    private GameObject HideObj;

    [Header("Scripts")]
    private PickUpSystem pickUpSystem;
    private ReticleManagement reticleManagement;
    private InnerDialouge innerDialouge;
    private LookFunction lookFunction;

    private float _interactionDelay = 0f;

    private void Start()
    {
        pickUpSystem = GetComponent<PickUpSystem>();
        reticleManagement = GetComponent<ReticleManagement>();
        lookFunction = GetComponentInParent<LookFunction>();
    }

    void Update()
    {
        HandleInteractions();
        if (_interactionDelay > 0f)
        {
            _interactionDelay -= Time.deltaTime;
        }
    }







    public void OnInteractions(InputAction.CallbackContext context) { _interactionInput = context.ReadValueAsButton(); }
    public void OnDrop(InputAction.CallbackContext context) { _dropInput = context.ReadValueAsButton(); }

    public void OnHideExit(InputAction.CallbackContext context) { _ExitHideInput = context.ReadValueAsButton(); }

    void HandleInteractions()
    {


        _isGenericObject = Physics.Raycast(transform.position, transform.forward, out hitGeneric, 3.5f, interactionsMask);
        _isPickUpObject = Physics.Raycast(transform.position, transform.forward, out hitPickUp, 3.5f, pickUpMask);
        _isHideObject = Physics.Raycast(transform.position, transform.forward, out hitHideObj, 3.5f, HideAwayMask);

        reticleManagement.HandleTooltip();

        if (_interactionInput && _interactionDelay <= 0)
        {

            if (_isGenericObject)
            {
                Debug.Log("hit _isGenericObject");
                Debug.DrawRay(transform.position, transform.TransformDirection(Vector3.forward) * hitGeneric.distance, Color.green);
                StartCoroutine(innerDialouge.InnerDialogueContorl());


            }

            else if (_isPickUpObject)
            {
                Debug.Log("hit _isPickUpObject");
                Debug.DrawRay(transform.position, transform.TransformDirection(Vector3.forward) * hitPickUp.distance, Color.blue);
                _interactionDelay = 1f;

                pickUpSystem.EquipItem();

            }

            else if (_isHideObject)
            {
                HidePlayer();
            }

            else
            {
                //Debug.Log("NULL");
                Debug.DrawRay(transform.position, transform.TransformDirection(Vector3.forward) * 5f, Color.red);
            }

            if (_interactionDelay < 0)
            {
                ShowPlayer();
            }
        }



        if (_dropInput)
        {
            pickUpSystem.DropItem();
        }



    }





    public void HidePlayer()
    {
        Player.transform.SetParent(hitHideObj.collider.gameObject.transform, true);

        Player.GetComponent<PlayerMovement>().enabled = false;
        Player.transform.localPosition = Vector3.zero;
        HideObj = hitHideObj.collider.gameObject;
        _interactionDelay = 5f;
        HideOverlay.SetActive(true);

        lookFunction.cameraTransform.GetComponent<Camera>().fieldOfView = 45;
        lookFunction.verticalLookLimit = 20;




    }

    public void ShowPlayer()
    {

        Player.GetComponent<PlayerMovement>().enabled = true;
        Player.transform.localPosition = HideObj.transform.GetChild(0).localPosition;
        HideOverlay.SetActive(false);
        Player.transform.SetParent(null, true);
        lookFunction.cameraTransform.GetComponent<Camera>().fieldOfView = 60;
        lookFunction.verticalLookLimit = 90f;

    }


}
