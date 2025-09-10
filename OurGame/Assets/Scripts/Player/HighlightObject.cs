using UnityEngine;
using UnityEngine.InputSystem;

public class HighlightObject : MonoBehaviour
{
    private Material _defaultMat;
    private Material _currentMat;
    public Material changedMat;
    private Interactor interactor;
    private bool _hasChangedMaterial;



    public void ChangeMaterial()
    {
        _defaultMat = GetComponent<Renderer>().material;
        Debug.Log(interactor.hitPickUp.collider.gameObject.name);
        interactor.hitPickUp.collider.gameObject.GetComponent<MeshRenderer>().material = changedMat;
    }

    public void ResetMaterial()
    {

        interactor.hitPickUp.collider.gameObject.GetComponent<MeshRenderer>().material = _defaultMat;
    }
}
