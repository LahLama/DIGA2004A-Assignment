using UnityEngine;

public class interactionLamp : MonoBehaviour, IInteractables
{


    public void Interact()
    {
        transform.GetChild(0).TryGetComponent<Light>(out Light lampLight);
        if (lampLight != null)
        {
            lampLight.enabled = !lampLight.enabled;
        }
    }
}
