using System;
using UnityEngine;
using UnityEngine.InputSystem;

public class Interactions : MonoBehaviour
{



    /*private PlayerInput _playerInput;    

    private void Awake()
    {
        _playerInput = GetComponent<PlayerInput>();
    }

    private void OnEnable()
    {
        _playerInput.actions.Enable();
    }

    private void OnDisable()
    {
        _playerInput.actions.Disable();
    }*/

    public void OnInteract(InputAction.CallbackContext context)
    {
        if (context.performed)
        {
            // Handle interaction logic here
            Debug.Log("Interact action performed");

        }
    }

}
