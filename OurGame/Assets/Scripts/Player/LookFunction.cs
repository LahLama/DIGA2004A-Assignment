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
    private Vector2 _lookInput;
    private float _verticalRotation = 0f;
    private float startingYPos = 0;
    public GameObject HandHeldItem;
    private CharacterController controller;
    public GameObject cameraTransform;

    private PlayerMovement playerMovement;

    #endregion

    private void Awake()
    {
        controller = GetComponent<CharacterController>();
        playerMovement = GetComponent<PlayerMovement>();
        Cursor.lockState = CursorLockMode.Locked;
        //Cursor.visible = false;
        startingYPos = HandHeldItem.transform.localPosition.y;
    }
    private void Update()
    {
        HandleLook();
    }
    public void OnLook(InputAction.CallbackContext context)
    {
        _lookInput = context.ReadValue<Vector2>();
    }

    public void BobCamera()
    { //Bobbing

        float __bobbingAmount = Mathf.Sin(Time.time * (bobbingFrequency + playerMovement.moveSpeed / 2)) * bobbingAmplitude;

        //cameraTransform.transform.localRotation = Quaternion.Euler(cameraTransform.transform.localRotation.eulerAngles.x, cameraTransform.transform.localRotation.eulerAngles.y, __bobbingAmount);
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
}
