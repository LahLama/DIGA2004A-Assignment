using UnityEngine;

/*
Title: Creating a Horror Game in Unity - Part 6: Hiding System (JavaScript)
Author: SpeedTutor
Date:  Jun 10, 2014
Availability: https://www.youtube.com/watch?v=zEfahR66Pa8
*/

public class PickUpSystem : MonoBehaviour
{
    #region  Varibles
    public Transform playerHands;
    private Interactor _interactor;
    private Transform _pickUpsContatiner;
    private Vector3 _equippedItemScale;
    private Quaternion _equippedItemRotation;
    private RaycastHit _hitPickUp;


    #endregion

    #region UnityFunctions
    void Awake()
    {
        _interactor = GetComponent<Interactor>();
        playerHands = GameObject.FindWithTag("HoldingPos").transform;
        _pickUpsContatiner = GameObject.FindWithTag("PickUps").transform;
    }


    private void Update()
    {
        _hitPickUp = _interactor.raycastHit;
    }
    #endregion

    public void EquipItem()
    {

        //If the player has an item, drop that item
        //Only if the player has an open slot then reparent the object to the player
        if (playerHands.childCount > 1)
        {
            DropItem();
        }
        if (_hitPickUp.collider)
        {
            //Get the obj that will be picked up
            GameObject pickUpObj = _hitPickUp.collider.gameObject;

            //Switch off the gravity
            Destroy(pickUpObj.GetComponent<Rigidbody>());
            //Get the Orignal scale and rotation
            _equippedItemRotation = pickUpObj.transform.rotation;
            _equippedItemScale = pickUpObj.transform.localScale;

            //Reset scale and postion         
            pickUpObj.transform.localPosition = new Vector3(0f, 0f, 0f);
            pickUpObj.transform.localScale = new Vector3(0.2f, 0.2f, 0.2f);

            //Set the parent to the player
            pickUpObj.transform.SetParent(playerHands, false);

            //Animate a slight jiggle when picked up


            //To ensure the object doesnt visually clip through walls
            pickUpObj.layer = LayerMask.NameToLayer("holdingMask");
        }
        else
            return;
    }

    public void DropItem()
    {
        //Only if the player has something it thier hands
        if (playerHands.childCount > 1)
        {
            //Get the obj that is in the hands
            Transform equipedObj = playerHands.GetChild(1);

            // Place object infront of player
            Vector3 equipObjPos = equipedObj.transform.localPosition;
            equipObjPos = new Vector3(equipObjPos.x, equipObjPos.y + 1, equipObjPos.z);

            //Reset the player to the pickups element
            equipedObj.SetParent(_pickUpsContatiner, true);

            //Enable the grabity
            equipedObj.gameObject.AddComponent<Rigidbody>();

            //Reset so that the player cant see the objects through walls
            equipedObj.gameObject.layer = LayerMask.NameToLayer("pickUpMask");

            //Reset the scale and postion to its originals
            equipedObj.gameObject.transform.localScale = _equippedItemScale;
            equipedObj.gameObject.transform.rotation = _equippedItemRotation;



            return;

        }


    }

    public void ThrowItem()
    {
        if (playerHands.childCount > 1)
        {
            Transform equipedObj = playerHands.GetChild(1);
            Vector3 equipObjPos = equipedObj.transform.localPosition;
            equipObjPos = new Vector3(equipObjPos.x, equipObjPos.y + 1, equipObjPos.z);
            equipedObj.SetParent(_pickUpsContatiner, true);
            Rigidbody rb = equipedObj.gameObject.AddComponent<Rigidbody>();
            rb.useGravity = true; ;

            equipedObj.gameObject.layer = LayerMask.NameToLayer("pickUpMask");

            rb.AddForce(playerHands.forward * 10f, ForceMode.Impulse);
            equipedObj.gameObject.transform.localScale = _equippedItemScale;
            equipedObj.gameObject.transform.rotation = _equippedItemRotation;



            return;

        }
    }


}
