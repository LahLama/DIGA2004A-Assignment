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
    public Vector2 _moveInput;



    [Header("Crouch Checking")]
    bool _isUnderSomething = false;
    RaycastHit underSomething;
    float sightFraction = 2;

    bool _sightDecreased = false;

    [Header("Sprint Controls")]
    public float _sprintTimer = 5f;
    // current available stamina (seconds)
    private float _sprintStamina;
    public bool _canSprint = true;
    private float _sprintDuration = 5f;
    public Scrollbar sprintBar;
    [SerializeField] bool isMoving;

    [Header("Dialogue State")]
    public bool isDialogueActive = false;

    [Header("Audio Settings")]
    public Slider sfxVolumeSlider;

    [Header("Other Componets")]
    private CharacterController controller;
    public TextMeshProUGUI debugText;
    private GameObject _cameraTransform;
    private GameObject _OverlaycameraTransform;
    private LookFunction lookFunction;
    private NunAi enemyAI;
    private int hideLayer;


    private Interactor _interactor;
    #endregion

    #region UnityFunctions
    private void Awake()
    {
        controller = GetComponent<CharacterController>();
        lookFunction = GetComponent<LookFunction>();
        _cameraTransform = GameObject.FindWithTag("MainCamera");
        _OverlaycameraTransform = GameObject.FindWithTag("OverlayCamera");
        enemyAI = GameObject.FindAnyObjectByType<NunAi>();
        _sprintTimer = 5f;
        hideLayer = LayerMask.NameToLayer("hidePlacesMask");
        CanvasGroup canvasGroup = sprintBar.GetComponent<CanvasGroup>();
        canvasGroup.alpha = 0f;

        _interactor = GameObject.FindAnyObjectByType<Interactor>();

    }
    private void Update()
    {
        HandleMovement();
        HandleMovementModifiers();
        HandleUnderAObject();
        HandleFootstepAudio();


    }
    #endregion

    #region NewInputSystem
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

    #endregion


    public void HandleMovement()
    {
        Vector3 move = transform.right * _moveInput.x + transform.forward * _moveInput.y;
        controller.Move(move * moveSpeed * Time.deltaTime);

        //Handle gravity
        if (controller.isGrounded && _velocity.y < 0)
        {
            _velocity.y = -2f;
        }
        else if (_velocity.y > 10)
        {
            _velocity.y = 0f;
        }
        _velocity.y += gravity * Time.deltaTime;
        controller.Move(_velocity * Time.deltaTime);

        // Only bob when moving
        if (_moveInput.magnitude > 0.1f)
        {
            lookFunction.BobCamera();
        }
    }

    public void HandleSprint()
    {

        float __sprintSpeed = 2.5f;
        int __sprintFOV = 80;



        if (_sprintInput && _canSprint)
        {
            moveSpeed = __sprintSpeed; // Sprint speed

            float __CurrentFOV = _cameraTransform.GetComponent<Camera>().fieldOfView;
            _cameraTransform.GetComponent<Camera>().fieldOfView = Mathf.Lerp(__CurrentFOV, __sprintFOV, Time.deltaTime / 0.3f);
            _OverlaycameraTransform.GetComponent<Camera>().fieldOfView = Mathf.Lerp(__CurrentFOV, __sprintFOV, Time.deltaTime / 0.3f);
        }
        else
        {
            HandleWalk();
        }



    }
    public void HandleCrouch()
    {


        //Adjust the player's components
        float __crouchSpeed = 1f;
        float __scaleModifer = 0.4f;
        if (_crouchInput)
        {
            moveSpeed = __crouchSpeed; // Sprint speed
            this.transform.localScale = new Vector3(__scaleModifer, __scaleModifer, __scaleModifer); // Adjust player scale for crouching

        }


        //Decrease the sight when crounching
        if (!_sightDecreased)
        {
            enemyAI.sightRange /= sightFraction;
            _sightDecreased = true;
        }
    }

    private void HandleWalk()
    {



        //Adjust the player's components to the originals
        float __normalScale = 0.7f;
        float __normalSpeed = 1.5f;
        int __normalFOV = 60;

        moveSpeed = __normalSpeed; // Reset to normal speed
        this.transform.localScale = new Vector3(__normalScale, __normalScale, __normalScale); // Adjust player scale for crouching


        //Get the current FOV and smoothly move Camera to new FOV
        float __CurrentFOV = _cameraTransform.GetComponent<Camera>().fieldOfView;
        _cameraTransform.GetComponent<Camera>().fieldOfView = Mathf.Lerp(__CurrentFOV, __normalFOV, Time.deltaTime / 0.1f);
        _OverlaycameraTransform.GetComponent<Camera>().fieldOfView = Mathf.Lerp(__CurrentFOV, __normalFOV, Time.deltaTime / 0.1f);

        //Increase the sight when crounching
        if (_sightDecreased)
        {
            enemyAI.sightRange *= sightFraction;
            _sightDecreased = false;
        }
    }


    private void HandleUnderAObject()
    {
        //Cast a ray above the player that detects if the player can stand or not.
        Debug.DrawRay(transform.position, transform.TransformDirection(Vector3.up) * underSomething.distance, Color.yellow);
        _isUnderSomething = Physics.Raycast(transform.position, transform.up, out underSomething, 1f);


    }

    private void HandleMovementModifiers()
    {
        HandleSprintBarAppearing();

        bool isMoving = _moveInput.magnitude > 0.1f;
        bool actuallySprinting = _sprintInput && isMoving && _sprintTimer > 0f;

        // Update timer: consume when actually sprinting, otherwise regenerate
        if (actuallySprinting)
        {
            _sprintTimer = Mathf.Max(0f, _sprintTimer - Time.deltaTime);
            _canSprint = true;
        }
        else
        {
            _sprintTimer = Mathf.Min(_sprintDuration, _sprintTimer + Time.deltaTime);
            if (_sprintTimer > 0f) _canSprint = true;
        }

        // Update UI (normalized)
        if (sprintBar != null)
            sprintBar.size = 1f - (_sprintTimer / _sprintDuration);

        // If depleted, force walk and stop sprinting
        if (_sprintTimer <= 0f)
        {
            _sprintTimer = 0f;
            _canSprint = false;
            _sprintInput = false;
            HandleWalk();
            if (SoundManager.Instance != null) SoundManager.Instance.StopLooping("SprintStep");
            return;
        }

        // Movement state handling
        if (actuallySprinting && !_crouchInput && !_isUnderSomething)
        {
            HandleSprint();
            if (SoundManager.Instance != null)
            {
                SoundManager.Instance.PlayLooping("SprintStep");
                SoundManager.Instance.StopLooping("WalkStep");
            }
        }
        else if (_crouchInput && !_sprintInput)
        {
            HandleCrouch();
        }
        else if (!_isUnderSomething)
        {
            HandleWalk();
            if (SoundManager.Instance != null)
            {
                SoundManager.Instance.PlayLooping("WalkStep");
                SoundManager.Instance.StopLooping("SprintStep");
            }
        }
        else if (_isUnderSomething)
        {
            HandleCrouch();
        }
    }

    void HandleSprintBarAppearing()
    {
        if (_sprintInput)
        {
            CanvasGroup canvasGroup = sprintBar.GetComponent<CanvasGroup>();
            if (canvasGroup == null)
            {
                canvasGroup = sprintBar.gameObject.AddComponent<CanvasGroup>();
            }
            canvasGroup.alpha = Mathf.MoveTowards(canvasGroup.alpha, 1f, Time.deltaTime / 0.2f);
        }

        else if (sprintBar.size < 0.05)
        {
            CanvasGroup canvasGroup = sprintBar.GetComponent<CanvasGroup>();
            if (canvasGroup == null)
            {
                canvasGroup = sprintBar.gameObject.AddComponent<CanvasGroup>();
            }
            canvasGroup.alpha = Mathf.MoveTowards(canvasGroup.alpha, 0f, Time.deltaTime / 0.7f);
        }
    }

    public void HandleFootstepAudio()
    {
        float volume = sfxVolumeSlider != null ? sfxVolumeSlider.value : 1f;

        // if player is hidden (via interactor flag) or player layer is hide layer, immediately stop footsteps
        if ((_interactor != null && _interactor._PlayerIsHidden) || gameObject.layer == hideLayer)
        {
            SoundManager.Instance.StopLooping("SprintStep");
            SoundManager.Instance.StopLooping("WalkStep");
            return;
        }

        if (_moveInput.magnitude > 0.1f && controller.isGrounded)
        {
            if (_sprintInput && _canSprint)
            {
                SoundManager.Instance.SetLoopingVolume("SprintStep", volume);
                SoundManager.Instance.PlayLooping("SprintStep");

            }
            else
            {
                SoundManager.Instance.SetLoopingVolume("WalkStep", volume);
                SoundManager.Instance.PlayLooping("WalkStep");

            }
        }
        else
        {
            SoundManager.Instance.StopLooping("SprintStep");
            SoundManager.Instance.StopLooping("WalkStep");

        }
    }
}
