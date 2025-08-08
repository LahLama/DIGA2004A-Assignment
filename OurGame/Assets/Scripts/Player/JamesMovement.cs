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



    [Header("Other Componets")]


    private CharacterController controller;
    public TextMeshProUGUI debugText;
    public GameObject cameraTransform;

    private LookFunction lookFunction;

    private void Awake()
    {
        controller = GetComponent<CharacterController>();
        lookFunction = GetComponent<LookFunction>();

    }
    private void Update()
    {

        HandleMovement();

        HandleMovementModifiers();


    }
    public void OnMove(InputAction.CallbackContext context)
    {
        _moveInput = context.ReadValue<Vector2>();

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
        Vector3 move = transform.right * _moveInput.x + transform.forward * _moveInput.y;
        controller.Move(move * moveSpeed * Time.deltaTime);
        if (controller.isGrounded && _velocity.y < 0)
        {
            _velocity.y = -2f;
        }
        _velocity.y += gravity * Time.deltaTime;
        controller.Move(_velocity * Time.deltaTime);
        if (_moveInput.magnitude > 0.1f) // Only bob when moving
        {
            lookFunction.BobCamera();
        }
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
        cameraTransform.GetComponent<Camera>().fieldOfView = Mathf.Lerp(__CurrentFOV, __normalFOV, Time.deltaTime / 0.2f);

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
