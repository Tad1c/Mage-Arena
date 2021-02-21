public class HealthController
{
    private IHealth _health;

    public HealthController(IHealth health, float maxHealth)
    {
        _health = health;
        RefillHealth(maxHealth);
    }

    public void TakeDmg(float dmg)
    {
        if(_health.Health <= 0)
            return;
        
        _health.Health -= dmg;

        if (_health.Health <= 0)
        {
            _health.Health = 0;
            _health.IsDead();
        }
    }

    public void RefillHealth(float health) => _health.Health = health;
    
    public void AddHealth(float health) => _health.Health += health;
}