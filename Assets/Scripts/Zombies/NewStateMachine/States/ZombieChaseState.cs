using System;
using UnityEngine;
using UnityHFSM;

public class ZombieChaseState : ZombieStateBase
{
	private Transform _target;

	public ZombieChaseState(bool needsExitTime, Zombie zombie, Transform target) : base(needsExitTime, zombie)
	{
		_target = target;
	}

	public override void OnEnter()
	{
		Debug.Log("Enter to Chase");
		base.OnEnter();
		Agent.enabled = true;
		Agent.isStopped = false;
		Animator.SetBool("Chase", true);
	}

	public override void OnLogic()
	{
		base.OnLogic();
		if (!RequestedExit)
		{
			Agent.SetDestination(_target.position);
		}

		else if (Agent != null)
		{
			if (Agent.remainingDistance <=Agent.stoppingDistance)
				fsm.StateCanExit();
		}
	}

	public override void OnExit()
	{
		Debug.Log("Exit from Chase");
		base.OnExit();
		Animator.SetBool("Chase", false);
	}
}
