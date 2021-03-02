using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MidpointCalculator : MonoBehaviour
{

    [HideInInspector]
    public Transform player;

    public Transform midpoint;
    public float maxDistance = 8f;

    private Plane groundPlane;

    private void Start()
    {
        groundPlane = new Plane(Vector3.up, Vector3.zero);
    }
    void FixedUpdate()
    {
        if (player == null || midpoint == null) return;

        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        float rayDistance;
        if (groundPlane.Raycast(ray, out rayDistance))
        {
            Vector3 point = ray.GetPoint(rayDistance);

            Vector3 cursorPosition = new Vector3(point.x, player.transform.position.y, point.z);
            // giving some offset towards the player
            Vector3 midpointPosition = (player.transform.position + cursorPosition * 0.8f) / 2;
            midpoint.position = midpointPosition;

            // Limit the distance between the player and the midpoint
            float distance = Vector3.Distance(player.transform.position, midpointPosition);
            if (distance > maxDistance) {
                Vector3 vect = player.transform.position - midpointPosition;
                vect = vect.normalized;
                vect *= (distance - maxDistance);
                midpoint.position += vect;
            }
        }

    }
}
