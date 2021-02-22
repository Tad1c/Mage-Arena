
using UnityEngine;

public abstract class PlayerBaseState
{
    public virtual void EnterState(Player player)
    {
        // Default implementation
        // This can be overriden in the certain state if there's need to
        // Doesn't have to be implemented in the subclass if the default implementation is enough
        MyLog.D("Entering " + this.GetType().Name);
        player.StateManager.AddState(this);
    }

    public abstract void Update(Player player);

}
