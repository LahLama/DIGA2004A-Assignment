using TMPro;
using Unity.VisualScripting;
using UnityEditor.TerrainTools;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UI;



public class Interactor : MonoBehaviour
{

    //Drop": Look in pickUp parent, look a children tha matches current enabled one on the player, if yes: diabble On player, set pos of 'drop" to be infront(only 1 mili, cause Out of bounds things) of player then enable dropped item

    // /https://www.youtube.com/watch?v=cUf7FnNqv7U
    //https://youtu.be/zEfahR66Pa8
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
    public bool _PlayerIsHidden = false;




    [Header("Scripts")]
    private PickUpSystem pickUpSystem;
    private ReticleManagement reticleManagement;
    private InnerDialouge innerDialouge;
    private HideAndShowPlayer hideAndShowPlayer;
    private DoorUnlocking doorUnlocking;

    [Header("InteractionVar")]
    public float _interactionDelay = 0f;
    private float _maxInteractionDelay = 0.5f;
    private float _interactionRange = 3.5f;

    #endregion
    private void Awake()
    {
        pickUpSystem = GetComponent<PickUpSystem>();
        reticleManagement = GetComponent<ReticleManagement>();
        hideAndShowPlayer = GetComponent<HideAndShowPlayer>();
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
    }







    public void OnInteractions(InputAction.CallbackContext context) { _interactionInput = context.ReadValueAsButton(); }
    public void OnDrop(InputAction.CallbackContext context) { _dropInput = context.ReadValueAsButton(); }


    void HandleInteractions()
    {


        _isGenericObject = Physics.Raycast(transform.position, transform.forward, out hitGeneric, _interactionRange, interactionsMask);
        _isPickUpObject = Physics.Raycast(transform.position, transform.forward, out hitPickUp, _interactionRange, pickUpMask);
        _isHideObject = Physics.Raycast(transform.position, transform.forward, out hitHideObj, _interactionRange / 2, hideAwayMask);
        _isDoorObject = Physics.Raycast(transform.position, transform.forward, out hitDoorObj, _interactionRange, doorMask);

        reticleManagement.HandleTooltip();

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
                hideAndShowPlayer.HidePlayer();
                _PlayerIsHidden = true;
            }

            else if (_isDoorObject)
            {

                doorUnlocking.CanPlayerOpenDoor();
            }
            else
            {
                //Debug.Log("NULL");
                Debug.DrawRay(transform.position, transform.TransformDirection(Vector3.forward) * 5f, Color.red);
            }

            if (_interactionDelay <= 0 && _PlayerIsHidden)
            {
                hideAndShowPlayer.ShowPlayer();
                _PlayerIsHidden = false;
            }
        }



        if (_dropInput)
        {
            pickUpSystem.DropItem();
        }



    }








}
