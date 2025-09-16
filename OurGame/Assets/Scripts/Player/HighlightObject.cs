using UnityEngine;


public class HighlightObject : MonoBehaviour
{

    private Interactor interactor;




    public void ChangeMaterial()
    {

        // interactor.hitPickUp.collider.gameObject.GetComponent<MeshRenderer>().material = changedMat;
    }

    public void ResetMaterial()
    {

        // interactor.hitPickUp.collider.gameObject.GetComponent<MeshRenderer>().material = _defaultMat;
    }
}
