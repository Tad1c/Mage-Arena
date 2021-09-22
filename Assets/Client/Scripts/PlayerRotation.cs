using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerRotation : MonoBehaviour
{

    private Plane groundPlane;
    private Camera mainCam;

    private void Start()
    {
        groundPlane = new Plane(Vector3.up, Vector3.zero);
        mainCam = Camera.main;
    }

    void Update()
    {
        Ray ray = mainCam.ScreenPointToRay(Mouse.current.position.ReadValue());
        if (groundPlane.Raycast(ray, out float rayDistance))
        {
            Vector3 point = ray.GetPoint(rayDistance);
            Vector3 lookAtPos = new Vector3(point.x, this.transform.position.y, point.z);
            this.transform.LookAt(lookAtPos);
        }
    }

}
