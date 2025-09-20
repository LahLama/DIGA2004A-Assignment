using UnityEngine;


public class DoorUnlocking : MonoBehaviour
{
    #region Varibles

    private Interactor _interactor;
    private PickUpSystem _pickUpSystem;
    private RaycastHit _hitDoorObj;
    private InnerDialouge _innerDialouge;
    private string[] _doorTags = { "ForGreenDoor", "ForRedDoor", "ForBlueDoor", "ForYellowDoor" };

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
        //Getting the door the player is looking at
        // and gets the item the player is holding
        var currentDoor = _hitDoorObj.collider;
        var playerHands = _pickUpSystem.playerHands;

        if (playerHands.childCount > 1)
        {
            var currentItem = _pickUpSystem.playerHands.GetChild(1);
            //Compares if the door and handheld item matches tags

            //Green Door
            if (currentItem.CompareTag(_doorTags[0]) && currentDoor.CompareTag(_doorTags[0]))
            {
                Destroy(currentDoor.gameObject);
                Destroy(currentItem.gameObject);
            }

            //Red Door
            else if (currentItem.CompareTag(_doorTags[1]) && currentDoor.CompareTag(_doorTags[1]))
            {
                Destroy(currentItem.gameObject);
                Destroy(currentDoor.transform.parent.parent.gameObject);
            }

            //Blue Door
            else if (currentItem.CompareTag(_doorTags[2]) && currentDoor.CompareTag(_doorTags[2]))
            {
                Destroy(currentItem.gameObject);
                Destroy(currentDoor.gameObject);
            }

            //Yellow Door
            else if (currentItem.CompareTag(_doorTags[3]) && currentDoor.CompareTag(_doorTags[3]))
            {
                Destroy(currentItem.gameObject);
                Destroy(currentDoor.gameObject);
            }

            //If its not the key
            else
            {
                _innerDialouge.text.text = "I gotta find the key.";
                StartCoroutine(_innerDialouge.InnerDialogueContorl());
            }
        }
        else
        {
            //If the player doesnt have anything in hand
            _innerDialouge.text.text = "The door is locked.";
            StartCoroutine(_innerDialouge.InnerDialogueContorl());
        }


    }


}
