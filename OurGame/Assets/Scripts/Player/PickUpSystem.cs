using UnityEditor;
using UnityEngine;

public class PickUpSystem : Interactor
{
    public GameObject pickUpObject;


    void Start()
    {
        pickUpObject.gameObject.SetActive(false);
    }

    void Update()
    {
        if (hitInfo.collider.gameObject.CompareTag("PickUp"))
        {
            print("this is a pick up -able obj");
        }
    }


}
