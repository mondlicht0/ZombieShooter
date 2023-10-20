using baponkar.npc.zombie;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class NPCPatrolState : NPCState
{

    float patrolRadius;
    float patrolTurnSpeed;
    float patrolWalkingSpeed;
    bool walkPointSet;
    Vector3 tempTarget;
    Vector3 lastTempTarget;
    float timer;
    float maxTime = 1f;
    NavMeshPath navMeshPath;
    Vector3 initialPosition;



    public NPCStateId GetId()
    {
        return NPCStateId.Patrol;
    }
    void NPCState.Enter(NPCAgent agent)
    {
        agent.isPatrolling = true;
        navMeshPath = new NavMeshPath();
        initialPosition = agent.transform.position;
        patrolRadius = agent.config.patrolRadius;
        //patrolTurnSpeed = agent.config.patrolTurnSpeed;
        //patrolWalkingSpeed = agent.config.patrolWalkingSpeed;
    }

    void NPCState.Exit(NPCAgent agent)
    {
        agent.isPatrolling = false;
    }


    void NPCState.Update(NPCAgent agent)
    {

        bool playerSeen = findThePlayer(agent);
        timer -= Time.deltaTime;
        if (!agent.aiHealth.isDead)
        {
            if (playerSeen || agent.aiHealth.currentHealth < agent.aiHealth.maxHealth)
            {
                agent.stateMachine.ChangeState(NPCStateId.ChasePlayer);
            }
            else
            {

                if (!walkPointSet)
                {
                    SearchingPoint(agent);
                }
                if (walkPointSet && timer < 0f)
                {
                    FacePatrol(agent);
                    //agent.navMeshAgent.speed = agent.config.patrolWalkingSpeed + agent.config.offsetPatrolSpeed;
                    agent.navMeshAgent.SetDestination(tempTarget);
                    lastTempTarget = tempTarget;
                    timer = maxTime;
                }

                //float sqrDistanceToTempTarget = (agent.navMeshAgent.transform.position - tempTarget).sqrMagnitude;

                //if(sqrDistanceToTempTarget <= ((agent.navMeshAgent.stoppingDistance * agent.navMeshAgent.stoppingDistance) + (0.10f * 0.10f)))
                if (agent.navMeshAgent.remainingDistance <= 0.1f)
                {
                    walkPointSet = false;
                }

            }
        }
        else if (agent.aiHealth.isDead)
        {
            agent.stateMachine.ChangeState(NPCStateId.Death);
        }
    }

    void SearchingPoint(NPCAgent agent)
    {
        Vector3 tempPos = Vector3.zero;
        tempPos = RandomNavmeshLocation(agent);
        //tempTarget = new Vector3(agent.navMeshAgent.transform.position.x + tempPos.x, agent.navMeshAgent.transform.position.y, agent.navMeshAgent.transform.position.z + tempPos.z);
        NavMeshHit hit;
        if (NavMesh.SamplePosition(tempPos, out hit, patrolRadius, NavMesh.AllAreas))
        {
            if (agent.navMeshAgent.CalculatePath(hit.position, navMeshPath)) //check a path available or not
            {

                tempTarget = hit.position;
                walkPointSet = true;

            }
        }
        else
        {
            tempTarget = agent.initialPosition;
            walkPointSet = false;
        }
    }

    Vector3 RandomNavmeshLocation(NPCAgent agent)
    {
        Vector3 randomDirection = Random.insideUnitSphere * patrolRadius;
        randomDirection += agent.navMeshAgent.transform.position;
        NavMeshHit hit;
        Vector3 finalPosition = (Vector3)agent.navMeshAgent.transform.position;
        if (NavMesh.SamplePosition(randomDirection, out hit, patrolRadius, 1))
        {
            float distance = Vector3.SqrMagnitude(initialPosition - hit.position);
            if (distance < patrolRadius * patrolRadius)
            {
                finalPosition = hit.position;
                walkPointSet = true;
            }
        }
        return finalPosition;
    }

    void FacePatrol(NPCAgent agent)
    {
        Vector3 direction = (tempTarget - agent.navMeshAgent.transform.position).normalized;
        Quaternion lookRotation = Quaternion.LookRotation(new Vector3(direction.x, 0, direction.z));
        agent.navMeshAgent.transform.rotation = Quaternion.Lerp(agent.navMeshAgent.transform.rotation, lookRotation, Time.time * patrolTurnSpeed);
    }


    bool findThePlayer(NPCAgent agent)
    {
        for (int i = 0; i < agent.visonSensor.Objects.Count; i++)
        {
            if (agent.visonSensor.Objects[i].tag == "Player")
            {
                agent.playerSeen = true;
                return true;
            }
        }
        return false;
    }
}

