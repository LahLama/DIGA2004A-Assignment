using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.InputSystem;
using UnityEngine.Rendering;

public class PlayerMovement : MonoBehaviour
{
    //https://www.youtube.com/watch?v=BxIIg639KpM
    public float moveSpeed = 5f;
    public float lookSensitivity = 50f;
    public Vector2 LookVector;
    public Vector3 rotation;
    public Vector2 _moveDirection;


    public CharacterController characterController;

    void Start()
    {
        characterController = GetComponent<CharacterController>();
        rotation = transform.localEulerAngles;
        Cursor.lockState = CursorLockMode.Locked; // Lock the cursor to the center of the screen

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

    public void OnLook(InputAction.CallbackContext context) //this is how we read the input
    {
        LookVector = context.ReadValue<Vector2>();
        print("LookVector: " + LookVector);
    }

    private void RotatePlayer()
    {
        rotation.y += LookVector.x * lookSensitivity * Time.deltaTime;
        transform.localEulerAngles = rotation;
    }






















}
