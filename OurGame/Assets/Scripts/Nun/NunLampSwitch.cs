using TreeEditor;
using UnityEngine;

public class NunLampSwitch : MonoBehaviour
{

    private interactionLamp _interactionLamp;
    void OnTriggerEnter(Collider col)
    {
        if (col.name == "Nun")
        {
            transform.parent.GetComponent<interactionLamp>().Interact();
        }

    }
}
