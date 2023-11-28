using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum NPCStateId
{
    Idle,
    Patrol,
    ChasePlayer,
    Attack,
    Death,
    AttackWall

}
public interface NPCState
{
    NPCStateId GetId();
    void Enter(NPCAgent agent);
    void Update(NPCAgent agent);
    void Exit(NPCAgent agent);
}
