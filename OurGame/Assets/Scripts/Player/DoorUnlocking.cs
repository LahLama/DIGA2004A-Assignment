using System;
using System.Collections.Generic;
using System.Data;
using UnityEngine;
using UnityEngine.Rendering;

public class DoorUnlocking : MonoBehaviour
{
    private Interactor _interactor;
    private PickUpSystem _pickUpSystem;
    private RaycastHit _hitDoorObj;

    private InnerDialouge _innerDialouge;
    private string[] DoorTags = { "ForGreenDoor", "ForRedDoor", "ForBlueDoor", "ForYellowDoor" };



    void Awake()
    {
        _interactor = GetComponent<Interactor>();
        _pickUpSystem = GetComponent<PickUpSystem>();
        _innerDialouge = GetComponent<InnerDialouge>();
    }

    void Update()
    {
        _hitDoorObj = _interactor.hitDoorObj;
    }
    public void CanPlayerOpenDoor()
    {
        var currentDoor = _hitDoorObj.collider;
        var playerHands = _pickUpSystem.playerHands;
        if (playerHands.childCount > 1)
        {
            var currentItem = _pickUpSystem.playerHands.GetChild(1);


            //Green Door
            if (currentItem.CompareTag(DoorTags[0]) && currentDoor.CompareTag(DoorTags[0]))
            {
                Destroy(currentItem.gameObject);
                Destroy(currentDoor.gameObject);
                Debug.Log(" GREEN DOOR");
            }

            //Red Door
            else if (currentItem.CompareTag(DoorTags[1]) && currentDoor.CompareTag(DoorTags[1]))
            {
                Destroy(currentItem.gameObject);
                Destroy(currentDoor.transform.parent.gameObject);

                Debug.Log(" RED DOOR");
            }

            //Blue Door
            else if (currentItem.CompareTag(DoorTags[2]) && currentDoor.CompareTag(DoorTags[2]))
            {
                Destroy(currentItem.gameObject);
                Destroy(currentDoor.gameObject);
                Debug.Log(" BLUE DOOR");
            }

            //Yellow Door
            else if (currentItem.CompareTag(DoorTags[3]) && currentDoor.CompareTag(DoorTags[3]))
            {
                Destroy(currentItem.gameObject);
                Destroy(currentDoor.gameObject);
                Debug.Log(" YELLOW DOOR");
            }



            else
            {
                _innerDialouge.text.text = "I gotta find the key.";
                StartCoroutine(_innerDialouge.InnerDialogueContorl());
                print("FIND THE KEY");
            }
        }
        else
        {
            _innerDialouge.text.text = "The door is locked.";
            StartCoroutine(_innerDialouge.InnerDialogueContorl());
            print("The door is locked");
        }


    }


}
