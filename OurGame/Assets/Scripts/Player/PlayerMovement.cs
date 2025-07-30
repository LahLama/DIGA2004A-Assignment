using UnityEngine;
using UnityEngine.InputSystem;
public class PlayerMovementScript : MonoBehaviour
{
    public void Movement(InputAction.CallbackContext context)
    {
        print("movement: " + context.phase);
    }

    public void Rotation(InputAction.CallbackContext context)
    {
        print("rotation: " + context.phase);

    }
}
