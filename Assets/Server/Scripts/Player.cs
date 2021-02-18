using System.Collections;
using UnityEngine;

public class Player : MonoBehaviour
{
    public int id;
    public string username;

    public Rigidbody controller;
    public Transform shootOrigin;
    public float gravity = -9.81f;
    public float moveSpeed = 8f;
    public float jumpSpeed = 5f;
    public float health;
    public float maxHealht = 100;

    private bool[] inputs;

    [SerializeField]
    private float distanceCheck;

    private Vector3 projectileDirection;
    private float projectilePushTime;
    private float projectilePushForce;

    [Header("Only for testing purposes")]
    public bool isOffline;

    private void Start()
    {
        //  gravity *= Time.fixedDeltaTime * Time.fixedDeltaTime;
        //   moveSpeed *= Time.fixedDeltaTime;
        //  jumpSpeed *= Time.fixedDeltaTime;
        if (isOffline)
            Initialize(1, "Hello");

    }

    public void Initialize(int id, string username)
    {
        this.id = id;
        this.username = username;
        health = maxHealht;
        inputs = new bool[5];
    }

    public void FixedUpdate()
    {
        if (health <= 0)
            return;

        if (isOffline)
        {
            OfflineMode();
        }
        else
        {
            NetworkMovement();
        }

    }

    private void OfflineMode()
    {
        float h = Input.GetAxisRaw("Horizontal");
        float v = Input.GetAxisRaw("Vertical");

        Move(new Vector2(h, v));
    }

    private void NetworkMovement()
    {

        Vector3 inputDirection = Vector3.zero;

        // W
        if (inputs[0])
            inputDirection.y += 1;

        // S
        if (inputs[1])
            inputDirection.y -= 1;

        // A
        if (inputs[2])
            inputDirection.x -= 1;

        // D
        if (inputs[3])
            inputDirection.x += 1;

        Move(inputDirection);

    }

    private void Move(Vector2 inputDirection)
    {

        Vector3 moveDirection = new Vector3(inputDirection.x, 0f, inputDirection.y);//transform.right * inputDirection.x + transform.forward * inputDirection.y;
        moveDirection *= moveSpeed;

        // So we can move with the same speed when going diagonally
        if (moveDirection.magnitude > moveSpeed)
        {
            float ratio = moveSpeed / moveDirection.magnitude;
            moveDirection.x *= ratio;
            moveDirection.z *= ratio;
        }

        controller.velocity = new Vector3(moveDirection.x, controller.velocity.y, moveDirection.z);

        if (projectilePushForce >= 1)
        {
            // calculate the force to be applied to the rb
            controller.AddForce(projectileDirection * projectilePushForce, ForceMode.VelocityChange);
            projectilePushForce = Mathf.Lerp(projectilePushForce, 0f, projectilePushTime * Time.deltaTime);
        }

        ServerSend.PlayerPosition(this);
        ServerSend.PlayerRotation(this);
    }

    public void SetInputs(bool[] inputs, Quaternion rotation)
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

    public void HitByProjectile(Vector3 direction, float pushTime, float pushForce)
    {
        projectileDirection = direction;
        projectilePushTime = pushTime;
        projectilePushForce = pushForce;
    }

    public void TakeDamage(float dmg)
    {
        if (health <= 0)
            return;

        health -= dmg;
        if (health <= 0f)
        {
            health = 0f;
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

        health = maxHealht;
        controller.useGravity = true;
        ServerSend.PlayerRespawn(this);
    }

}

