using System.Collections;
using System.Collections.Generic;
using Unity.MLAgents;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.AI;


    // [RequireComponent(typeof(UnityEngine.AI.NavMeshAgent))]
    // [RequireComponent(typeof(NPCHealth))]
    // [RequireComponent(typeof(Rigidbody))]
    // [RequireComponent(typeof(DebugNavmeshAgent))]
    // [RequireComponent(typeof(NPCVisonSensor))]
    // [RequireComponent(typeof(NPCSoundSensor))]
    // [RequireComponent(typeof(AudioSource))]
    // [RequireComponent(typeof(CapsuleCollider))]
    // [RequireComponent(typeof(NPCCall))]

public class NPCAgent : MonoBehaviour
{
    [HideInInspector]
    public Transform playerTransform;
    public NPCStateMachine stateMachine;
    public NPCStateId initialState;
       
    [HideInInspector]
    public NavMeshAgent navMeshAgent;
    public NPCAgentConfig config;
    [HideInInspector]
    public NPCVisonSensor visonSensor;
    [HideInInspector]
    public NPCSoundSensor soundSensor;
    [HideInInspector]
    public Health aiHealth;
    [HideInInspector]
    public NPCRagdol ragdoll;

    private Animator _animator;
    [HideInInspector]
    public CapsuleCollider capsuleCollider;
        
    [Header("NPC Zombie Agent.")]
    public bool isIdleing;
    public bool isPatrolling;
    public bool isChaseing;
    public bool isAttacking;
    public bool isDead;
    public bool playerSeen = false;

    private WaveSpawner _waveSpawner;

    [HideInInspector]
    public Vector3 initialPosition;

    public Animator animator { get { return _animator; } }

    private void Awake()
    {
        _waveSpawner = FindObjectOfType<WaveSpawner>();
    }

    void Start()
    {
        if(playerTransform == null){
            playerTransform = GameObject.FindGameObjectWithTag("Player").transform;
        }

        initialPosition = this.transform.position;
        navMeshAgent = GetComponent<NavMeshAgent>();
        visonSensor = GetComponentInChildren<NPCVisonSensor>();
        soundSensor = GetComponentInChildren<NPCSoundSensor>();
        aiHealth = GetComponent<Health>();
        ragdoll = GetComponent<NPCRagdol>();
        _animator = GetComponentInChildren<Animator>();
        capsuleCollider = GetComponent<CapsuleCollider>();
            
        stateMachine = new NPCStateMachine(this);
        stateMachine.RegisterState(new NPCChasePlayerState());
        stateMachine.RegisterState(new NPCDeathState());
        stateMachine.RegisterState(new NPCIdleState());
        stateMachine.RegisterState(new NPCPatrolState());
        stateMachine.RegisterState(new NPCAttackState());
        

        stateMachine.ChangeState(initialState);

        _animator.applyRootMotion = true;

        navMeshAgent.updatePosition = false;
        navMeshAgent.updateRotation = true;
    }

        
    void Update()
    {
        stateMachine.Update();
    }

    private void OnDestroy()
    {
        int enemiesLeft = 0;
        enemiesLeft = FindObjectsOfType<NPCAgent>().Length;

        if (enemiesLeft == 0)
            _waveSpawner.LaunchWave();
    }

    public void AttackPlayer()
    {
        if(playerTransform.TryGetComponent(out PlayerHealth health) && !aiHealth.isDead && Vector3.Distance(playerTransform.position, transform.position) <= config.attackRadius + config.offsetAttackRadius && !health.isDead)
        {
            health.TakeDamage(config.attackDamage, Vector3.zero);
        }
    }


}

