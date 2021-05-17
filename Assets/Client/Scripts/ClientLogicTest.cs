using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ClientLogicTest : MonoBehaviour
{

    public float moveSpeed = 6.5f;

    public GameObject playerPrefab;
    public GameObject canvas;

    private Rigidbody rb;

    private void Start()
    {
        GameObject player = Instantiate(playerPrefab, Vector3.zero, Quaternion.identity);

        rb = player.AddComponent<Rigidbody>();
        rb.constraints = RigidbodyConstraints.FreezeRotation;

        player.GetComponent<CapsuleCollider>().enabled = true;

        PlayerController playerController = player.GetComponent<PlayerController>();
        playerController.enabled = false;

        PlayerClient playerClient = player.GetComponent<PlayerClient>();
        playerClient.enabled = false;

        canvas.SetActive(false);
    }

    private void Update()
    {

        float h = Input.GetAxisRaw("Horizontal");
        float v = Input.GetAxisRaw("Vertical");

        Vector3 moveDirection = MoveDirection(new Vector2(h,v));
        moveDirection *= moveSpeed;

        // So we can move with the same speed when going diagonally
        moveDirection = GetCorrectedDirection(moveDirection);

        rb.velocity = ControllerVelocity(moveDirection);
    }

    private Vector3 ControllerVelocity(Vector3 moveDirection)
    {
        return new Vector3(moveDirection.x, rb.velocity.y, moveDirection.z);
    }

    private Vector3 MoveDirection(Vector2 inputDirection)
    {
        return new Vector3(inputDirection.x, 0f, inputDirection.y);
    }

    private Vector3 GetCorrectedDirection(Vector3 moveDirection)
    {
        if (moveDirection.magnitude > moveSpeed)
        {
            float ratio = moveSpeed / moveDirection.magnitude;
            moveDirection.x *= ratio;
            moveDirection.z *= ratio;
        }

        return moveDirection;
    }


}
