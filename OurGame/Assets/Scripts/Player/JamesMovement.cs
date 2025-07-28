using Unity.Mathematics;
using UnityEditor.Rendering.LookDev;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.InputSystem;
using UnityEngine.Rendering;

public class PlayerMovement : MonoBehaviour
{
    //https://www.youtube.com/watch?v=BxIIg639KpM

    [Header("Movement Settings")]
    public float moveSpeed = 5f;
    public Vector2 _moveDirection;

    [Header("Player Rotation")]
    public Vector2 LookVector;
    public Vector3 rotation;
    public float lookSensitivity = 50f;

    [Header("Components")]
    public CharacterController characterController;
    private InputActionAsset inputActions;
    private InputAction lookAction;

    void Start()
    {
        characterController = GetComponent<CharacterController>();
        rotation = transform.localEulerAngles;
        lookAction = inputActions.FindActionMap("Gameplay").FindAction("Look");

        lookAction.performed += context => LookVector = context.ReadValue<Vector2>();
        lookAction.canceled += context => LookVector = Vector2.zero;

    }
    void Update()
    {
        MovePlayer();
        RotatePlayer();

    }
    public void OnMove(InputAction.CallbackContext context) //this is how we read the input
    {
        _moveDirection = context.ReadValue<Vector2>();

    }


    private void MovePlayer()
    {
        Vector3 move = transform.right * _moveDirection.x + transform.forward * _moveDirection.y;
        characterController.Move(move * moveSpeed * Time.deltaTime);
    }



    private void RotatePlayer()
    {
        float mouseX = LookVector.x * lookSensitivity;
        transform.Rotate(0, mouseX, 0);
    }






















}
