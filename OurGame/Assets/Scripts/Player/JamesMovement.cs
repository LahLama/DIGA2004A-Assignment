using System.Xml.Serialization;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.InputSystem;
using UnityEngine.Rendering;

public class PlayerMovement : MonoBehaviour
{
    //https://www.youtube.com/watch?v=BxIIg639KpM

    [Header("Movement Settings")]
    public float moveSpeed = 5f;
    private Vector2 _moveDirection;


    [Header("Looking Settings")]
    public float lookSensitivity = 50f;
    private Vector2 LookVector;
    private Vector3 rotation;
    public float bobbingSensitivity = 0.1f; // Adjust this value to control the bobbing effect

    [Header("Character Controller")]
    public CharacterController characterController;
    public Camera FPCamera; // Reference to the camera for looking around

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


        BobCamera();
    }


    private void MovePlayer()
    {
        Vector3 move = transform.right * _moveDirection.x + transform.forward * _moveDirection.y;
        characterController.Move(move * moveSpeed * Time.deltaTime);

        //-------------------TEMPORARY-------------------
        transform.position = new Vector3(transform.position.x, 1, transform.position.z); // Ensure the player stays on the ground plane

        BobCamera();
    }

    public void OnLook(InputAction.CallbackContext context) //this is how we read the input
    {
        LookVector = context.ReadValue<Vector2>();
    }

    private void RotatePlayer()
    {
        rotation.y += LookVector.x * lookSensitivity * Time.deltaTime;
        rotation.x -= LookVector.y * lookSensitivity * Time.deltaTime;
        rotation.x = Mathf.Clamp(rotation.x, -90f, 90f); // Clamp the vertical rotation to prevent flipping
        transform.localEulerAngles = rotation;
    }

    private void BobCamera()
    { //Bobbing
        float bobbingAmount = Mathf.Sin(Time.time * 10) * bobbingSensitivity; // Adjust the multiplier for more or less bobbing
        FPCamera.transform.localEulerAngles = new Vector3(FPCamera.transform.rotation.x + bobbingAmount, FPCamera.transform.rotation.y, FPCamera.transform.rotation.z);
    }




















}
