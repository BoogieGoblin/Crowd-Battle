using UnityEngine;
using UnityEngine.InputSystem;

public class InputManager : MonoBehaviour
{
    private PlayerInput playerInput;
    private PlayerInput.OnFootActions onFoot;

    [SerializeField] private PlayerMotor motor;
    [SerializeField] private AllySpawner allySpawner;
    [SerializeField] private PrimaryAttack primaryAttack;

    void Awake()
    {
        playerInput = new PlayerInput();
        onFoot = playerInput.OnFoot;
        if (motor == null) motor = GetComponent<PlayerMotor>();
        if (allySpawner == null) allySpawner = GetComponent<AllySpawner>();
    }

    void FixedUpdate()
    {
        CoreMovements();
    }

    void OnEnable()
    {
        onFoot.Enable();
        onFoot.Deploy.performed += OnDeployPerformed;
        onFoot.Attack.performed += OnAttackPerformed;
    }

    void OnDisable()
    {
        onFoot.Disable();
        onFoot.Deploy.performed -= OnDeployPerformed;
        onFoot.Attack.performed -= OnAttackPerformed;
    }

    private void OnDeployPerformed(InputAction.CallbackContext ctx)
    {
        if (allySpawner != null)
        {
            allySpawner.TriggerAllySpawn();
        }
    }

    private void OnAttackPerformed(InputAction.CallbackContext ctx)
    {
        if (primaryAttack != null)
        {
            primaryAttack.Attack();
        }
    }

    void CoreMovements()
    {
        if (motor != null)
        {
            motor.ProcessMove(onFoot.Movement.ReadValue<Vector2>());
        }
    }
}