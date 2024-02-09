using System;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.InputSystem.LowLevel;
using UnityHFSM;

public class ZombieStateBase : State<ZombieState, StateEvent>
{
    protected readonly Zombie Zombie;   
    protected readonly NavMeshAgent Agent;   
    protected readonly Animator Animator;   
    protected readonly Health Health;   
    protected bool RequestedExit;   
    protected float ExitTime;

    protected readonly Action<State<ZombieState, StateEvent>> onEnter;
    protected readonly Action<State<ZombieState, StateEvent>> onLogic;
    protected readonly Action<State<ZombieState, StateEvent>> onExit;
    protected readonly Func<State<ZombieState, StateEvent>, bool> CanExit;

    public ZombieStateBase(bool needsExitTime, Zombie zombie, float exitTime = 0.1f,
                           Action<State<ZombieState, StateEvent>> onEnter = null, 
                           Action<State<ZombieState, StateEvent>> onLogic = null,
                           Action<State<ZombieState, StateEvent>> onExit = null, 
                           Func<State<ZombieState, StateEvent>, bool> canExit = null)
    {
        Zombie = zombie;
        Animator = Zombie.GetComponent<Animator>();
        Agent = Zombie.GetComponent<NavMeshAgent>();
        Health = Zombie.GetComponent<Health>();
        this.onEnter = onEnter;
        this.onLogic = onLogic;
        this.onExit = onExit;
        CanExit = canExit;
        ExitTime = exitTime;
        this.needsExitTime = needsExitTime;

    }

    public override void OnEnter()
    {
        base.OnEnter();
        RequestedExit = false;
        onEnter?.Invoke(this);
    }

    public override void OnLogic()
    {
        base.OnLogic();
        if (RequestedExit && timer.Elapsed >=ExitTime)
        {
            fsm.StateCanExit();
        }
    }

    public override void OnExitRequest()
    {
        if (!needsExitTime || CanExit != null && CanExit(this))
        {
            fsm.StateCanExit();
        }

        else
        {
            RequestedExit = true;
        }
    }
}
