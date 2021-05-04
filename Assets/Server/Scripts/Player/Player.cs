using System.Collections;
using UnityEngine;
using System.Collections.Generic;

public class Player : MonoBehaviour
{
    public int id;
    public string username;
    private Rigidbody _controller;
    private AbilityManager _abilityManager;

    public AbilityManager AbilityManager
    {
        get => _abilityManager;
    }

    private StateHelper _stateHelper;

    public StateHelper StateHelper
    {
        get { return _stateHelper; }
    }

    private HealthManager _healthManager;

    public HealthManager HealthManager
    {
        get { return _healthManager; }
    }

    public Rigidbody Controller
    {
        get { return _controller; }
        set { _controller = value; }
    }
    //TODO: Need to fix this
    public PlayerStats playerStats;

    private float[] inputs;

    [Header("Only for testing purposes")]
    public bool isOffline;

    [HideInInspector] public float h, v;

    private void Awake()
    {
        _controller = GetComponent<Rigidbody>();
        _abilityManager = GetComponent<AbilityManager>();
    }

    private void Start()
    {
        if (isOffline)
        {
            Initialize(10, "Hello");
        //    StartCoroutine(Test());
        }
    }

    private IEnumerator Test()
    {
        yield return new WaitForSeconds(2);
        TransitionToState(new SlideState(-transform.forward, 50, 5));
        yield return new WaitForSeconds(1f);
        TransitionToState(new StunState(4f));
        yield return new WaitForSeconds(1);
        TransitionToState(new SlideState(-transform.forward, 70, 3));
    }

    public void Initialize(int id, string username)
    {
        _healthManager = GetComponent<HealthManager>();
        _stateHelper = new StateHelper();

        this.id = id;
        this.username = username;
        playerStats.health = playerStats.maxHealht;
        inputs = new float[2];
    }

    // Using this when transitioning from state to state
    public void TransitionToState(PlayerBaseState newState)
    {
        if(_healthManager.Health <= 0)
            return;
        
        newState.EnterState(this);
    }

    public void FixedUpdate()
    {
        if (_healthManager.Health <= 0)
            return;

        h = isOffline ? Input.GetAxisRaw("Horizontal") : inputs[0];
        v = isOffline ? Input.GetAxisRaw("Vertical") : inputs[1];

        _stateHelper.CheckForOtherStates(this);

        ServerSend.PlayerPosition(this);
        ServerSend.PlayerRotation(this);
    }

    public void Move(Vector2 inputDirection)
    {
        Vector3 moveDirection = MoveDirection(inputDirection);
        moveDirection *= playerStats.moveSpeed;

        // So we can move with the same speed when going diagonally
        moveDirection = GetCorrectedDirection(moveDirection);

        _controller.velocity = ControllerVelocity(moveDirection);
    }

    private Vector3 ControllerVelocity(Vector3 moveDirection)
    {
        return new Vector3(moveDirection.x, _controller.velocity.y, moveDirection.z);
    }

    private Vector3 GetCorrectedDirection(Vector3 moveDirection)
    {
        if (moveDirection.magnitude > playerStats.moveSpeed)
        {
            float ratio = playerStats.moveSpeed / moveDirection.magnitude;
            moveDirection.x *= ratio;
            moveDirection.z *= ratio;
        }

        return moveDirection;
    }

    private Vector3 MoveDirection(Vector2 inputDirection)
    {
        return new Vector3(inputDirection.x, 0f, inputDirection.y);
    }

    public void SetInputs(float[] inputs, Quaternion rotation)
    {
        this.inputs = inputs;
        transform.rotation = rotation;
    }

    public void ShootProjectile(Vector3 shootDirection, int type)
    {
        // if(_stateHelper.HasState<StunState>())
        //     return;
        //
        // _abilityManager.Shoot(type);
        // NetworkManager.instance.InstanciateProjectile(shootOrigin, type).Init(shootDirection, id);
    }

    public void Jump()
    {
        ServerSend.Jump(this);
    }
}