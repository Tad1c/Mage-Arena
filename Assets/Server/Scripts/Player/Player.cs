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
    public Transform shootOrigin;

    public PlayerStats playerStats;

    public List<MovementState> movementStates = new List<MovementState>();

    private float[] inputs;
    private Vector3 projectileDirection;
    private float projectilePushTime;
    private float projectilePushForce;

    [Header("Only for testing purposes")]
    public bool isOffline;

    private void Awake()
    {
        controller = GetComponent<Rigidbody>();
    }

    private void Start()
    {
        if (isOffline)
            Initialize(1, "Hello");
    }

    public void Initialize(int id, string username)
    {
        this.id = id;
        this.username = username;
        playerStats.health = playerStats.maxHealht;
        inputs = new float[2];
    }

    public void FixedUpdate()
    {
        if (playerStats.health <= 0)
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
        Move(new Vector3(inputs[0], inputs[1]));

        //TODO: add stun, slow, or push the player
        if (projectilePushForce >= 1)
        {
            // calculate the force to be applied to the rb
            controller.AddForce(projectileDirection * projectilePushForce, ForceMode.VelocityChange);
            projectilePushForce = Mathf.Lerp(projectilePushForce, 0f, projectilePushTime * Time.deltaTime);
        }


        ServerSend.PlayerPosition(this);
        ServerSend.PlayerRotation(this);
    }

    private void Move(Vector2 inputDirection)
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



    public void HitByProjectile(Vector3 direction, float pushTime, float pushForce)
    {
        projectileDirection = direction;
        projectilePushTime = pushTime;
        projectilePushForce = pushForce;
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

}

