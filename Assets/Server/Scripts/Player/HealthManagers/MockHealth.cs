using UnityEngine;

public class MockHealth : IHealth
 {
     public float Health { get; set; }
     public void Died()
     {
         Debug.Log("Player is dead");
     }

     public void Respawn()
     {
         Debug.Log("Player Respawned");
     }
 }