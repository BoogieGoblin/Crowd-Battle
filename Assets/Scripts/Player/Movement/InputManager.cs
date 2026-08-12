using UnityEngine;
using UnityEngine.InputSystem;

public class InputManager : MonoBehaviour
{
    PlayerInput playerInput;
    PlayerInput.OnFootActions onFoot;

    [SerializeField] PlayerMotor motor;
    [SerializeField] Player player;
    [SerializeField] SpawnRadiusVisual radiusVisual;
    [SerializeField] float attackRadius = 5f;

    void Awake()
    {
        playerInput = new PlayerInput();
        onFoot = playerInput.OnFoot;
        motor = GetComponent<PlayerMotor>();
    }

    void FixedUpdate()
    {
       CoreMovements();
    }

    void OnEnable()
    {
        onFoot.Enable();
        onFoot.Deploy.performed += ctx => radiusVisual.TryActivate(transform.position, attackRadius);
        onFoot.Attack.performed += ctx => DealDamage(30);
    }
    void OnDisable()
    {
        onFoot.Disable();
        onFoot.Deploy.performed -= ctx => radiusVisual.TryActivate(transform.position, attackRadius);
    }

    void CoreMovements()
    {
        motor.ProcessMove(onFoot.Movement.ReadValue<Vector2>());
    }

    void DealDamage(int damage)
    {
        player.TakeDamage(damage);
    }

}