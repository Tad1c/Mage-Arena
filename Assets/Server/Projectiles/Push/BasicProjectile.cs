using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public enum ProjectileType
{
    Stun,
    Push
}

public class BasicProjectile : MonoBehaviour
{
    public static Dictionary<int, BasicProjectile> projectileDic = new Dictionary<int, BasicProjectile>();
    public static int nextProjectileId = 1;

    public ProjectileType projectileType;

    public int type;

    public float speed = 10.0f;
    public float range = 30.0f;

    public float stunDuration = 1f;

    private float pushTime = 3f;
    private float pushForce = 50f;


    private Rigidbody rb;
    public int byPlayerId;
    public int id;
    public int damage;

    private Vector3 finalDestination;

    public float posUpdateRate = 0.2f;

    public void Init(Vector3 direction, int playerId)
    {
        byPlayerId = playerId;
        finalDestination = transform.TransformPoint(direction * range);
    }

    // Start is called before the first frame update
    void Start()
    {
        rb = GetComponent<Rigidbody>();

        id = nextProjectileId;
        nextProjectileId++;

        projectileDic.Add(id, this);

        ServerSend.InstantiateBasicProjectile(this, byPlayerId, finalDestination, type);
        transform.LookAt(finalDestination);
        // ServerSend.ProjectilePosition(id, finalDestination);
        //InvokeRepeating("UpdatePosition", posUpdateRate, posUpdateRate);  //1s delay, repeat every 1s
    }

    private void Update()
    {
        float step = speed * Time.deltaTime; // calculate distance to move
        transform.position = Vector3.MoveTowards(transform.position, finalDestination, step);

        if (Vector3.Distance(transform.position, finalDestination) < 0.01f)
        {
            Destroy(gameObject);
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            Player player = other.GetComponent<Player>();

            if (player.id == id) return;

            Vector3 pushDirection =
                transform.position - player.transform.position; //player.transform.position - transform.position;
            pushDirection = -pushDirection.normalized;

            player.HealthManager.TakeDamage(damage);

            switch (projectileType)
            {
                case ProjectileType.Push:
                    player.TransitionToState(new SlideState(pushDirection, pushForce, pushTime));
                    break;
                case ProjectileType.Stun:
                    player.TransitionToState(new StunState(stunDuration));
                    break;
            }


            Destroy(gameObject);
        }
    }


    private void OnDestroy()
    {
        CancelInvoke();
        projectileDic.Remove(id);
        ServerSend.DestroyBasicProjectile(this);
    }
}