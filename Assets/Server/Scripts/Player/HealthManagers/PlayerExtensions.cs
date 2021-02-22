using UnityEngine;

public static class PlayerExtensions
{
    public static bool IsDead(this Player player) => player.HealthManager.Health == 0;

}