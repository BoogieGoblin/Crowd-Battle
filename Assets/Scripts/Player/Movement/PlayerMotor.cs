using UnityEngine;

public class PlayerMotor : MonoBehaviour
{
    CharacterController controller;
    Vector3 playerVelocity;
    bool isGrounded;
    [SerializeField] float gravity = -9.8f;
    [SerializeField] float speed = 10f;  
    [SerializeField] float rotationSpeed = 720f; // Degrees per second

    void Start()
    {
        controller = GetComponent<CharacterController>();
    }
    void Update()
    {
        isGrounded = controller.isGrounded;
    }

//Take input from InputManager and apply them on CharacterController.
    public void ProcessMove(Vector2 input)
    { 
        Mover(input);
        ApplyGravity();
    }

private void RotateTowardsMovement(Vector3 direction)
    {
        // Create a rotation that looks in the movement direction
        Quaternion targetRotation = Quaternion.LookRotation(direction);

        // Smoothly rotate the character
        transform.rotation = Quaternion.RotateTowards(
        transform.rotation, 
        targetRotation, 
        rotationSpeed * Time.deltaTime
        );
    }

    void ApplyGravity()
    {
        // 3. Gravity logic remains the same
        playerVelocity.y += gravity * Time.deltaTime;
        if(isGrounded && playerVelocity.y < 0)
        {
            playerVelocity.y = -2f;
        }
        controller.Move(playerVelocity * Time.deltaTime);
    }

    void Mover(Vector2 input)
    {
        // 1. Calculate direction in World Space (ignoring current rotation)
        Vector3 moveDirection = new Vector3(input.x, 0, input.y);

        // 2. Only rotate and move if there is input
        if (moveDirection.sqrMagnitude > 0.01f)
        {
            // Rotate towards the movement direction
            RotateTowardsMovement(moveDirection);
        
            // Move in the world space direction
            controller.Move(moveDirection.normalized * speed * Time.deltaTime);
        }
    }
}