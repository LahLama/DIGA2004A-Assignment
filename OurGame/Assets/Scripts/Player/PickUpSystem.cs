using UnityEngine;
using System.Collections;
using UnityEngine.AI;
using UnityEditor;
using Mono.Cecil.Cil;

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
    private Quaternion _equippedItemRotation;
    private RaycastHit _hitPickUp;
    public bool objHasBeenThrown = false;
    private ControllerRumble controller;
    private Transform HoldL;
    private PlayerStats playerStats;
    private Transform HoldR;

    [Header("SFX (optional)")]
    [SerializeField] private AudioClip pickupSfx;
    [SerializeField] private AudioClip throwSfx;
    [SerializeField] private AudioClip dropSfx;
    [SerializeField] private AudioClip swapSfx;

    // Optional: your SoundManager script (if available). Will try to auto-find in Awake if null.
    private SoundManager _soundManager;

    #endregion

    #region UnityFunctions
    void Awake()
    {
        _interactor = GetComponent<Interactor>();
        playerHands = GameObject.FindWithTag("HoldingPos").transform;
        controller = GameObject.FindAnyObjectByType<ControllerRumble>();
        _enemyAI = GameObject.FindAnyObjectByType<NunAi>();
        HoldL = GameObject.FindWithTag("Hold.L").transform;
        HoldR = GameObject.FindWithTag("Hold.R").transform;
        playerStats = GameObject.FindAnyObjectByType<PlayerStats>();

        if (_soundManager == null)
            _soundManager = GameObject.FindAnyObjectByType<SoundManager>();
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
            //Reset scale and postion         
            pickUpObj.transform.localPosition = new Vector3(0f, 0f, 0f);
            pickUpObj.transform.rotation = Quaternion.identity;
            //Set the parent to the player
            _pickUpsContatiner = pickUpObj.transform.parent;
            SetParentPreserveWorldScale(pickUpObj.transform, playerHands, false);


            SwapItems();



            // To ensure the object and all its children don't visually clip through walls
            int holdingLayer = LayerMask.NameToLayer("holdingMask");
            foreach (Transform t in pickUpObj.GetComponentsInChildren<Transform>(true))
            {
                t.gameObject.layer = holdingLayer;
            }
            controller.RumblePusle(0.2f, 0.2f, 0.2f);

            // Play pickup sound (SoundManager preferred, fallback to clip)
            if (_soundManager != null)
                _soundManager.Play(pickupSfx);
            else if (pickupSfx != null)
                AudioSource.PlayClipAtPoint(pickupSfx, transform.position);
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

            int pickUpLayer = LayerMask.NameToLayer("pickUpMask");
            foreach (Transform t in equipedObj.GetComponentsInChildren<Transform>(true))
            {
                t.gameObject.layer = pickUpLayer;
            }


            //Reset the player to the pickups element
            SetParentPreserveWorldScale(equipedObj, _pickUpsContatiner, true);



            // Play drop sound
            if (_soundManager != null)
                _soundManager.Play(dropSfx);
            else if (dropSfx != null)
                AudioSource.PlayClipAtPoint(dropSfx, transform.position);

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
            SetParentPreserveWorldScale(equipedObj, _pickUpsContatiner, true);

            int pickUpLayer = LayerMask.NameToLayer("pickUpMask");
            foreach (Transform t in equipedObj.GetComponentsInChildren<Transform>(true))
            {
                t.gameObject.layer = pickUpLayer;
            }


            equipedObj.gameObject.transform.rotation = _equippedItemRotation;

            rb.AddForce(playerHands.forward * 5f, ForceMode.Impulse);

            // Play throw sound
            if (_soundManager != null)
                _soundManager.Play(throwSfx);
            else if (throwSfx != null)
                AudioSource.PlayClipAtPoint(throwSfx, transform.position);

            objHasBeenThrown = true;
            if (playerStats.playerLevel != PlayerStats.PlayerLevel.Tutorial && !_enemyAI._isGracePeriod)
                StartCoroutine(ThrowCooldown(equipedObj.gameObject));

            Invoke("RepositionItems", 0.1f);
            return;
        }

        return;

    }

    private IEnumerator ThrowCooldown(GameObject gameObject)
    {
        float timer = 5f;
        while (timer > 0f)
        {
            _enemyAI.agent.SetDestination(gameObject.transform.position);
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
            //            playerHands.GetChild(0).transform.position = HoldR.position;
        }
    }

    public void SwapItems()
    {
        if (playerHands.childCount > 1)
        {
            playerHands.GetChild(1).SetAsFirstSibling();
            playerHands.GetChild(0).transform.position = HoldR.position;
            playerHands.GetChild(1).transform.position = HoldL.position;

            // Play swap sound
            if (_soundManager != null)
                _soundManager.Play(swapSfx);
            else if (swapSfx != null)
                AudioSource.PlayClipAtPoint(swapSfx, transform.position);
        }
        else if (playerHands.childCount == 1)
        {
            playerHands.GetChild(0).transform.position = HoldR.position;
        }
        else
            return;

    }
    private void SetParentPreserveWorldScale(Transform child, Transform parent, bool worldPositionStays)
    {
        if (child == null)
            return;

        Vector3 worldScale = child.lossyScale;
        child.SetParent(parent, worldPositionStays);

        if (parent == null)
        {
            child.localScale = worldScale;
            return;
        }

        Vector3 parentScale = parent.lossyScale;
        child.localScale = new Vector3(
            parentScale.x != 0f ? worldScale.x / parentScale.x : worldScale.x,
            parentScale.y != 0f ? worldScale.y / parentScale.y : worldScale.y,
            parentScale.z != 0f ? worldScale.z / parentScale.z : worldScale.z
        );
    }
}




