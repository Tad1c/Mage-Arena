﻿using System.Collections;
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
    private float yVelocity = 0;

    private bool isGrounded;

    [SerializeField]
    private float distanceCheck;

    private bool hasBeenHit = false;
    private Vector3 projectileDirection;
    private float projectilePushTime;

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

    private void Update()
    {
        RaycastHit hit;

        Debug.DrawRay(this.transform.position, Vector3.down, Color.red, distanceCheck);
        if (Physics.Raycast(this.transform.position, Vector3.down, out hit, distanceCheck))
        {
            if (hit.collider.CompareTag("Untagged"))
                isGrounded = true;
        }
        else
            isGrounded = false;
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
    
        if (isGrounded)
        {
            if (inputs[4])
                controller.AddForce(new Vector3(0f, jumpSpeed, 0f));
        }

        controller.velocity = new Vector3(moveDirection.x, controller.velocity.y, moveDirection.z);

        if (hasBeenHit)
        {
            // this -3f will actually be the speed when moving opposite of the applied force
            controller.AddForce(projectileDirection * (moveSpeed - (moveSpeed / 2.5f)), ForceMode.VelocityChange);
        }

        ServerSend.PlayerPosition(this);
        ServerSend.PlayerRotation(this);
    }

    public void SetInputs(bool[] inputs, Quaternion rotation)
    {
        this.inputs = inputs;
        transform.rotation = rotation;
    }

    public void Shoot(Vector3 viewDirection)
    {
        if (Physics.Raycast(shootOrigin.position, viewDirection, out RaycastHit hit, 25f))
        {
            if (hit.collider.CompareTag("Player"))
            {
                hit.collider.GetComponent<Player>().TakeDamage(5);
                //hit.collider.GetComponent<Player>().MoveBackwardsOnHit(-hit.transform.forward, 200);
            }
        }
    }

    public void ShootProjectile(Vector3 shootDirection)
    {
        NetworkManager.instance.InstanciateProjectile(shootOrigin).Init(shootDirection, id);
    }

    public void HitByProjectile(Vector3 direction, float pushTime)
    {
        projectileDirection = direction;
        projectilePushTime = pushTime;
        StartCoroutine(stunCountdown(direction));
    }

    private IEnumerator stunCountdown(Vector3 dir)
    {
        hasBeenHit = true;
        yield return new WaitForSeconds(projectilePushTime);
        hasBeenHit = false;
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
