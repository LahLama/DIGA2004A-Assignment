using UnityEngine;
using System.Collections;
using UnityEngine.AI;
using UnityEditor;

/*
Title: How To Pick Up an Item - Unity
Author: Gunzz
Date:  Oct 10, 2022
Availability: https://www.youtube.com/watch?v=zEfahR66Pa8
*/

public class PickUpSystem : MonoBehaviour
{
    #region  Varibles
    public Transform playerHands;
    private Interactor _interactor;
    private NunAi _enemyAI;
    private Transform _pickUpsContatiner;
    private Vector3 _equippedItemScale;
    private Quaternion _equippedItemRotation;
    private RaycastHit _hitPickUp;
    public bool objHasBeenThrown = false;
    private ControllerRumble controller;
    private Transform HoldL;
    private PlayerStats playerStats;
    private Transform HoldR;

    #endregion

    #region UnityFunctions
    void Awake()
    {
        _interactor = GetComponent<Interactor>();
        playerHands = GameObject.FindWithTag("HoldingPos").transform;
        controller = GameObject.FindGameObjectWithTag("ControllerManager").GetComponent<ControllerRumble>();
        _enemyAI = GameObject.FindWithTag("NunEnemy").GetComponent<NunAi>();
        HoldL = GameObject.FindWithTag("Hold.L").transform;
        HoldR = GameObject.FindWithTag("Hold.R").transform;
        playerStats = GameObject.FindWithTag("PlayerStats").GetComponent<PlayerStats>();
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

            ParticleSystem particles;
            if (pickUpObj.TryGetComponent<ParticleSystem>(out particles))
            {
                ParticleSystem.MainModule particles_main = particles.main;
                particles_main.startColor = new Color(1f, 0.078f, 0.576f, 1f); // Specific pink (hex #FF1493)
                particles.Play();
            }


            //Switch off the gravity
            Destroy(pickUpObj.GetComponent<Rigidbody>());
            //Get the Orignal scale and rotation
            _equippedItemRotation = pickUpObj.transform.rotation;
            _equippedItemScale = pickUpObj.transform.localScale;

            //Reset scale and postion         
            pickUpObj.transform.localPosition = new Vector3(0f, 0f, 0f);
            pickUpObj.transform.localScale *= 1;
            pickUpObj.transform.rotation = Quaternion.identity;



            //Set the parent to the player
            _pickUpsContatiner = pickUpObj.transform.parent;
            pickUpObj.transform.SetParent(playerHands, false);




            //Animate a slight jiggle when picked up

            //Reposition items on player
            //if only 1 item -- Child1 in hand
            //if 2 items, item 1 in hand and item 2 in other hand
            SwapItems();



            //To ensure the object doesnt visually clip through walls
            pickUpObj.layer = LayerMask.NameToLayer("holdingMask");
            controller.RumblePusle(0.2f, 0.2f, 0.2f);
        }
        else
            return;
    }

    public void DropItem()
    {
        //Only if the player has something it thier hands
        if (playerHands.childCount > 0)
        {

            //Get the obj that is in the hands
            Transform equipedObj = playerHands.GetChild(0);

            //Enable the grabity
            equipedObj.gameObject.AddComponent<Rigidbody>();

            //Reset so that the player cant see the objects through walls
            equipedObj.gameObject.layer = LayerMask.NameToLayer("pickUpMask");

            // Place object infront of player
            Vector3 equipObjPos = equipedObj.transform.localPosition;
            equipObjPos = new Vector3(equipObjPos.x, equipObjPos.y + 1, equipObjPos.z);

            //Reset the player to the pickups element
            equipedObj.SetParent(_pickUpsContatiner, true);

            //Reset the scale and postion to its originals
            equipedObj.gameObject.transform.localScale = _equippedItemScale;
            equipedObj.gameObject.transform.rotation = _equippedItemRotation;



            Invoke("RepositionItems", 0.1f);
            return;

        }


    }

    public void ThrowItem()
    {
        if (playerHands.childCount > 0)
        {

            Transform equipedObj = playerHands.GetChild(0);

            Rigidbody rb = equipedObj.gameObject.AddComponent<Rigidbody>();
            rb.useGravity = true;

            Vector3 equipObjPos = equipedObj.transform.localPosition;
            equipObjPos = new Vector3(equipObjPos.x, equipObjPos.y + 1, equipObjPos.z);
            equipedObj.SetParent(_pickUpsContatiner, true);

            equipedObj.gameObject.layer = LayerMask.NameToLayer("pickUpMask");


            equipedObj.gameObject.transform.localScale = _equippedItemScale;
            equipedObj.gameObject.transform.rotation = _equippedItemRotation;

            rb.AddForce(playerHands.forward * 5f, ForceMode.Impulse);

            objHasBeenThrown = true;
            if (playerStats.playerLevel != PlayerStats.PlayerLevel.Tutorial && !_enemyAI._isGracePeriod)
                StartCoroutine(ThrowCooldown());

            Invoke("RepositionItems", 0.1f);
            return;
        }

        return;

    }

    private IEnumerator ThrowCooldown()
    {
        float timer = 5f;
        while (timer > 0f)
        {
            _enemyAI.agent.SetDestination(transform.position);
            timer -= Time.deltaTime;
            yield return null;
        }
        objHasBeenThrown = false;
    }

    private void RepositionItems()
    {
        if (playerHands.childCount > 1)
        {
            playerHands.GetChild(0).transform.position = HoldL.position;
            playerHands.GetChild(1).transform.position = HoldR.position;
        }
        else
        {
            playerHands.GetChild(0).transform.position = HoldR.position;
        }
    }

    public void SwapItems()
    {
        if (playerHands.childCount > 1)
        {
            playerHands.GetChild(1).SetAsFirstSibling();
            playerHands.GetChild(0).transform.position = HoldR.position;
            playerHands.GetChild(1).transform.position = HoldL.position;
        }
        else if (playerHands.childCount == 1)
        {
            playerHands.GetChild(0).transform.position = HoldR.position;
        }
        else
            return;

    }

}




