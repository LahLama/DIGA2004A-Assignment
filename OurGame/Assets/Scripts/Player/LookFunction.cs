using UnityEngine;
using UnityEngine.InputSystem;
public class LookFunction : MonoBehaviour
{

    #region Varibles
    [Header("Look Settings")]

    public float lookSensitivity = 2f;
    public float verticalLookLimit = 90f;
    public float bobbingAmplitude = 25f; // Sensitivity for camera bobbing
    public float bobbingFrequency = 1f; // Frequency of bobbing
    private float _verticalRotation = 0f;
    private float _startingYPos = 0;
    private Vector2 _lookInput;
    private GameObject _handHeldItem;
    private GameObject _cameraTransform;
    private PlayerMovement _playerMovement;

    #endregion

    #region UnityFunctions
    private void Awake()
    {
        _playerMovement = GetComponent<PlayerMovement>();
        _cameraTransform = GameObject.FindWithTag("MainCamera");
        _handHeldItem = GameObject.FindWithTag("HoldingPos");
        Cursor.lockState = CursorLockMode.Locked;                   //Lock the cursor to the screen
        _startingYPos = _handHeldItem.transform.localPosition.y;    //Get the intial postion of the object that will be held
    }
    private void Update()
    {
        HandleLook();
    }
    #endregion


    #region NewInputSystem
    public void OnLook(InputAction.CallbackContext context)

    {
        _lookInput = context.ReadValue<Vector2>();
    }
    #endregion


    public void BobCamera()
    {
        //Bob the player's hands in a sinosoidal wave along the y-axis
        float __bobbingAmount = Mathf.Sin(Time.time * (bobbingFrequency + _playerMovement.moveSpeed / 2)) * bobbingAmplitude;
        _handHeldItem.transform.localPosition = new Vector3(_handHeldItem.transform.localPosition.x, _startingYPos + __bobbingAmount, _handHeldItem.transform.localPosition.z);
    }


    public void HandleLook()
    {
        float mouseX = _lookInput.x * lookSensitivity;
        float mouseY = _lookInput.y * lookSensitivity;
        _verticalRotation -= mouseY;
        _verticalRotation = Mathf.Clamp(_verticalRotation, -verticalLookLimit, verticalLookLimit);
        _cameraTransform.transform.localRotation = Quaternion.Euler(_verticalRotation, 0f, _cameraTransform.transform.localRotation.eulerAngles.z);
        transform.Rotate(Vector3.up * mouseX);
    }
}
