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
    //https://www.youtube.com/watch?v=QYpWYZq2I6E


    [Header("Masks")]
    public LayerMask interactionsMask;
    public LayerMask pickUpMask;
    public LayerMask HoldMask;
    public LayerMask HideAwayMask;

    [Header("Tooltips")]
    public Image CanInteractToolTip;
    public GameObject CanInteractText;
    public GameObject innerDialougePanel;
    public RaycastHit hitGeneric;
    public RaycastHit hitPickUp;
    public RaycastHit hitHideObj;
    public Transform PlayerHands;
    public Transform PickUpsContatiner;
    private bool _interactionInput;
    private bool _dropInput;
    bool _isGenericObject;
    bool _isPickUpObject;
    bool _isHideObject;

    private Vector3 _equippedItemScale;
    private Quaternion _equippedItemRotation;

    void Update() { HandleInteractions(); }

    public void OnInteractions(InputAction.CallbackContext context) { _interactionInput = context.ReadValueAsButton(); }
    public void OnDrop(InputAction.CallbackContext context) { _dropInput = context.ReadValueAsButton(); }


    void HandleInteractions()
    {


        _isGenericObject = Physics.Raycast(transform.position, transform.forward, out hitGeneric, 3.5f, interactionsMask);
        _isPickUpObject = Physics.Raycast(transform.position, transform.forward, out hitPickUp, 3.5f, pickUpMask);

        HandleTooltip();

        if (_interactionInput)
        {

            if (_isGenericObject)
            {
                Debug.Log("hit _isGenericObject");
                Debug.DrawRay(transform.position, transform.TransformDirection(Vector3.forward) * hitGeneric.distance, Color.green);
                StartCoroutine(InnerDialogueContorl());

            }

            else if (_isPickUpObject)
            {
                Debug.Log("hit _isPickUpObject");
                Debug.DrawRay(transform.position, transform.TransformDirection(Vector3.forward) * hitPickUp.distance, Color.blue);

                EquipItem();

            }

            else if (_isHideObject)
            {

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
        if (PlayerHands.childCount > 1)
        {
            DropItem();
        }
        GameObject __pickUpObj = hitPickUp.collider.gameObject;
        Destroy(__pickUpObj.GetComponent<Rigidbody>());
        __pickUpObj.transform.localPosition = new Vector3(0f, 0f, 0f);
        __pickUpObj.transform.SetParent(PlayerHands, false);

        _equippedItemScale = __pickUpObj.gameObject.transform.localScale;
        _equippedItemRotation = __pickUpObj.gameObject.transform.rotation;
        __pickUpObj.layer = LayerMask.NameToLayer("holdingMask");
    }

    private void DropItem()
    {
        if (PlayerHands.childCount > 1)
        {
            Transform __equipedObj = PlayerHands.GetChild(1);
            Vector3 __equipObjPos = __equipedObj.transform.localPosition;
            __equipObjPos = new Vector3(__equipObjPos.x, __equipObjPos.y + 1, __equipObjPos.z);
            __equipedObj.SetParent(PickUpsContatiner, true);
            __equipedObj.gameObject.AddComponent<Rigidbody>();

            __equipedObj.gameObject.layer = LayerMask.NameToLayer("pickUpMask");
            __equipedObj.gameObject.transform.localScale = _equippedItemScale;
            __equipedObj.gameObject.transform.rotation = _equippedItemRotation;

        }


    }

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

    public void HidePlayer()
    {

    }

    public void ShowPlayer()
    {

    }


}
