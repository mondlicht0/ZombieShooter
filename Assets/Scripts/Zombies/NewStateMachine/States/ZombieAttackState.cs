using System;
using UnityEngine;
using UnityHFSM;

public class ZombieAttackState : ZombieStateBase
{
    public ZombieAttackState(bool needsExitTime, Zombie zombie, Action<State<ZombieState, StateEvent>> onEnter, float exitTime = 1.5f) : base(needsExitTime, zombie, exitTime, onEnter)
    {
    }

    public override void OnEnter()
    {
        Debug.Log("Enter to Attack");
        Agent.isStopped = true;
        base.OnEnter();
        Animator.SetBool("Attack", true);
    }

    public override void OnExit()
    {
        Debug.Log("Exit from Attack");
        base.OnExit();
        Animator.SetBool("Attack", false);
    }
}
