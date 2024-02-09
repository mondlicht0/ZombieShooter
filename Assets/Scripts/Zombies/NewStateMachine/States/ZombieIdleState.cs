using System;
using UnityEngine;
using UnityHFSM;

public class ZombieIdleState : ZombieStateBase
{
    public ZombieIdleState(bool needsExitTime, Zombie zombie) : base(needsExitTime, zombie)
    {
    }

    public override void OnEnter()
    {
        Debug.Log("Enter to Idle");
        base.OnEnter();
        Agent.isStopped = true;
        Animator.SetBool("Idle", true);
    }

    public override void OnExit()
    {
        Debug.Log("Exit from Idle");
        base.OnExit();
        Animator.SetBool("Idle", false);
    }
}
