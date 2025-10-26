using UnityEngine;

public class NunLampSwitch : MonoBehaviour
{
    private interactionLamp _interactionLamp; // Reference to the lamp interaction script

    void OnTriggerEnter(Collider col)
    {
        // Check if the object entering the trigger is the nun
        if (col.name == "Nun")
        {
            // Call the interaction method on the parent lamp
            transform.parent.GetComponent<interactionLamp>().Interact();
        }
    }
}
