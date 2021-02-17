using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ProjectileManager : MonoBehaviour
{
    public int id;
    public int playerId;

    public float speed = 10.0f;

    private Vector3 finalPosition;

    public void Init(int id, int playerId, Vector3 finalPosition)
    {
        this.id = id;
        this.playerId = playerId;
        this.finalPosition = finalPosition;
        transform.LookAt(finalPosition);
    }

    private void Update()
    {
        float step = speed * Time.deltaTime; // calculate distance to move
        transform.position = Vector3.MoveTowards(transform.position, finalPosition, step);
    }

    public void DestoryProjectile(Vector3 pos)
    {
        transform.position = pos;
        Destroy(gameObject);
    }

}
