using System.Collections;
using UnityEngine;
using System.Collections.Generic;

public class Player : MonoBehaviour
{
    public int id;
    public string username;
    private Rigidbody _controller;

    private StateHelper stateHelper;
    public StateHelper StateHelper { get { return stateHelper; } }


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

    private StunState _stun;

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
        _stun = GetComponent<StunState>();
    }

    private void Start()
    {
        if (isOffline)
        {
            Initialize(10, "Hello");
            StartCoroutine(Test());
        }
    }

    private IEnumerator Test()
    {
        yield return  new WaitForSeconds(2);
        TransitionToState(new SlideState(-transform.forward, 50, 3));
        yield return  new WaitForSeconds(0.1f);
        TransitionToState(_stun.Init(4f, this));
        // yield return  new WaitForSeconds(2f);
        // TransitionToState(new SlideState(-transform.forward, 100, 3));
    }

    public void Initialize(int id, string username)
    {
        healthManager = GetComponent<HealthManager>();
        stateHelper = new StateHelper();

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

        stateHelper.GetTopState().StateUpdate(this);
        
        
        

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

