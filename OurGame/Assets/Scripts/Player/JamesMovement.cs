using UnityEngine;

using UnityEngine.InputSystem;

public class PlayerMovement : MonoBehaviour
{
    //https://www.youtube.com/watch?v=BxIIg639KpM
    public float moveSpeed = 5f;
    public float lookSensitivity = 2f;
    public Vector2 LookVector;
    public Vector3 _lookDirection;
    public Vector2 _moveDirection;


    public CharacterController characterController;
    private PlayerControls playerControls;
    private InputAction lookDirectionAction;

    void Start()
    {
        characterController = GetComponent<CharacterController>();
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

    public void OnLook(InputAction.CallbackContext context) //this is how we read the input
    {
        LookVector = context.ReadValue<Vector2>();

    }

    private void RotatePlayer()
    {
        _lookDirection.y += LookVector.x * lookSensitivity * Time.deltaTime;
        transform.localEulerAngles = _lookDirection;
    }

    private void MovePlayer()
    {
        Vector3 move = transform.right * _moveDirection.x + transform.forward * _moveDirection.y;
        characterController.Move(move * moveSpeed * Time.deltaTime);
    }





















}
