using UnityEngine;

using UnityEngine.InputSystem;

public class PlayerMovement : MonoBehaviour
{
    //https://www.youtube.com/watch?v=BxIIg639KpM
    public float moveSpeed = 5f;
    public float lookSensitivity = 2f;
    public Vector2 LookVector;
    public Vector2 MoveVector;




    public CharacterController characterController;



    void Start()
    {
        characterController = GetComponent<CharacterController>();
    }
    void Update()
    {
        MovePlayer();
        RotatePlayer();

    }
    public void OnMove(InputValue value) //this is how we read the input
    {
        MoveVector = value.Get<Vector2>();
        print("MoveVector: " + MoveVector);

    }

    public void OnLook(InputValue value) //this is how we read the input
    {
        LookVector = value.Get<Vector2>();
        print("LookVector: " + LookVector);

    }

    private void RotatePlayer()
    {
        transform.Translate(new Vector3(LookVector.x, LookVector.y, 0) * lookSensitivity * Time.deltaTime);
    }

    private void MovePlayer()
    {
        Vector3 move = transform.right * MoveVector.x + transform.forward * MoveVector.y;
        characterController.Move(move * moveSpeed * Time.deltaTime);
    }





















}
