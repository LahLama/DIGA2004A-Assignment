using System.Collections;
using UnityEngine;
using UnityEngine.InputSystem;




public class Interactor : MonoBehaviour
{

    //Drop": Look in pickUp parent, look a children tha matches current enabled one on the player, if yes: diabble On player, set pos of 'drop" to be infront(only 1 mili, cause Out of bounds things) of player then enable dropped item


    /*
    Title: Creating a Horror Game in Unity - Part 6: Hiding System (JavaScript)
    Author: SpeedTutor
    Date:  Jun 10, 2014
    Availability: https://youtu.be/GTtW57u_cfg?si=NqfWBUk5yLfmXfn-
    */

    /*
    Title: Creating a Horror Game in Unity - Part 6: Hiding System (JavaScript)
    Author: SpeedTutor
    Date:  Jun 10, 2014
    Availability: https://youtu.be/GTtW57u_cfg?si=NqfWBUk5yLfmXfn-
    */
    // /https://www.youtube.com/watch?v=cUf7FnNqv7U
    //
    //https://www.youtube.com/watch?v=QYpWYZq2I6E


    #region Varibles

    [Header("Masks")]
    public LayerMask interactionsMask;
    public LayerMask pickUpMask;
    public LayerMask HoldMask;
    public LayerMask hideAwayMask;
    public LayerMask doorMask;

    [Header("RaycastHits")]


    public RaycastHit hitGeneric;
    public RaycastHit hitPickUp;
    public RaycastHit hitHideObj;
    public RaycastHit hitDoorObj;

    [Header("Bools")]
    public bool _isGenericObject;
    public bool _isPickUpObject;
    public bool _isHideObject;
    public bool _isDoorObject;

    private bool _interactionInput;
    private bool _dropInput;
    private bool _throwInput;
    public bool _PlayerIsHidden = false;




    [Header("Scripts")]
    private PickUpSystem pickUpSystem;
    private ReticleManagement reticleManagement;
    private InnerDialouge innerDialouge;
    private HideAndShowPlayer hideAndShowPlayer;
    private DoorUnlocking doorUnlocking;

    private HighlightObject highlightObject;

    [Header("InteractionVar")]
    public float _interactionDelay = 0f;
    private float _maxInteractionDelay = 0.5f;
    private float _interactionRange = 3.5f;
    public float hideDuration = 5f;
    private Collider _currentHideObj;

    #endregion
    private void Awake()
    {
        pickUpSystem = GetComponent<PickUpSystem>();
        reticleManagement = GetComponent<ReticleManagement>();
        innerDialouge = GetComponent<InnerDialouge>();
        doorUnlocking = GetComponent<DoorUnlocking>();


    }

    void Update()
    {
        HandleInteractions();
        if (_interactionDelay > 0f)
        {
            _interactionDelay -= Time.deltaTime;
            Mathf.RoundToInt(_interactionDelay);
        }

        if (hideDuration > 0f)
        {
            hideDuration -= Time.deltaTime;
            Mathf.RoundToInt(hideDuration);
        }


    }







    public void OnInteractions(InputAction.CallbackContext context) { _interactionInput = context.ReadValueAsButton(); }
    public void OnDrop(InputAction.CallbackContext context) { _dropInput = context.ReadValueAsButton(); }
    public void OnThrow(InputAction.CallbackContext context) { _throwInput = context.ReadValueAsButton(); }


    void HandleInteractions()
    {


        _isGenericObject = Physics.Raycast(transform.position, transform.forward, out hitGeneric, _interactionRange, interactionsMask);
        _isPickUpObject = Physics.Raycast(transform.position, transform.forward, out hitPickUp, _interactionRange, pickUpMask);
        _isDoorObject = Physics.Raycast(transform.position, transform.forward, out hitDoorObj, _interactionRange, doorMask);

        reticleManagement.HandleTooltip();

        if (!_PlayerIsHidden)
        {
            _isHideObject = Physics.Raycast(transform.position, transform.forward, out hitHideObj, _interactionRange / 2, hideAwayMask);
        }


        if (_interactionInput && _interactionDelay <= 0)
        {



            if (_isGenericObject)
            {
                Debug.DrawRay(transform.position, transform.TransformDirection(Vector3.forward) * hitGeneric.distance, Color.green);
                innerDialouge.text.text = "This is just an object.";
                StartCoroutine(innerDialouge.InnerDialogueContorl());
            }

            else if (_isPickUpObject)
            {
                Debug.DrawRay(transform.position, transform.TransformDirection(Vector3.forward) * hitPickUp.distance, Color.blue);
                _interactionDelay = _maxInteractionDelay;
                pickUpSystem.EquipItem();

            }

            else if (_isHideObject)
            {
                hitHideObj.collider.gameObject.GetComponent<HideAndShowPlayer>().HidePlayer();
                _PlayerIsHidden = true;
                hideDuration = 5f;
                _interactionDelay = 1f;
            }

            else if (_isDoorObject)
            {

                doorUnlocking.CanPlayerOpenDoor();
            }

        }

        //Show player after 5seconds, the limit of hiding
        if (hideDuration <= 0 && _PlayerIsHidden)
        {
            hitHideObj.collider.gameObject.GetComponent<HideAndShowPlayer>().ShowPlayer();
            _PlayerIsHidden = false;
        }
        // Wait for 1 second, then check if interaction input and player is hidden
        if (_PlayerIsHidden)
        {
            StartCoroutine(WaitAndCheckHide());
        }


        if (_interactionInput && _interactionDelay / 4 < 0)
        {
            pickUpSystem.DropItem();
        }
        if (_throwInput)
        {
            pickUpSystem.ThrowItem();
        }

        if (_isPickUpObject)
        {
            hitPickUp.collider.gameObject.GetComponent<HighlightObject>().ChangeMaterial();
        }
        else
        {
            //   hitPickUp.collider.gameObject.GetComponent<HighlightObject>().ResetMaterial();
        }
    }


    private IEnumerator WaitAndCheckHide()
    {
        yield return new WaitForSeconds(0.5f);
        if (_interactionInput && _PlayerIsHidden)
        {
            hitHideObj.collider.gameObject.GetComponent<HideAndShowPlayer>().ShowPlayer();
            _PlayerIsHidden = false;
        }
    }






}
