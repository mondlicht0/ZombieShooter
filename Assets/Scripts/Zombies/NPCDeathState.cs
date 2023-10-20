using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class NPCDeathState : NPCState
{

    public NPCStateId GetId()
    {
        return NPCStateId.Death;
    }
    void NPCState.Enter(NPCAgent agent)
    {
        agent.isDead = true;
        agent.ragdoll.ActivateRagdol();
        agent.StartCoroutine(agent.ragdoll.FadeOut());
        /*if (agent.navMeshAgent != null)
        {
            agent.navMeshAgent.isStopped = true;
        }*/
        //agent.animator.SetTrigger("death");
        agent.aiHealth.isDead = true;
    }

    void NPCState.Exit(NPCAgent agent)
    {
        agent.isDead = false;
    }



    void NPCState.Update(NPCAgent agent)
    {

    }


}

