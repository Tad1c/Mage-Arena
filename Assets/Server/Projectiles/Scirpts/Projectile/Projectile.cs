using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Projectile : MonoBehaviour
{
    protected static Dictionary<int, Projectile> projectileDic = new Dictionary<int, Projectile>();
    protected static int nextProjectileId = 1;
   [HideInInspector] public int type;
    
   [HideInInspector] public int damage;
   [HideInInspector] public float cooldown;
   [HideInInspector] public float speed = 10.0f;
   [HideInInspector] public float range = 30.0f;
    
    [HideInInspector] public int byPlayerId;
   [HideInInspector] public int id;

    public abstract void Init(Vector3 dir, int playerId);
}
