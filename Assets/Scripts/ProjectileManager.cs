using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ProjectileManager : MonoBehaviour
{
    public int id;
    public int playerId;

    public void Init(int id, int playerId)
    {
        this.id = id;
        this.playerId = playerId;
    }

    public void DestoryProjectile(Vector3 pos)
    {
        transform.position = pos;
        Destroy(gameObject);
    }

}
