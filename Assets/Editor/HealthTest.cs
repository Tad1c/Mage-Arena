using System.Collections;
using System.Collections.Generic;
using NUnit.Framework;
using UnityEngine;

public class HealthTest
{

    [Test]
    public void AddHealth()
    {
        IHealth playerHealth = new MockHealth() {Health = 100};
        HealthController healthController = new HealthController(playerHealth, 100);
        healthController.AddHealth(100);
        Assert.AreEqual(200, playerHealth.Health);
    }
    
    [Test]
    public void DamgePlayer()
    {
        IHealth playerHealth = new MockHealth() {Health = 100};
        HealthController healthController = new HealthController(playerHealth, 100);
        healthController.TakeDmg(150);
        Assert.AreEqual(0, playerHealth.Health);
    }

    [Test]
    public void RefillHealth()
    {
        IHealth playerHealth = new MockHealth() {Health = 100};
        HealthController healthController = new HealthController(playerHealth, 100);
        healthController.RefillHealth(100);
        Assert.AreEqual(100, playerHealth.Health);
    }
}