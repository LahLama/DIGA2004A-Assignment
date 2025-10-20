using UnityEngine;
using System.Collections.Generic;

public class DoorUnlocking : MonoBehaviour
{
    #region Varibles

    private Interactor _interactor;
    private PickUpSystem _pickUpSystem;
    private RaycastHit _hitDoorObj;
    private InnerDialouge _innerDialouge;
    private string[] _doorTags = { "ForGreenDoor", "ForRedDoor", "ForBlueDoor", "ForYellowDoor" };
    public GameObject[] RedDoors;
    // track which key types have been unlocked
    private HashSet<string> _unlockedKeys = new HashSet<string>();

    #endregion

    #region UnityFunctions
    void Awake()
    {
        _interactor = GetComponent<Interactor>();
        _pickUpSystem = GetComponent<PickUpSystem>();
        _innerDialouge = GetComponent<InnerDialouge>();
    }

    void Update()
    {
        _hitDoorObj = _interactor.raycastHit;
    }
    #endregion

    public void CanPlayerOpenDoor()
    {
        // safe-guard: make sure we actually hit something
        if (_hitDoorObj.collider == null) return;

        // Getting the door the player is looking at and the item the player is holding
        Collider currentDoor = _hitDoorObj.collider;
        var playerHands = _pickUpSystem.playerHands;

        // determine which door key type this door needs
        string requiredTag = null;
        foreach (var tag in _doorTags)
        {
            if (currentDoor.CompareTag(tag))
            {
                requiredTag = tag;
                break;
            }
        }

        // not a door we handle
        if (requiredTag == null) return;

        // If door type already unlocked, just play the animation repeatedly
        if (_unlockedKeys.Contains(requiredTag))
        {
            DoorAnim(currentDoor);
            if (requiredTag == _doorTags[1] && RedDoors != null) // RedDoors special group
            {
                foreach (var door in RedDoors)
                {
                    if (door == null) continue;
                    var anim = door.GetComponentInChildren<Animator>();
                    if (anim != null) anim.SetTrigger("DoorOpen");
                }
            }
            return;
        }

        // Player must have the correct key in hand to unlock
        if (playerHands.childCount > 0)
        {
            var currentItem = playerHands.GetChild(0);
            if (currentItem != null && currentItem.CompareTag(requiredTag))
            {
                // mark unlocked so doors can be opened any amount of times
                _unlockedKeys.Add(requiredTag);

                // play animation on this door
                DoorAnim(currentDoor);

                // special handling for red doors group
                if (requiredTag == _doorTags[1] && RedDoors != null)
                {
                    foreach (var door in RedDoors)
                    {
                        if (door == null) continue;
                        var anim = door.GetComponentInChildren<Animator>();
                        if (anim != null) anim.SetTrigger("DoorOpen");
                    }
                }

                // consume key (optional) - remove this line if you want the player to keep the key
                Destroy(currentItem.gameObject);

                return;
            }

            // player has something but not the right key
            _innerDialouge.text.text = "I gotta find the key.";
            StartCoroutine(_innerDialouge.InnerDialogueContorl());
        }
        else
        {
            // player has nothing in hand
            _innerDialouge.text.text = "I need my item.";
            StartCoroutine(_innerDialouge.InnerDialogueContorl());
        }
    }


    private void DoorAnim(Collider currentDoor)
    {
        if (currentDoor == null) return;

        // try to find an Animator in the parent chain first, then children
        Animator animator = currentDoor.GetComponentInParent<Animator>();
        if (animator == null)
            animator = currentDoor.GetComponentInChildren<Animator>();

        if (animator != null)
        {
            animator.SetTrigger("DoorOpen");
            return;
        }
        else
        {
            Debug.LogWarning("No Animator found for door: " + currentDoor.name);
        }
    }

}
