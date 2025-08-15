using Unity.VisualScripting;
using UnityEditor;
using UnityEngine;


/*
VARIBLE_NAME = External Var
_VaribleName = private Var
varibleName = public/temporary Var


*/
public class PickUpSystem : MonoBehaviour
{
    #region  Varibles
    private Interactor _interactor;
    public Transform playerHands;
    public Transform pickUpsContatiner;
    private Vector3 _equippedItemScale;
    private Quaternion _equippedItemRotation;
    public RaycastHit _hitPickUp;

    #endregion

    void Awake()
    {
        _interactor = GetComponent<Interactor>();
    }


    private void Update()
    {
        _hitPickUp = _interactor.hitPickUp;
    }
    public void EquipItem()
    {
        if (playerHands.childCount > 1)
        {
            DropItem();
        }
        if (_hitPickUp.collider)
        {
            GameObject pickUpObj = _hitPickUp.collider.gameObject;  //
            Destroy(pickUpObj.GetComponent<Rigidbody>());
            pickUpObj.transform.localPosition = new Vector3(0f, 0f, 0f);
            pickUpObj.transform.SetParent(playerHands, false);

            _equippedItemScale = pickUpObj.transform.localScale;
            pickUpObj.transform.localScale = new Vector3(0.2f, 0.2f, 0.2f);


            _equippedItemRotation = pickUpObj.transform.rotation;

            //Animate a slight jiggle when picked up
            _equippedItemRotation = Quaternion.Euler(0, 0, 0);


            pickUpObj.layer = LayerMask.NameToLayer("holdingMask");
        }
        else
            return;
    }

    public void DropItem()
    {
        if (playerHands.childCount > 1)
        {
            Transform equipedObj = playerHands.GetChild(1);
            Vector3 equipObjPos = equipedObj.transform.localPosition;
            equipObjPos = new Vector3(equipObjPos.x, equipObjPos.y + 1, equipObjPos.z);
            equipedObj.SetParent(pickUpsContatiner, true);
            equipedObj.gameObject.AddComponent<Rigidbody>();

            equipedObj.gameObject.layer = LayerMask.NameToLayer("pickUpMask");
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
            equipedObj.SetParent(pickUpsContatiner, true);
            Rigidbody rb = equipedObj.gameObject.AddComponent<Rigidbody>();
            rb.useGravity =true;;
            rb.AddForce(playerHands.forward * 10f, ForceMode.Impulse);

            equipedObj.gameObject.layer = LayerMask.NameToLayer("pickUpMask");
            equipedObj.gameObject.transform.localScale = _equippedItemScale;
            equipedObj.gameObject.transform.rotation = _equippedItemRotation;

            return;

        }
    }


}
