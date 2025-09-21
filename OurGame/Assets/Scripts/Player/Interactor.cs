using System.Collections;
using System.Runtime.CompilerServices;
using Unity.VisualScripting;
using UnityEditor.Timeline.Actions;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.Rendering;




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
    public RaycastHit raycastHit;

    public LayerMask ResponsiveMasks;

    public LayerMask StopMasks;


    [Header("Bools")]

    private bool _interactionInput;
    private bool _dropInput;
    private bool _throwInput;
    public bool _PlayerIsHidden = false;
    public bool hitObj = false;




    [Header("Scripts")]
    private PickUpSystem pickUpSystem;
    private ReticleManagement reticleManagement;
    private InnerDialouge innerDialouge;
    private HideAndShowPlayer hideAndShowPlayer;
    private DoorUnlocking doorUnlocking;

    private HighlightObject highlightObject;
    private EndDemo endDemoScript;
    private DialougeState startDialougeScript;

    [Header("InteractionVar")]
    public float _interactionDelay = 0f;
    private float _maxInteractionDelay = 0.5f;
    private float _interactionRange = 1.5f;
    public float hideDuration = 5f;
    private Collider _currentHideObj;
    RaycastHit lineCasthit;
    bool noLOS = false;
    bool hasChatted = false;

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
        Debug.DrawLine(transform.position, transform.forward * _interactionRange);


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


    private void OnDrawGizmos()
    {
        Gizmos.color = Color.green;
        Gizmos.DrawSphere(transform.position + transform.forward * _interactionRange, 0.3f);
    }





    public void OnInteractions(InputAction.CallbackContext context) { _interactionInput = context.ReadValueAsButton(); }
    public void OnDrop(InputAction.CallbackContext context) { _dropInput = context.ReadValueAsButton(); }
    public void OnThrow(InputAction.CallbackContext context) { _throwInput = context.ReadValueAsButton(); }


    void HandleInteractions()


    {

        if (!noLOS)
            reticleManagement.HandleTooltip();


        hitObj = Physics.SphereCast(transform.position, 0.3f, transform.forward, out raycastHit, _interactionRange, ResponsiveMasks);

        noLOS = Physics.Raycast(transform.position, transform.forward, out lineCasthit, _interactionRange / 3, StopMasks);

        string LayerName = "";
        if (hitObj & !noLOS)
        {
            LayerMask specifiedLayer = raycastHit.transform.gameObject.layer;
            LayerName = LayerMask.LayerToName(specifiedLayer.value);


        }



        /* if (!_PlayerIsHidden)
         {
             _isHideObject = Physics.Raycast(transform.position, transform.forward, out hitHideObj, _interactionRange / 2, hideAwayMask);
         }*/


        if (_interactionInput && _interactionDelay <= 0)
        {

            switch (LayerName)
            {
                case "interactionsMask":


                    if (raycastHit.collider.gameObject.TryGetComponent<EndDemo>(out endDemoScript))
                    {
                        innerDialouge.text.text = "You have found your brother that you lost!";
                        StartCoroutine(innerDialouge.InnerDialogueContorl());
                    }
                    else if (raycastHit.collider.gameObject.TryGetComponent<DialougeState>(out startDialougeScript) && hasChatted == false)
                    {

                        hasChatted = true;
                        startDialougeScript.StartDialouge();
                        return;
                    }
                    else
                    {
                        innerDialouge.text.text = "I need my ball...Get it this time...";
                    }

                    break;

                case "hidePlacesMask":
                    raycastHit.collider.gameObject.GetComponent<HideAndShowPlayer>().HidePlayer();
                    _PlayerIsHidden = true;
                    hideDuration = 5f;
                    _interactionDelay = 1f;

                    break;

                case "doorMask":
                    doorUnlocking.CanPlayerOpenDoor();
                    break;

                case "pickUpMask":
                    _interactionDelay = _maxInteractionDelay;
                    pickUpSystem.EquipItem();
                    break;


                default:
                    if (pickUpSystem.playerHands.childCount > 1)
                    {
                        pickUpSystem.DropItem();
                    }
                    return;

            }

        }
        if (_throwInput)
        {
            pickUpSystem.ThrowItem();
            //Play sound
        }

        //Show player after 5seconds, the limit of hiding
        if (hideDuration <= 0 && _PlayerIsHidden)
        {
            raycastHit.collider.gameObject.GetComponent<HideAndShowPlayer>().ShowPlayer();
            _PlayerIsHidden = false;
        }

        // Wait for 1 second, then check if interaction input and player is hidden
        if (_PlayerIsHidden)
        {
            StartCoroutine(WaitAndCheckHide());
        }





    }


    private IEnumerator WaitAndCheckHide()
    {
        yield return new WaitForSeconds(0.5f);
        if (_interactionInput && _PlayerIsHidden)
        {
            raycastHit.collider.gameObject.GetComponent<HideAndShowPlayer>().ShowPlayer();
            _PlayerIsHidden = false;
        }
    }






}
