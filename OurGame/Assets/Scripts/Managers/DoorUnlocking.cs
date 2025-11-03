using UnityEngine;
using System.Collections.Generic;

public class DoorUnlocking : MonoBehaviour
{
    #region Variables

    private Interactor _interactor; // reference to raycast interaction system
    private PickUpSystem _pickUpSystem; // reference to item holding system
    private RaycastHit _hitDoorObj; // stores what interact ray is hitting
    private InnerDialouge _innerDialouge; // handles player dialogue text
    private string[] _doorTags = { "ForGreenDoor", "ForRedDoor", "ForBlueDoor", "ForYellowDoor" }; // door types
    public GameObject[] RedDoors; // linked red doors that open together
    [SerializeField] private EndDemo endDemo; // reference to end demo video trigger

    private HashSet<string> _unlockedKeys = new HashSet<string>(); // permanently unlocked keys
    [SerializeField] private float fogIncreaseAmount = 0.2f;
    [SerializeField] private float maxFogDensity = 1.0f;

    #endregion

    #region UnityFunctions
    void Awake()
    {
        _interactor = GetComponent<Interactor>();
        _pickUpSystem = GetComponent<PickUpSystem>();
        _innerDialouge = GetComponent<InnerDialouge>();

        RenderSettings.fog = true;
    }

    void Update()
    {
        _hitDoorObj = _interactor.raycastHit;
    }
    #endregion

    public void CanPlayerOpenDoor()
    {
        if (_hitDoorObj.collider == null) return;

        Collider currentDoor = _hitDoorObj.collider;
        var playerHands = _pickUpSystem.playerHands;

        string requiredTag = null;
        foreach (var tag in _doorTags)
        {
            if (currentDoor.CompareTag(tag))
            {
                requiredTag = tag;
                break;
            }
        }

        if (requiredTag == null) return;

        if (_unlockedKeys.Contains(requiredTag))
        {
            DoorAnim(currentDoor);

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

        if (playerHands.childCount > 0)
        {
            var currentItem = playerHands.GetChild(0);

            if (currentItem != null && currentItem.CompareTag(requiredTag))
            {
                _unlockedKeys.Add(requiredTag);
                IncreaseFog();
                currentDoor.GetComponentInParent<IInteractables>().Interact();
                DoorAnim(currentDoor);

                if (requiredTag == _doorTags[1] && RedDoors != null)
                {
                    foreach (var door in RedDoors)
                    {
                        if (door == null) continue;
                        var anim = door.GetComponentInChildren<Animator>();
                        if (anim != null) anim.SetTrigger("DoorOpen");
                    }
                }

                Destroy(currentItem.gameObject);

                if (requiredTag == "ForYellowDoor" && endDemo != null)
                {
                    endDemo.Interact();
                }

                return;
            }

            _innerDialouge.text.text = "I gotta find the key.";
            StartCoroutine(_innerDialouge.InnerDialogueContorl());
        }
        else
        {
            _innerDialouge.text.text = "I need my item.";
            StartCoroutine(_innerDialouge.InnerDialogueContorl());
        }
    }

    private void DoorAnim(Collider currentDoor)
    {
        if (currentDoor == null) return;

        Animator animator = currentDoor.GetComponentInParent<Animator>();
        if (animator == null)
            animator = currentDoor.GetComponentInChildren<Animator>();

        if (animator != null)
        {
            animator.SetTrigger("DoorOpen");
        }
        else
        {
            Debug.LogWarning("No Animator found for door: " + currentDoor.name);
        }
    }

    private void IncreaseFog()
    {
        RenderSettings.fog = true;

        float newDensity = RenderSettings.fogDensity + fogIncreaseAmount;
        RenderSettings.fogDensity = Mathf.Clamp(newDensity, 0f, maxFogDensity);

        Debug.Log($"Fog increased to {RenderSettings.fogDensity:F2}");
    }
}