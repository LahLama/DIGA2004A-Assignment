using TMPro;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UI;


public class PlayerMovement : MonoBehaviour
{

    #region Varibles
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
    bool _isUnderSomething = false;
    RaycastHit underSomething;
    private float _sprintTimer;
    bool _canSprint = true;
    private float _sprintDuration = 4f;
    public Scrollbar sprintBar;

    #endregion

    private void Awake()
    {
        controller = GetComponent<CharacterController>();
        lookFunction = GetComponent<LookFunction>();
        _sprintTimer = 4f;
    }
    private void Update()
    {

        HandleMovement();

        HandleMovementModifiers();
        HandleUnderAObject();




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
        float __sprintSpeed = 6;
        int __sprintFOV = 65;




        if (_sprintTimer >= _sprintDuration) { _canSprint = true; }
        if (_sprintTimer < 0) { _canSprint = false; }


        if (_sprintInput && _canSprint)
        {
            moveSpeed = __sprintSpeed; // Sprint speed
            debugText.text = "Sprinting"; // Update debug text
            float __CurrentFOV = cameraTransform.GetComponent<Camera>().fieldOfView;
            cameraTransform.GetComponent<Camera>().fieldOfView = Mathf.Lerp(__CurrentFOV, __sprintFOV, Time.deltaTime / 0.3f);

            _sprintTimer -= Time.deltaTime;
            sprintBar.size = 1 - (_sprintTimer / 4);
            //Debug.Log("--" + (int)_sprintTimer);
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
        cameraTransform.GetComponent<Camera>().fieldOfView = Mathf.Lerp(__CurrentFOV, __normalFOV, Time.deltaTime / 0.1f);

    }


    private void HandleUnderAObject()
    {
        Debug.DrawRay(transform.position, transform.TransformDirection(Vector3.up) * underSomething.distance, Color.yellow);
        _isUnderSomething = Physics.Raycast(transform.position, transform.up, out underSomething, 1f);

        if (_isUnderSomething)
        { debugText.text = "Stuck Crouching"; }
    }
    private void HandleMovementModifiers()
    {
        if (_sprintInput && !_crouchInput && !_isUnderSomething)
        {
            HandleSprint();
        }
        else if (_crouchInput && !_sprintInput)
        {
            HandleCrouch();
        }
        else if (!_isUnderSomething)
        {
            HandleWalk();
        }
        else if (_isUnderSomething)
        {
            HandleCrouch();

        }


        if ((_sprintTimer < _sprintDuration && (!_canSprint || !_sprintInput)))
        {
            if (_crouchInput)
            {
                HandleCrouch();
            }
            else
            {
                HandleWalk();
            }
            _sprintTimer += Time.deltaTime;
            // Debug.Log("++" + (int)_sprintTimer);
            sprintBar.size = 1 - (_sprintTimer / 4);
        }
    }




}
