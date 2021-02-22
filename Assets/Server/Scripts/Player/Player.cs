using System.Collections;
using UnityEngine;
using System.Collections.Generic;

public class Player : MonoBehaviour
{
    public int id;
    public string username;
    private Rigidbody _controller;


    // StateManager can be modified just by this player
    private StateManager stateManager;
    // Only the getter is public
    public StateManager StateManager { get { return stateManager; } }


    private HealthManager healthManager;
    public HealthManager HealthManager { get { return healthManager; } }


    public Rigidbody Controller
    {
        get
        {
            return _controller;
        }
        set
        {
            _controller = value;
        }
    }

    public Transform shootOrigin;

    public PlayerStats playerStats;

    // public List<MovementState> movementStates = new List<MovementState>();

    private float[] inputs;

    [Header("Only for testing purposes")]
    public bool isOffline;

    [HideInInspector]
    public float h, v;

    private Projectile _projectile;

    public Projectile Projectile
    {
        get
        {
            return _projectile;
        }
        set
        {
            _projectile = value;
        }
    }

    private void Awake()
    {
        _controller = GetComponent<Rigidbody>();
    }

    private void Start()
    {
        if (isOffline)
        {
            Initialize(10, "Hello");
        }
    }

    public void Initialize(int id, string username)
    {
        healthManager = GetComponent<HealthManager>();
        stateManager = GetComponent<StateManager>();

        this.id = id;
        this.username = username;
        playerStats.health = playerStats.maxHealht;
        inputs = new float[2];
    }

    // Using this when transitioning from state to state
    public void TransitionToState(PlayerBaseState newState)
    {
        newState.EnterState(this);
    }

    private void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            var pro = new Projectile();
            GetProjectileInfo(pro);
        }
    }

    public void FixedUpdate()
    {
        if (playerStats.health <= 0)
            return;

        if (isOffline)
        {
            h = Input.GetAxisRaw("Horizontal");
            v = Input.GetAxisRaw("Vertical");
        }
        else
        {
            h = inputs[0];
            v = inputs[1];
        }

        stateManager.GetTopState().Update(this);

        ServerSend.PlayerPosition(this);
        ServerSend.PlayerRotation(this);
    }

    private void OfflineMode()
    {
        float h = Input.GetAxisRaw("Horizontal");
        float v = Input.GetAxisRaw("Vertical");

        Move(new Vector2(h, v));
    }

    public void Move(Vector2 inputDirection)
    {
        Vector3 moveDirection = new Vector3(inputDirection.x, 0f, inputDirection.y);//transform.right * inputDirection.x + transform.forward * inputDirection.y;
        moveDirection *= playerStats.moveSpeed;

        // So we can move with the same speed when going diagonally
        if (moveDirection.magnitude > playerStats.moveSpeed)
        {
            float ratio = playerStats.moveSpeed / moveDirection.magnitude;
            moveDirection.x *= ratio;
            moveDirection.z *= ratio;
        }

        _controller.velocity = new Vector3(moveDirection.x, _controller.velocity.y, moveDirection.z);
    }



    public void SetInputs(float[] inputs, Quaternion rotation)
    {
        this.inputs = inputs;
        transform.rotation = rotation;
    }

    public void ShootProjectile(Vector3 shootDirection)
    {
        NetworkManager.instance.InstanciateProjectile(shootOrigin).Init(shootDirection, id);
    }

    public void Jump()
    {
        ServerSend.Jump(this);
    }

    private IEnumerator Respawn()
    {
        yield return new WaitForSeconds(5f);

        playerStats.health = playerStats.maxHealht;
        _controller.useGravity = true;
        ServerSend.PlayerRespawn(this);
    }

    public void GetProjectileInfo(Projectile projectile)
    {
        this._projectile = projectile;
    }

}

