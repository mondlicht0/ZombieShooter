using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class NPCAttackState : NPCState
{
    private float timer;
    private float attackTime;

    private Vector3 offset;
    private PlayerHealth playerHealth;
    private Player player;

    private RaycastHit hit;

    public NPCStateId GetId()
    {
        return NPCStateId.Attack;
    }

    void NPCState.Enter(NPCAgent agent)
    {
        Debug.Log("Enter to Attack State");
        //Debug.Log("Zombie Attack");
        attackTime = agent.config.attackTime;
        agent.isAttacking = true;
        //offset = new Vector3(Random.Range(-1f, 1f), 0f, Random.Range(-1f, 1f));
        playerHealth = agent.playerTransform.GetComponent<PlayerHealth>();
        player = agent.playerTransform.GetComponent<Player>();
    }
    void NPCState.Update(NPCAgent agent)
    {
        agent.animator.SetBool("isAttacking", agent.isAttacking);
        timer -= Time.deltaTime;

        FacePlayer(agent, Vector3.zero);
        
        float distance = Vector3.Distance(player.transform.position, agent.transform.position);

        if (!agent.aiHealth.isDead && (distance <= agent.config.attackRadius + agent.config.offsetAttackRadius || agent.attackWall) && !playerHealth.IsDead)
        {
            agent.navMeshAgent.isStopped = true;
            Ray ray = new Ray(agent.transform.position, agent.transform.forward);
            if (Physics.Raycast(ray, out agent.hit, 5f))
            {
                if (agent.hit.collider.TryGetComponent(out BarricadeWall barricade) && !barricade.IsDestroyed)
                {
                    agent.attackWall = true;
                }

                else
                {
                    agent.attackWall = false;
                }
            }

            if (playerHealth != null && !playerHealth.IsDead)
            {
                timer -= Time.deltaTime;
                if (timer <= 0)
                {
                    //playerHealth.TakeDamage(agent.config.attackDamage, Vector3.zero);

                    timer = attackTime;
                }
            }

        }


        else if (!agent.aiHealth.isDead && distance > agent.config.attackRadius + agent.config.offsetAttackRadius && !agent.attackWall)
        {
            agent.stateMachine.ChangeState(NPCStateId.ChasePlayer);
        }
        else if (agent.aiHealth.isDead)
        {
            agent.stateMachine.ChangeState(NPCStateId.Death);
        }
        
        
        if (playerHealth.IsDead)
        {
            agent.playerSeen = false;
            agent.stateMachine.ChangeState(NPCStateId.Idle);
        }

    }

    void NPCState.Exit(NPCAgent agent)
    {
        Debug.Log("Exit from Attack State");
        agent.isAttacking = false;
        agent.animator.SetBool("isAttacking", agent.isAttacking);
    }

    private void FacePlayer(NPCAgent agent, Vector3 offset)
    {
        Vector3 direction = (agent.playerTransform.position - agent.navMeshAgent.transform.position + offset).normalized;

        Quaternion lookRotation = Quaternion.LookRotation(new Vector3(direction.x, 0, direction.z));
        agent.navMeshAgent.transform.rotation = Quaternion.Lerp(agent.navMeshAgent.transform.rotation, lookRotation, Time.time * agent.config.patrolTurnSpeed);
    }


}
