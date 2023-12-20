using System.Collections;
using System.Collections.Generic;
using TMPro;
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
    public bool isIdleing = false;
    public bool isPatrolling = false;
    public bool isChaseing = false;
    public bool isAttacking = false;
    public bool isDead = false;
    public bool playerSeen = false;
    public bool attackWall = false;

    public TextMeshProUGUI DebugStateText;

    public RaycastHit hit;
    public NavMeshHit navHit;

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
            playerTransform = GameObject.FindGameObjectWithTag("Player")?.transform;
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

    public void AttackAnimationEvent()
    {
        Ray ray = new Ray(transform.position, transform.forward);

        if(playerTransform.TryGetComponent(out PlayerHealth health) && !aiHealth.isDead && Vector3.Distance(playerTransform.position, transform.position) <= config.attackRadius + config.offsetAttackRadius && !health.IsDead)
        {
            health.TakeDamage(config.attackDamage, Vector3.zero);
        }

        if (Physics.Raycast(ray, out hit, 1f))
        {
            if (hit.collider.TryGetComponent(out BarricadeWall barricade))
            {
                barricade.RemoveBoard();
            }
        }
    }
}

