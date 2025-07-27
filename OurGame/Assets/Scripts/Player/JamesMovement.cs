using Unity.Mathematics;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.InputSystem;
using UnityEngine.Rendering;

public class PlayerMovement : MonoBehaviour
{
    //https://www.youtube.com/watch?v=BxIIg639KpM

    [Header("Movement Settings")]
    public float moveSpeed = 5f;
    public Vector2 _moveDirection;

    [Header("Player Rotation")]
    public Vector2 LookVector;
    public Vector3 rotation;
    public float lookSensitivity = 50f;

    [Header("Components")]
    public CharacterController characterController;

    void Start()
    {
        characterController = GetComponent<CharacterController>();
        rotation = transform.localEulerAngles;

        Cursor.visible = true; // Hide the cursor
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


    private void MovePlayer()
    {
        Vector3 move = transform.right * _moveDirection.x + transform.forward * _moveDirection.y;
        characterController.Move(move * moveSpeed * Time.deltaTime);
    }

    public void OnLook(InputAction.CallbackContext context) //this is how we read the input
    {
        LookVector = context.ReadValue<Vector2>();
        print("LookVector: " + LookVector);
    }

    private void RotatePlayer()
    {
        /*print("Rotation: " + rotation);
        print("LookVector: " + LookVector.x);
        print("lookSensitivity: " + lookSensitivity);
       */
        rotation.y += LookVector.x * lookSensitivity;
        transform.rotation = Quaternion.Euler(new Vector3(0, rotation.y, 0));
    }






















}
