using System;
using UnityEngine;
using UnityHFSM;

public class ZombieDeadState : ZombieStateBase
{
    public ZombieDeadState(bool needsExitTime, Zombie zombie) : base(needsExitTime, zombie)
    {
    }

    public override void OnEnter()
    {
        base.OnEnter();
        Animator.SetBool("IsDead", true);
        Health.Die(Vector3.forward);
        Agent.height = 0;
        Agent.velocity = Vector3.zero;
    }

    public override void OnExit()
    {
        base.OnExit();
    }
}
