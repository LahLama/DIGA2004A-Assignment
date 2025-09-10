using System.Collections;
using UnityEngine;
using UnityEngine.InputSystem;




public class Interactor : MonoBehaviour
{
    /*
    Title: Raycast Unity 3d - unity raycast tutorial
    Author: Pixelbug Studio
    Date:  Sep 9, 2021
    Availability: https://www.youtube.com/watch?v=cUf7FnNqv7U
    */

    /*
    Title: (URP) How to fix Gun Clipping in Unity | Unity3D Tutorial | 
    Author: Podd Studios
    Date:  Dec 3, 2022
    Availability: https://www.youtube.com/watch?v=QYpWYZq2I6E
    */



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
    #region UnityFunctions
    private void Awake()
    {
        pickUpSystem = GetComponent<PickUpSystem>();
        reticleManagement = GetComponent<ReticleManagement>();
        innerDialouge = GetComponent<InnerDialouge>();
        doorUnlocking = GetComponent<DoorUnlocking>();
<<<<<<< HEAD
=======


>>>>>>> James
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
    #endregion

    #region NewInputSystem
    public void OnInteractions(InputAction.CallbackContext context) { _interactionInput = context.ReadValueAsButton(); }
    public void OnThrow(InputAction.CallbackContext context) { _throwInput = context.ReadValueAsButton(); }

    #endregion

    void HandleInteractions()
    {
        //Get depending on the ray and mask
        _isGenericObject = Physics.Raycast(transform.position, transform.forward, out hitGeneric, _interactionRange, interactionsMask);
        _isPickUpObject = Physics.Raycast(transform.position, transform.forward, out hitPickUp, _interactionRange, pickUpMask);
        _isDoorObject = Physics.Raycast(transform.position, transform.forward, out hitDoorObj, _interactionRange, doorMask);


        reticleManagement.HandleTooltip();

        //Only if the player is not hidden then check for a hiding spot
        if (!_PlayerIsHidden)
        {
            _isHideObject = Physics.Raycast(transform.position, transform.forward, out hitHideObj, _interactionRange / 2, hideAwayMask);
        }

        //Only when the player uses the interaction button and the cooldown is finished
        if (_interactionInput && _interactionDelay <= 0)
        {
            //If the object is a generic object then display a dialouge box
            if (_isGenericObject)
            {
                Debug.DrawRay(transform.position, transform.TransformDirection(Vector3.forward) * hitGeneric.distance, Color.green);
                innerDialouge.text.text = "This is just an object.";
                StartCoroutine(innerDialouge.InnerDialogueContorl());
            }

            //If the object is a pickup object, pick it up
            else if (_isPickUpObject)
            {
                Debug.DrawRay(transform.position, transform.TransformDirection(Vector3.forward) * hitPickUp.distance, Color.blue);
                _interactionDelay = _maxInteractionDelay;
                pickUpSystem.EquipItem();

            }

            //If the object is a hiding spot, hide the player and reset the hiding spot time
            else if (_isHideObject)
            {
                hitHideObj.collider.gameObject.GetComponent<HideAndShowPlayer>().HidePlayer();
                _PlayerIsHidden = true;
                hideDuration = 5f;
                _interactionDelay = 1f;
            }
            //If it is a door, check if the player can open the door or not
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

        //Drop the item if the player presses interaction and is not facing a door
        if (_interactionInput && _interactionDelay / 4 < 0 && !_isDoorObject && !_isGenericObject)
        {
            pickUpSystem.DropItem();
        }
        if (_throwInput)
        {
            pickUpSystem.ThrowItem();
        }
<<<<<<< HEAD
=======

        if (_isPickUpObject)
        {
            hitPickUp.collider.gameObject.GetComponent<HighlightObject>().ChangeMaterial();
        }
        else
        {
            //   hitPickUp.collider.gameObject.GetComponent<HighlightObject>().ResetMaterial();
        }
>>>>>>> James
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
