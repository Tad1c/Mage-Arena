using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerRotation : MonoBehaviour
{


    private Plane groundPlane;
    private void Start()
    {
        groundPlane = new Plane(Vector3.up, Vector3.zero);
    }

    void Update()
    {
        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        float rayDistance;
        if (groundPlane.Raycast(ray, out rayDistance))
        {
            Vector3 point = ray.GetPoint(rayDistance);
            Vector3 lookAtPos = new Vector3(point.x, this.transform.position.y, point.z);
            this.transform.LookAt(lookAtPos);
        }
    }

}
