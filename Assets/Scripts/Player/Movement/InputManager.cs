using UnityEngine;
using UnityEngine.InputSystem;

public class InputManager : MonoBehaviour
{
    PlayerInput playerInput;
    PlayerInput.OnFootActions onFoot;

    [SerializeField] PlayerMotor motor;
    [SerializeField] SpawnRadiusVisual spawnRadiusVisual;
    [SerializeField] PrimaryAttack primaryAttack;

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
        onFoot.Deploy.performed += ctx => spawnRadiusVisual.TryActivate(10f);
        onFoot.Attack.performed += ctx => primaryAttack.Attack();
    }
    void OnDisable()
    {
        onFoot.Disable();
        onFoot.Deploy.performed -= ctx => spawnRadiusVisual.TryActivate(10f);
        onFoot.Attack.performed -= ctx => primaryAttack.Attack();
    }

    void CoreMovements()
    {
        motor.ProcessMove(onFoot.Movement.ReadValue<Vector2>());
    }

}