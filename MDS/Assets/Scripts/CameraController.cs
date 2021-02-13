using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraController : MonoBehaviour
{

    public PlayerManager player;
    public float sensitivity = 100f;
    public float clampAngle = 85f;

    private float verticalRot;
    private float horizontalRot;

    private void Start()
    {
        verticalRot = transform.localEulerAngles.x;
        horizontalRot = transform.localEulerAngles.y;
    }

    private void Update()
    {

        if (Input.GetKeyDown(KeyCode.Escape))
            ToggleCoursorMode();


        if (Cursor.lockState == CursorLockMode.Locked)
            Look();

        Debug.DrawRay(transform.position, transform.forward * 2f, Color.red);
    }

    private void Look()
    {
        float vertical = -Input.GetAxis("Mouse Y");
        float horizontal = Input.GetAxis("Mouse X");

        verticalRot += vertical * sensitivity * Time.deltaTime;
        horizontalRot += horizontal * sensitivity * Time.deltaTime;

        verticalRot = Mathf.Clamp(verticalRot, -clampAngle, clampAngle);

        transform.localRotation = Quaternion.Euler(verticalRot, 0f, 0f);
        player.transform.rotation = Quaternion.Euler(0f, horizontalRot, 0f);
    }


    private void ToggleCoursorMode()
    {
        Cursor.visible = !Cursor.visible;

        if (Cursor.lockState == CursorLockMode.None)
            Cursor.lockState = CursorLockMode.Locked;
        else
            Cursor.lockState = CursorLockMode.None;
    }

}
