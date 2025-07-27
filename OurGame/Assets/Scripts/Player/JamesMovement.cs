using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.InputSystem;

public class PlayerMovement : MonoBehaviour
{

    public float moveSpeed = 5f;
    public Vector2 _moveDirection;


    public CharacterController characterController;

    void Start()
    {
        characterController = GetComponent<CharacterController>();
    }
    void Update()
    {
        MovePlayer();

    }
    public void OnMove(InputAction.CallbackContext context) //this is how we read the input
    {
        _moveDirection = context.ReadValue<Vector2>();

    }

    private void MovePlayer()
    {
        Vector3 move = Vector3.right * _moveDirection.x + Vector3.forward * _moveDirection.y;
        characterController.Move(move * moveSpeed * Time.deltaTime);
    }





















}
