using UnityEngine;


public class DoorUnlocking : MonoBehaviour
{
    #region Varibles

    private Interactor _interactor;
    private PickUpSystem _pickUpSystem;
    private RaycastHit _hitDoorObj;
    private InnerDialouge _innerDialouge;
    private string[] _doorTags = { "ForGreenDoor", "ForRedDoor", "ForBlueDoor", "ForYellowDoor" };
    public GameObject[] RedDoors;

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

        if (playerHands.childCount > 0)
        {
            var currentItem = _pickUpSystem.playerHands.GetChild(0);
            //Compares if the door and handheld item matches tags

            //Green Door
            if (currentItem.CompareTag(_doorTags[0]) && currentDoor.CompareTag(_doorTags[0]))
            {
                Destroy(currentDoor.gameObject.transform.parent.gameObject);
                Destroy(currentItem.gameObject);
            }

            //Red Door
            else if (currentItem.CompareTag(_doorTags[1]) && currentDoor.CompareTag(_doorTags[1]))
            {
                Destroy(currentItem.gameObject);
                foreach (var Door in RedDoors)
                {
                    Destroy(Door);
                }

            }

            //Blue Door
            else if (currentItem.CompareTag(_doorTags[2]) && currentDoor.CompareTag(_doorTags[2]))
            {
                Destroy(currentItem.gameObject);
                Destroy(currentDoor.gameObject.transform.parent.gameObject);
            }

            //Yellow Door
            else if (currentItem.CompareTag(_doorTags[3]) && currentDoor.CompareTag(_doorTags[3]))
            {
                Destroy(currentItem.gameObject);
                Destroy(currentDoor.gameObject.transform.parent.gameObject);
            }

            //If its not the key
            else
            {
                _innerDialouge.text.text = "I gotta find the key.";
                StartCoroutine(_innerDialouge.InnerDialogueContorl());


                //Door Open


            }
        }
        else
        {
            //If the player doesnt have anything in hand
            _innerDialouge.text.text = "I need my item.";
            StartCoroutine(_innerDialouge.InnerDialogueContorl());

            // Test door animation

            if (currentDoor.TryGetComponent<Animator>(out Animator animator))

                if (animator != null)
                {
                    animator.SetTrigger("DoorOpen");
                    return;
                }
        }


    }


}
