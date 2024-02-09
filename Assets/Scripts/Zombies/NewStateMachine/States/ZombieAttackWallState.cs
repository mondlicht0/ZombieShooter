using System;
using UnityEngine;
using UnityHFSM;

public class ZombieAttackWallState : ZombieStateBase
{
    public ZombieAttackWallState(bool needsExitTime, Zombie zombie) : base(needsExitTime, zombie)
    {
    }
}
