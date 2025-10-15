using UnityEngine;

public class interactionLamp : MonoBehaviour, IInteractables
{


    public void Interact()
    {
        Light light = transform.GetChild(0).GetComponent<Light>();
        light.enabled = !light.isActiveAndEnabled;

    }
}
