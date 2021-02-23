public interface IHealth
{
    float Health { get; set; }
    void Died();
    void Respawn();
}