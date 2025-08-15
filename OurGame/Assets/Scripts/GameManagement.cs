using UnityEngine;
using UnityEngine.InputSystem;

public class GameManagement : MonoBehaviour
{

    private bool EscapeButton;
    public void OnExitGame(InputAction.CallbackContext context) { Application.Quit(); }



}
