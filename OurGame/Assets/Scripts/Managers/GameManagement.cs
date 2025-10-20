using UnityEngine;
using UnityEngine.InputSystem;

public class GameManagement : MonoBehaviour
{

    private bool EscapeButton;
    private Gamepad gamepad;
    public void OnExitGame(InputAction.CallbackContext context)
    {

        gamepad = Gamepad.current;


        if (gamepad != null)
            gamepad.SetMotorSpeeds(0, 0);

    }



}
