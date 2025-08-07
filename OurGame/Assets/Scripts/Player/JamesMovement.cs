using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.InputSystem;


public class PlayerMovement : MonoBehaviour
{
    [Header("Movement Settings")]
    public float moveSpeed = 5f;
    public float gravity = -9.81f;
    private bool _sprintInput;
    private bool _crouchInput;
    private Vector3 _velocity;
    private Vector2 _moveInput;

    [Header("Look Settings")]
    public GameObject cameraTransform;
    public float lookSensitivity = 2f;
    public float verticalLookLimit = 90f;
    public float bobbingAmplitude = 25f; // Sensitivity for camera bobbing
    public float bobbingFrequency = 1f; // Frequency of bobbing
    private Vector2 _lookInput;
    private float _verticalRotation = 0f;
    private float startingYPos = 0;

    [Header("Other Componets")]


    private CharacterController controller;
    public TextMeshProUGUI debugText;

    public GameObject HandHeldItem;

    private void Awake()
    {
        controller = GetComponent<CharacterController>();
        Cursor.lockState = CursorLockMode.Locked;
        //Cursor.visible = false;
        startingYPos = HandHeldItem.transform.localPosition.y;
    }

    private void Update()
    {

        HandleMovement();
        HandleLook();
        HandleMovementModifiers();


    }
    public void OnMove(InputAction.CallbackContext context)
    {
        _moveInput = context.ReadValue<Vector2>();

    }

    public void OnLook(InputAction.CallbackContext context)
    {
        _lookInput = context.ReadValue<Vector2>();
    }

    public void OnSprint(InputAction.CallbackContext context)
    {
        _sprintInput = context.ReadValueAsButton();
    }
    public void OnCrouch(InputAction.CallbackContext context)
    {
        _crouchInput = context.ReadValueAsButton();
    }


    public void HandleMovement()
    {
        Vector3 __move = transform.right * _moveInput.x + transform.forward * _moveInput.y;
        controller.Move(__move * moveSpeed * Time.deltaTime);
        if (controller.isGrounded && _velocity.y < 0)
        {
            _velocity.y = -2f;
        }
        _velocity.y += gravity * Time.deltaTime;
        controller.Move(_velocity * Time.deltaTime);
        if (_moveInput.magnitude > 0.1f) // Only bob when moving
        {
            BobCamera();
        }
    }

    private void BobCamera()
    { //Bobbing

        float __bobbingAmount = Mathf.Sin(Time.time * (bobbingFrequency + moveSpeed / 2)) * bobbingAmplitude;

        cameraTransform.transform.localRotation = Quaternion.Euler(cameraTransform.transform.localRotation.eulerAngles.x, cameraTransform.transform.localRotation.eulerAngles.y, __bobbingAmount);
        HandHeldItem.transform.localPosition = new Vector3(HandHeldItem.transform.localPosition.x, startingYPos + __bobbingAmount, HandHeldItem.transform.localPosition.z); // Adjust hand position based on bobbing


    }
    public void HandleLook()
    {
        float mouseX = _lookInput.x * lookSensitivity;
        float mouseY = _lookInput.y * lookSensitivity;

        _verticalRotation -= mouseY;
        _verticalRotation = Mathf.Clamp(_verticalRotation, -verticalLookLimit, verticalLookLimit);

        cameraTransform.transform.localRotation = Quaternion.Euler(_verticalRotation, 0f, cameraTransform.transform.localRotation.eulerAngles.z);
        transform.Rotate(Vector3.up * mouseX);
    }

    public void HandleSprint()
    {
        float __sprintSpeed = 10f;
        int __sprintFOV = 70;


        if (_sprintInput)
        {
            moveSpeed = __sprintSpeed; // Sprint speed
            debugText.text = "Sprinting"; // Update debug text
            float __CurrentFOV = cameraTransform.GetComponent<Camera>().fieldOfView;
            cameraTransform.GetComponent<Camera>().fieldOfView = Mathf.Lerp(__CurrentFOV, __sprintFOV, Time.deltaTime / 0.3f);
        }


    }
    public void HandleCrouch()
    {
        float __crouchSpeed = 1f;
        float __scaleModifer = 0.4f;
        if (_crouchInput)
        {
            moveSpeed = __crouchSpeed; // Sprint speed
            this.transform.localScale = new Vector3(__scaleModifer, __scaleModifer, __scaleModifer); // Adjust player scale for crouching
            debugText.text = "Crouching"; // Update debug text

            //Raycast above, if its hitting something, stay in crouch, bool when crouching
        }

    }

    private void HandleWalk()
    {
        float __normalScale = 0.7f;
        float __normalSpeed = 5f;
        int __normalFOV = 60;


        moveSpeed = __normalSpeed; // Reset to normal speed
        this.transform.localScale = new Vector3(__normalScale, __normalScale, __normalScale); // Adjust player scale for crouching
        debugText.text = "Walking"; // Update debug text

        float __CurrentFOV = cameraTransform.GetComponent<Camera>().fieldOfView;
        cameraTransform.GetComponent<Camera>().fieldOfView = Mathf.Lerp(__CurrentFOV, __normalFOV, Time.deltaTime / 0.5f);

    }

    private void HandleMovementModifiers()
    {
        if (_sprintInput && !_crouchInput)
        {
            HandleSprint();
        }
        else if (_crouchInput && !_sprintInput)
        {
            HandleCrouch();
        }
        else
            HandleWalk();
    }




}
