using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NPCAttackWallState : NPCState
{
    private float timer;
    private float attackTime;

    private Vector3 offset;
    private PlayerHealth playerHealth;
    private Player player;

    private RaycastHit hit;

    public NPCStateId GetId()
    {
        return NPCStateId.AttackWall;
    }

    void NPCState.Enter(NPCAgent agent)
    {
        Debug.Log("Enter to Attack Wall State");
        attackTime = agent.config.attackTime;
        agent.isAttacking = true;
        playerHealth = agent.playerTransform.GetComponent<PlayerHealth>();
        player = agent.playerTransform.GetComponent<Player>();
    }
    void NPCState.Update(NPCAgent agent)
    {
        agent.animator.SetBool("isAttacking", agent.isAttacking);
        timer -= Time.deltaTime;

        FacePlayer(agent, Vector3.zero);

        float distance = Vector3.Distance(player.transform.position, agent.transform.position);
        if (!agent.aiHealth.isDead && distance > agent.config.attackRadius + agent.config.offsetAttackRadius && !agent.attackWall)
        {
            agent.stateMachine.ChangeState(NPCStateId.ChasePlayer);
        }
        else if (agent.aiHealth.isDead)
        {
            agent.stateMachine.ChangeState(NPCStateId.Death);
        }
        else if (!agent.aiHealth.isDead && (distance <= agent.config.attackRadius + agent.config.offsetAttackRadius || agent.attackWall) && !playerHealth.IsDead)
        {

            Ray ray = new Ray(agent.transform.position, agent.transform.forward);
            if (Physics.Raycast(ray, out agent.hit, 3f))
            {
                if (agent.hit.collider.TryGetComponent(out BarricadeSpawner barricade) && !barricade._isDestroyed)
                {
                    agent.attackWall = false;
                    agent.stateMachine.ChangeState(NPCStateId.ChasePlayer);
                }

                else if (barricade._isDestroyed)
                {
                    agent.attackWall = true;
                    agent.stateMachine.ChangeState(NPCStateId.Attack);
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