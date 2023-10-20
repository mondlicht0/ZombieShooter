using UnityEngine;

public class ZombieTargetState : ZombieState
{
    public override ZombieState Tick(ZombieManager zombieManager)
    {
        return this;
    }
}
