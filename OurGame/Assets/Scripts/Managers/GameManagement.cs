using UnityEngine;
using UnityEngine.InputSystem;

public class GameManagement : MonoBehaviour
{

    private bool EscapeButton; // Tracks Escape button state if needed later
    private Gamepad gamepad; // Reference to the current gamepad

    public void OnExitGame(InputAction.CallbackContext context)
    {
        // Always check for the current active gamepad before using vibration API
        gamepad = Gamepad.current;

        // If a controller is connected, make sure vibration stops when exiting game state
        if (gamepad != null)
            gamepad.SetMotorSpeeds(0, 0); // Stops both low and high motors
    }

}
