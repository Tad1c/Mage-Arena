﻿using ScriptableObjectArchitecture;
using System.Collections;
using System.Collections.Generic;
using System.Timers;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    public Transform shootPos;

    private float nextTimeToFire;

    public float fireRate = 15f;

    public IntVariable selectedAbility;

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Mouse0) && Time.time >= nextTimeToFire)
        {
            nextTimeToFire = Time.time + 1f / fireRate;
            ClientSend.ShootProjectile(shootPos.forward, selectedAbility);
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