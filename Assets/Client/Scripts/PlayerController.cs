using ScriptableObjectArchitecture;
using System.Collections;
using System.Collections.Generic;
using System.Timers;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    public Transform shootPos;

    private float nextTimeToFire;

    public float fireRate = 15f;

    public IntVariable selectedSpellId;

    private Camera mainCamera;
    private Plane groundPlane;

    private void Start()
    {
        mainCamera = Camera.main;
        groundPlane = new Plane(Vector3.up, transform.position);
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Mouse0) && Time.time >= nextTimeToFire)
        {
            nextTimeToFire = Time.time + 1f / fireRate;

            if (selectedSpellId.Value > -1)
            {
                Vector3 mousePos = GetCursorWorldPosition();
                ClientSend.ShootProjectile(mousePos, selectedSpellId.Value);
            }
        }

        if (Input.GetKeyDown(KeyCode.Space)) ClientSend.PlayerJump();
    }


    private void FixedUpdate()
    {
        SendInputToServer();
    }

    Vector3 GetCursorWorldPosition()
    {
        Ray ray = mainCamera.ScreenPointToRay(Input.mousePosition);
        Vector3 point = Vector3.zero;
        if (groundPlane.Raycast(ray, out float distance))
        {
            point = ray.GetPoint(distance);
        }
        return new Vector3(point.x, transform.position.y, point.z);
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