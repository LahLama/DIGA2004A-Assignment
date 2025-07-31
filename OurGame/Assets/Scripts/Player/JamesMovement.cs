using System.Xml.Serialization;
using Unity.VisualScripting.FullSerializer;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.InputSystem;
using UnityEngine.Rendering;

public class PlayerMovement : MonoBehaviour
{
    [Header("Movement Settings")]
    public float moveSpeed = 5f;
    public float gravity = -9.81f;

    [Header("Look Settings")]
    public Transform cameraTransform;
    public float lookSensitivity = 2f;
    public float verticalLookLimit = 90f;
    public float bobbingSensitivity = 4; // Sensitivity for camera bobbing

    private CharacterController controller;
    private Vector2 moveInput;
    private Vector2 lookInput;

    private bool sprintInput;
    private Vector3 velocity;
    private float verticalRotation = 0f;

    private void Awake()
    {
        controller = GetComponent<CharacterController>();
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
    }

    private void Update()
    {
        HandleMovement();
        HandleLook();
        HandleSprint();
    }
    public void OnMove(InputAction.CallbackContext context)
    {
        moveInput = context.ReadValue<Vector2>();
    }

    public void OnLook(InputAction.CallbackContext context)
    {
        lookInput = context.ReadValue<Vector2>();
    }

    public void OnSprint(InputAction.CallbackContext context)
    {
        sprintInput = context.ReadValueAsButton();
    }



    public void HandleMovement()
    {
        Vector3 move = transform.right * moveInput.x + transform.forward * moveInput.y;
        controller.Move(move * moveSpeed * Time.deltaTime);

        if (controller.isGrounded && velocity.y < 0)
            velocity.y = -2f;

        velocity.y += gravity * Time.deltaTime;
        controller.Move(velocity * Time.deltaTime);

        BobCamera();


    }

    private void BobCamera()
    { //Bobbing
        float bobbingAmount = Mathf.Sin(Time.time * 10) * bobbingSensitivity; // Adjust the multiplier for more or less bobbing
        cameraTransform.transform.localEulerAngles = new Vector3(cameraTransform.transform.rotation.x + bobbingAmount, cameraTransform.transform.rotation.y, cameraTransform.transform.rotation.z);
    }
    public void HandleLook()
    {
        float mouseX = lookInput.x * lookSensitivity;
        float mouseY = lookInput.y * lookSensitivity;

        verticalRotation -= mouseY;
        verticalRotation = Mathf.Clamp(verticalRotation, -verticalLookLimit, verticalLookLimit);

        cameraTransform.localRotation = Quaternion.Euler(verticalRotation, 0f, 0f);
        transform.Rotate(Vector3.up * mouseX);
    }

    public void HandleSprint()
    {
        float sprintSpeed = 10f;
        float normalSpeed = 5f;


        if (sprintInput)
        {
            moveSpeed = sprintSpeed; // Sprint speed
        }
        else
        {
            moveSpeed = normalSpeed; // Reset to normal speed
        }
    }
}
