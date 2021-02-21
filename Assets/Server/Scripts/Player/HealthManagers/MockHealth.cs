using UnityEngine;

public class MockHealth : IHealth
 {
     public float Health { get; set; }
     public void IsDead()
     {
         Debug.Log("Player is dead");
     }
 }