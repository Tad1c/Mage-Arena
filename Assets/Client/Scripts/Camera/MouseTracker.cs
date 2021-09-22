using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class MouseTracker : MonoBehaviour
{

    private Plane groundPlane;
    private Camera mainCam;

    void Start()
    {
        groundPlane = new Plane(Vector3.up, Vector3.zero);
        mainCam = Camera.main;
    }

    void FixedUpdate()
    {
        Ray ray = mainCam.ScreenPointToRay(Mouse.current.position.ReadValue());
        if (groundPlane.Raycast(ray, out float rayDistance))
        {
            Vector3 point = ray.GetPoint(rayDistance);
            transform.position = point;
        }
    }

}
