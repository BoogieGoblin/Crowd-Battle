using UnityEngine;

[RequireComponent(typeof(Rigidbody))]
public class PlayerMotor : MonoBehaviour
{
    [SerializeField] private Rigidbody rb;
    [SerializeField] private float speed = 10f;
    [SerializeField] private float rotationSpeed = 720f; // Increase if you want faster rotation

    private Vector2 currentInput;

    void Start()
    {
        if (rb == null) rb = GetComponent<Rigidbody>();
    }

    public void ProcessMove(Vector2 input)
    {
        currentInput = input;
    }

    void FixedUpdate()
    {
        // 1. Kill physics torque so the player doesn't slowly spin on its own
        rb.angularVelocity = Vector3.zero;

        Vector3 moveDirection = new Vector3(currentInput.x, 0f, currentInput.y);

        // 2. Only rotate and move when there is movement input
        if (moveDirection.sqrMagnitude > 0.01f)
        {
            moveDirection.Normalize();

            // Smooth rotation towards input
            Quaternion targetRotation = Quaternion.LookRotation(moveDirection);
            rb.MoveRotation(Quaternion.RotateTowards(rb.rotation, targetRotation, rotationSpeed * Time.fixedDeltaTime));

            // Move player horizontally while keeping vertical gravity
            rb.linearVelocity = new Vector3(moveDirection.x * speed, rb.linearVelocity.y, moveDirection.z * speed);
        }
        else
        {
            // Stop horizontal movement instantly when idle
            rb.linearVelocity = new Vector3(0f, rb.linearVelocity.y, 0f);
        }
    }
}