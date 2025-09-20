using UnityEngine;
using UnityEngine.InputSystem;

public class GameManagement : MonoBehaviour
{

    private bool EscapeButton;
    private Gamepad gamepad;
    public void OnExitGame(InputAction.CallbackContext context)
    {
        Application.Quit();
        gamepad = Gamepad.current;


        if (gamepad != null)
            gamepad.SetMotorSpeeds(0, 0);

    }



}
