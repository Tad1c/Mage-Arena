using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{

    public Transform shootPos;

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Mouse0))
        {
            ClientSend.ShootProjectile(shootPos.forward);
        }

        if (Input.GetKeyDown(KeyCode.Space)) ClientSend.PlayerJump();
    }

    private void FixedUpdate()
    {
        SendInputToServer();
    }

    private void SendInputToServer()
    {
        float[] inputs = new float[]
        {
           Input.GetAxisRaw("Horizontal"),
           Input.GetAxisRaw("Vertical"),
        };

        ClientSend.PlayerMovement(inputs);
    }

}
