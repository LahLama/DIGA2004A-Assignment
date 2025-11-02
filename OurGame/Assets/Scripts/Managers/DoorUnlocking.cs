using UnityEngine;
using System.Collections.Generic;

public class DoorUnlocking : MonoBehaviour
{
    #region Varibles

    private Interactor _interactor; // reference to raycast interaction system
    private PickUpSystem _pickUpSystem; // reference to item holding system
    private RaycastHit _hitDoorObj; // stores what interact ray is hitting
    private InnerDialouge _innerDialouge; // handles player dialogue text
    private string[] _doorTags = { "ForGreenDoor", "ForRedDoor", "ForBlueDoor", "ForYellowDoor" }; // door types
    public GameObject[] RedDoors; // linked red doors that open together

    // Stores keys that have been used once and now permanently unlock doors of that colour
    private HashSet<string> _unlockedKeys = new HashSet<string>();
    // Fog control (increase when a new door type is unlocked)
    [SerializeField] private float fogIncreaseAmount = 0.2f;
    [SerializeField] private float maxFogDensity = 1.0f;

    #endregion

    #region UnityFunctions
    void Awake()
    {
        // Cache component references to avoid expensive GetComponent calls later
        _interactor = GetComponent<Interactor>();
        _pickUpSystem = GetComponent<PickUpSystem>();
        _innerDialouge = GetComponent<InnerDialouge>();

        RenderSettings.fog = true;

    }

    void Update()
    {
        // Always track what object the player is currently aiming at
        _hitDoorObj = _interactor.raycastHit;
    }
    #endregion

    public void CanPlayerOpenDoor()
    {
        // Make sure we are hitting something before continuing
        if (_hitDoorObj.collider == null) return;

        // Store the door collider and the object player is holding
        Collider currentDoor = _hitDoorObj.collider;
        var playerHands = _pickUpSystem.playerHands;

        // Detect door type based on tag from supported list
        string requiredTag = null;
        foreach (var tag in _doorTags)
        {
            if (currentDoor.CompareTag(tag))
            {
                requiredTag = tag; // Found required key tag
                break;
            }
        }

        // Not a recognised door, so do nothing
        if (requiredTag == null) return;

        // If door type already unlocked once, allow free access forever
        if (_unlockedKeys.Contains(requiredTag))
        {
            DoorAnim(currentDoor); // Play open animation again

            // Special case for red doors that open as a group
            if (requiredTag == _doorTags[1] && RedDoors != null)
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

        // If player is holding something
        if (playerHands.childCount > 0)
        {
            var currentItem = playerHands.GetChild(0); // Item in hand

            // If player is holding the correct key
            if (currentItem != null && currentItem.CompareTag(requiredTag))
            {
                // Mark this key/door type as unlocked permanently
                _unlockedKeys.Add(requiredTag);
                IncreaseFog();
                currentDoor.GetComponentInParent<IInteractables>().Interact();
                // Trigger opening animation
                DoorAnim(currentDoor);

                // Open all red doors as linked set
                if (requiredTag == _doorTags[1] && RedDoors != null)
                {
                    foreach (var door in RedDoors)
                    {
                        if (door == null) continue;
                        var anim = door.GetComponentInChildren<Animator>();
                        if (anim != null) anim.SetTrigger("DoorOpen");
                    }
                }

                // Remove key from players hands if intended
                Destroy(currentItem.gameObject);

                return;
            }

            // Player is holding wrong item
            _innerDialouge.text.text = "I gotta find the key.";
            StartCoroutine(_innerDialouge.InnerDialogueContorl());
        }
        else
        {
            // Player holding nothing
            _innerDialouge.text.text = "I need my item.";
            StartCoroutine(_innerDialouge.InnerDialogueContorl());
        }
    }

    private void DoorAnim(Collider currentDoor)
    {
        // Try to find door Animator either parent or children
        if (currentDoor == null) return;

        Animator animator = currentDoor.GetComponentInParent<Animator>();
        if (animator == null)
            animator = currentDoor.GetComponentInChildren<Animator>();

        // Trigger door animation if found
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
    private void IncreaseFog()
    {
        // enable fog if not already
        RenderSettings.fog = true;

        // increase density and clamp to max
        float newDensity = RenderSettings.fogDensity + fogIncreaseAmount;
        RenderSettings.fogDensity = Mathf.Clamp(newDensity, 0f, maxFogDensity);

        // optional: log for debugging
        Debug.Log($"Fog increased to {RenderSettings.fogDensity:F2}");
    }
}
