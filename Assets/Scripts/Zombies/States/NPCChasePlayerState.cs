using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class NPCChasePlayerState : NPCState
{
    private PlayerHealth playerHealth;
    float timer = 0.0f;

    public NPCStateId GetId()
    {
        return NPCStateId.ChasePlayer;
    }

    void NPCState.Enter(NPCAgent agent)
    {
        //Debug.Log("Zombie Chase");
        agent.playerSeen = true;
        agent.isChaseing = true;
        agent.navMeshAgent.stoppingDistance = agent.config.attackRadius;

        playerHealth = agent.playerTransform.GetComponent<PlayerHealth>();
    }

    void NPCState.Exit(NPCAgent agent)
    {
        //Debug.Log("Exit Zombie Chase");
        agent.isChaseing = false;
        agent.navMeshAgent.stoppingDistance = 0.0f;
    }

    void NPCState.Update(NPCAgent agent)
    {
        timer -= Time.deltaTime;
        if (agent.aiHealth.isDead)
        {
            agent.stateMachine.ChangeState(NPCStateId.Death);
        }
        else
        {
            if (!agent.navMeshAgent.hasPath && agent.playerTransform != null)
            {
                //agent.navMeshAgent.speed = agent.config.chaseWalkingSpeed + agent.config.offsetChaseSpeed;
                if (timer <= 0.0f)
                {
                    //Debug.Log("Chase Itme");
                    agent.navMeshAgent.SetDestination(agent.playerTransform.position);
                    timer = agent.config.waitTime;
                }
            }

            else
            {
                if (timer <= 0.0f)
                {
                    ChasePlayer(agent);
                    timer = agent.config.waitTime;
                }
            }
        }

        if (playerHealth.IsDead)
        {
            agent.playerSeen = false;
            agent.stateMachine.ChangeState(NPCStateId.Idle);
        }
    }

    private static void ChasePlayer(NPCAgent agent)
    {
        if (GameObject.FindObjectOfType<Player>() != null)
        {
            Player player = GameObject.FindObjectOfType<Player>();
            float distance = Vector3.Distance(player.transform.position, agent.transform.position);

            Ray ray = new Ray(agent.transform.position + Vector3.up, agent.transform.forward);
            Debug.DrawRay(agent.transform.position, agent.transform.forward, Color.red);

            if (distance >= agent.config.attackRadius + agent.config.offsetAttackRadius)
            {
                agent.animator.SetBool("isAttacking", false);
                agent.navMeshAgent.isStopped = false;
                agent.navMeshAgent.speed = agent.config.chaseWalkingSpeed + agent.config.offsetChaseSpeed;
                agent.navMeshAgent.SetDestination(agent.playerTransform.position);

                if (Physics.Raycast(ray, out agent.hit, 2f))
                {
                    if (agent.hit.transform.TryGetComponent(out BarricadeWall barricade) && !barricade.IsDestroyed)
                    {
                        Debug.Log("Barricade");
                        agent.navMeshAgent.isStopped = true;
                        agent.attackWall = true;
                        agent.stateMachine.ChangeState(NPCStateId.Attack);
                    }
                }
            }


            if (player.isDead)
            {
                agent.stateMachine.ChangeState(NPCStateId.Idle);
            }

            else if (!agent.aiHealth.isDead && !(distance > agent.config.attackRadius + agent.config.offsetAttackRadius))
            {
                //Debug.Log("Attack");
                agent.navMeshAgent.isStopped = true;
                agent.stateMachine.ChangeState(NPCStateId.Attack);
            }
        }
    }
}