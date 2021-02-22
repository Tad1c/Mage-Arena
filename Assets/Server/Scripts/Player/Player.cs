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

    public Vector3 projectileDirection;
    public float projectilePushTime;
    public float projectilePushForce;

    [Header("Only for testing purposes")]
    public bool isOffline;
    public bool isHit;

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

        stateHelper = new StateHelper();
    }

    public void Initialize(int id, string username)
    {
        healthManager = GetComponent<HealthManager>();
      
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

        stateHelper.GetTopState().Update(this);

        ServerSend.PlayerPosition(this);
        ServerSend.PlayerRotation(this);
    }

    private void OfflineMode()
    {
        float h = Input.GetAxisRaw("Horizontal");
        float v = Input.GetAxisRaw("Vertical");

        Move(new Vector2(h, v));
    }

    private void NetworkMovement()
    {
        Move(new Vector2(inputs[0], inputs[1]));

        //TODO: add stun, slow, or push the player.. ADD this to push state
        if (projectilePushForce >= 1)
        {
            // calculate the force to be applied to the rb
            _controller.AddForce(projectileDirection * projectilePushForce, ForceMode.VelocityChange);
            projectilePushForce = Mathf.Lerp(projectilePushForce, 0f, projectilePushTime * Time.deltaTime);
        }

        ServerSend.PlayerPosition(this);
        ServerSend.PlayerRotation(this);
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
        isHit = true;
    }

    public void OnPush(Vector3 direction, float time, float force)
    {

        //    movementStates.Add(MovementState.Pushed);
        //   StartCoroutine(ClearMovementState(MovementState.Pushed, time));
        projectileDirection = direction;
        projectilePushTime = time;
        projectilePushForce = force;
        isHit = true;
    }

}

