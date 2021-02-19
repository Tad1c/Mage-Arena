using System.Collections;
using UnityEngine;
using System.Collections.Generic;

[System.Serializable]
public class PlayerStats
{
    public float moveSpeed = 8f;
    public float jumpSpeed = 5f;
    public float health;
    public float maxHealht = 100;
}

public class Player : MonoBehaviour
{
    public int id;
    public string username;
    private Rigidbody controller;

    public Rigidbody Controller
    {
        get
        {
            return controller;
        }
        set
        {
            controller = value;
        }
    }

    public Transform shootOrigin;

    public PlayerStats playerStats;

    private PlayerBaseState currentState;

    public readonly MoveState moveState = new MoveState();
    public readonly AttackState attackState = new AttackState();

    // public List<MovementState> movementStates = new List<MovementState>();

    private float[] inputs;
    private Vector3 projectileDirection;
    private float projectilePushTime;
    private float projectilePushForce;

    [Header("Only for testing purposes")]
    public bool isOffline;
    public bool isHit;

    private Projectile projectile;

    public Projectile Projectile
    {
        get
        {
            return projectile;
        }
        set
        {
            projectile = value;
        }
    }

    private void Awake()
    {
        controller = GetComponent<Rigidbody>();
    }

    private void Start()
    {
        if (isOffline)
            Initialize(1, "Hello");

        TransitionToState(moveState);
    }

    public void Initialize(int id, string username)
    {
        this.id = id;
        this.username = username;
        playerStats.health = playerStats.maxHealht;
        inputs = new float[2];
    }

    // Using this when transitioning from state to state
    public void TransitionToState(PlayerBaseState state)
    {
        //applying the new state to be current state
        currentState = state;
        //enter in the state
        currentState.EnterState(this);
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



        currentState.Update(this);

        // if (isOffline)
        // {
        //     OfflineMode();
        // }
        // else
        // {
        //     NetworkMovement();
        // }

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
            controller.AddForce(projectileDirection * projectilePushForce, ForceMode.VelocityChange);
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

        controller.velocity = new Vector3(moveDirection.x, controller.velocity.y, moveDirection.z);
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


    public void TakeDamage(float dmg)
    {
        if (playerStats.health <= 0)
            return;

        playerStats.health -= dmg;
        if (playerStats.health <= 0f)
        {
            playerStats.health = 0f;
            controller.useGravity = false;
            transform.position = new Vector3(0f, 25f, 0f);
            ServerSend.PlayerPosition(this);
            StartCoroutine(Respawn());
        }

        ServerSend.PlayerHealth(this);
    }

    private IEnumerator Respawn()
    {
        yield return new WaitForSeconds(5f);

        playerStats.health = playerStats.maxHealht;
        controller.useGravity = true;
        ServerSend.PlayerRespawn(this);
    }

    public void GetProjectileInfo(Projectile projectile)
    {
        this.projectile = projectile;
        isHit = true;
    }

    public void OnPush(Vector3 direction, float time, float force)
    {
        //    movementStates.Add(MovementState.Pushed);
        //   StartCoroutine(ClearMovementState(MovementState.Pushed, time));
        projectileDirection = direction;
        projectilePushTime = time;
        projectilePushForce = force;
    }

}

