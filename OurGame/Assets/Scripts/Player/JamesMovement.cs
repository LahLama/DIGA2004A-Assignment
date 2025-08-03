using System;
using System.Xml.Serialization;
using TMPro;
using Unity.VisualScripting;
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
    public GameObject cameraTransform;
    public float lookSensitivity = 2f;
    public float verticalLookLimit = 90f;
    public float bobbingAmplitude = 25f; // Sensitivity for camera bobbing
    public float bobbingFrequency = 1f; // Frequency of bobbing

    private CharacterController controller;
    private Vector2 moveInput;
    private Vector2 lookInput;

    private bool sprintInput;
    private bool crouchInput;
    public TextMeshProUGUI debugText;


    private Vector3 velocity;
    private float verticalRotation = 0f;

    public GameObject Hand; // Reference to the hand object for bobbing

    private void Awake()
    {
        controller = GetComponent<CharacterController>();
        Cursor.lockState = CursorLockMode.Locked;
        //Cursor.visible = false;
    }

    private void Update()
    {

        HandleMovement();
        HandleLook();

        HandleMovementModifiers();


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
    public void OnCrouch(InputAction.CallbackContext context)
    {
        crouchInput = context.ReadValueAsButton();
    }


    public void HandleMovement()
    {
        Vector3 move = transform.right * moveInput.x + transform.forward * moveInput.y;
        controller.Move(move * moveSpeed * Time.deltaTime);



        if (controller.isGrounded && velocity.y < 0)
        {
            velocity.y = -2f;
        }
        velocity.y += gravity * Time.deltaTime;


        controller.Move(velocity * Time.deltaTime);



        if (moveInput.magnitude > 0.1f) // Only bob when moving
        {
            BobCamera();
        }




    }

    private void BobCamera()
    { //Bobbing

        float bobbingAmount = Mathf.Sin(Time.time * (bobbingFrequency + moveSpeed / 2)) * bobbingAmplitude;

        // print(bobbingAmount);
        /* Vector3 camPos = cameraTransform.transform.localPosition;
         camPos.y = bobbingAmount;
         cameraTransform.transform.localPosition = camPos;*/



        cameraTransform.transform.localRotation = Quaternion.Euler(cameraTransform.transform.localRotation.eulerAngles.x, cameraTransform.transform.localRotation.eulerAngles.y, (bobbingAmount / bobbingAmplitude) * 0.1f);
        Hand.transform.position = new Vector3(Hand.transform.position.x, bobbingAmount, Hand.transform.position.z); // Adjust hand position based on bobbing


    }
    public void HandleLook()
    {
        float mouseX = lookInput.x * lookSensitivity;
        float mouseY = lookInput.y * lookSensitivity;

        verticalRotation -= mouseY;
        verticalRotation = Mathf.Clamp(verticalRotation, -verticalLookLimit, verticalLookLimit);

        cameraTransform.transform.localRotation = Quaternion.Euler(verticalRotation, 0f, cameraTransform.transform.localRotation.eulerAngles.z);
        transform.Rotate(Vector3.up * mouseX);
    }

    public void HandleSprint()
    {
        float sprintSpeed = 10f;


        if (sprintInput)
        {
            moveSpeed = sprintSpeed; // Sprint speed
            debugText.text = "Sprinting"; // Update debug text
        }
        else
        {

        }
    }
    public void HandleCrouch()
    {
        float crouchSpeed = 1f;
        float scaleModifer = 0.4f;



        if (crouchInput)
        {
            moveSpeed = crouchSpeed; // Sprint speed
            this.transform.localScale = new Vector3(scaleModifer, scaleModifer, scaleModifer); // Adjust player scale for crouching
            debugText.text = "Crouching"; // Update debug text
        }
        else
        {


        }
    }

    private void HandleWalk()
    {
        float normalScale = 0.7f;
        float normalSpeed = 5f;


        moveSpeed = normalSpeed; // Reset to normal speed
        this.transform.localScale = new Vector3(normalScale, normalScale, normalScale); // Adjust player scale for crouching
        debugText.text = "Walking"; // Update debug text

    }

    private void HandleMovementModifiers()
    {
        if (sprintInput && !crouchInput)
        {
            HandleSprint();
        }
        else if (crouchInput && !sprintInput)
        {
            HandleCrouch();
        }
        else
            HandleWalk();
    }




}
