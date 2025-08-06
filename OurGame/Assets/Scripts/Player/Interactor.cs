using UnityEngine;
using UnityEngine.UI;
using UnityEngine.InputSystem;
using System.Collections;
using UnityEditor.ShaderGraph;
using Unity.VisualScripting;

public class Interactor : MonoBehaviour
{

    //Drop": Look in pickUp parent, look a children tha matches current enabled one on the player, if yes: diabble On player, set pos of 'drop" to be infront(only 1 mili, cause Out of bounds things) of player then enable dropped item

    // /https://www.youtube.com/watch?v=cUf7FnNqv7U
    //https://youtu.be/zEfahR66Pa8

    public LayerMask interactionsMask;
    public LayerMask pickUpMask;
    public Image CanInteractToolTip;
    public GameObject CanInteractText;
    public GameObject innerDialougePanel;
    public RaycastHit hitInfo;
    public Transform PlayerHands;
    public Transform PickUpsContatiner;
    private bool _interactionInput;
    private bool _dropInput;
    bool _isGenericObject;
    bool _isPickUpObject;

    private Vector3 _equippedItemScale;

    void Update() { HandleInteractions(); }

    public void OnInteractions(InputAction.CallbackContext context) { _interactionInput = context.ReadValueAsButton(); }
    public void OnDrop(InputAction.CallbackContext context) { _dropInput = context.ReadValueAsButton(); }


    void HandleInteractions()
    {

        _isGenericObject = Physics.Raycast(transform.position, transform.forward, out hitInfo, 3.5f, interactionsMask);
        _isPickUpObject = Physics.Raycast(transform.position, transform.forward, out hitInfo, 3.5f, pickUpMask);
        HandleTooltip();

        if (_interactionInput)
        {

            if (_isGenericObject)
            {
                Debug.Log("hit _isGenericObject");
                Debug.DrawRay(transform.position, transform.TransformDirection(Vector3.forward) * hitInfo.distance, Color.green);
                StartCoroutine(InnerDialogueContorl());

            }

            else if (_isPickUpObject)
            {
                Debug.Log("hit _isPickUpObject");
                Debug.DrawRay(transform.position, transform.TransformDirection(Vector3.forward) * hitInfo.distance, Color.blue);

                EquipItem();

            }

            else
            {
                Debug.Log("NULL");
                Debug.DrawRay(transform.position, transform.TransformDirection(Vector3.forward) * 5f, Color.red);
            }
        }


        if (_dropInput)
        {
            DropItem();
        }


    }

    private void EquipItem()
    {
        if (PlayerHands.childCount > 0)
        {
            DropItem();
        }
        GameObject PickUpObj = hitInfo.collider.gameObject;
        Destroy(PickUpObj.GetComponent<Rigidbody>());
        PickUpObj.transform.localPosition = new Vector3(0f, 0f, 0f);
        PickUpObj.transform.SetParent(PlayerHands, false);

        _equippedItemScale = PickUpObj.gameObject.transform.localScale;
    }

    private void DropItem()
    {
        if (PlayerHands.childCount > 0)
        {
            Transform EquipedObj = PlayerHands.GetChild(0);
            Vector3 EquipObjPos = EquipedObj.transform.localPosition;
            EquipObjPos = new Vector3(EquipObjPos.x, EquipObjPos.y + 1, EquipObjPos.z);
            EquipedObj.SetParent(PickUpsContatiner, true);
            EquipedObj.gameObject.AddComponent<Rigidbody>();
            EquipedObj.gameObject.transform.localScale = _equippedItemScale;

        }


    }


    /*

    private void CheckAndSetPickUpObj()
    {

        GameObject PickUpObj = hitInfo.collider.gameObject;
        Debug.Log("Name is: " + PickUpObj.name);

        transform.GetChild(0);

        foreach (Transform child in transform.GetChild(0))
        {
            if (PickUpObj.name == child.name)
            {
                PickUpObj.SetActive(false);
                child.gameObject.SetActive(true);
            }
        }
    }


    private void DisableAllOnPlayerItems()
    {
        foreach (Transform child in transform.GetChild(0))
        {
            child.gameObject.SetActive(false);
        }
    }*/



    private void HandleTooltip()
    {
        if (!(_isGenericObject || _isPickUpObject))
        {
            //Nothing to interact with 
            CanInteractToolTip.color = new Color(1f, 1, 1f, 0.5f);
            CanInteractText.SetActive(false);

        }
        else
        {
            //Can interact with something
            CanInteractToolTip.color = new Color(1f, 1, 1f, 1f);
            CanInteractText.SetActive(true);

        }
    }



    private IEnumerator InnerDialogueContorl()
    {
        innerDialougePanel.GetComponent<Image>().CrossFadeAlpha(1f, 0.2f, true);
        yield return new WaitForSeconds(0.22f);
        innerDialougePanel.SetActive(true);

        yield return new WaitForSeconds(2f);
        innerDialougePanel.GetComponent<Image>().CrossFadeAlpha(0f, 0.3f, true);

        yield return new WaitForSeconds(0.32f);
        innerDialougePanel.SetActive(false);

    }






}
