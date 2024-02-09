using Unity.MLAgents;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.AI;
using UnityHFSM;

[RequireComponent(typeof(Animator), typeof(NavMeshAgent))]
public class Zombie : MonoBehaviour
{
    [SerializeField]
    private Player _player;

    [SerializeField]
    private Health _health;

    private StateMachine<ZombieState, StateEvent> _zombieFSM;
    private Animator _animator;
    private NavMeshAgent _agent;

    [Header("Player Sensors")]
    [SerializeField]
    private PlayerSensor _followPlayerSensor;
    [SerializeField]
    private PlayerSensor _meleePlayerSensor;
    [SerializeField]
    private PlayerSensor _rightHandAttackSensor;
    [SerializeField]
    private PlayerSensor _leftHandAttackSensor;

    [Header("Attack Config")]
    [SerializeField]
    [Range(0.1f, 5f)]
    private float _attackCooldown = 2;  
    private float _damage = 10;  

    [Space]
    [Header("Debug Info")]
    [SerializeField]
    private bool _isInMeleeRange;
    [SerializeField]
    private bool _isInWallRange;
    [SerializeField]
    private bool _isInSpitRange;
    [SerializeField]
    private bool _isInChasingRange;
    [SerializeField]
    private float _lastAttackTime;
    [SerializeField]
    private float _lastBounceTime;
    [SerializeField]
    private float _lastRollTime;

    private void Awake()
    {
        _animator = GetComponent<Animator>();
        _agent = GetComponent<NavMeshAgent>();
        _health = GetComponent<Health>();
        _zombieFSM = new();


        _player = FindObjectOfType<Player>();

        _zombieFSM.AddState(ZombieState.Idle, new ZombieIdleState(false, this));
        _zombieFSM.AddState(ZombieState.ChasePlayer, new ZombieChaseState(true, this, _player.transform));
        _zombieFSM.AddState(ZombieState.AttackPlayer, new ZombieAttackState(true, this, OnAttack));
        _zombieFSM.AddState(ZombieState.AttackWall, new ZombieAttackWallState(true, this));
        _zombieFSM.AddState(ZombieState.Dead, new ZombieDeadState(false, this));

        // Transitions

        _zombieFSM.AddTriggerTransition(StateEvent.DetectPlayer, new Transition<ZombieState>(ZombieState.Idle, ZombieState.ChasePlayer));
        //_zombieFSM.AddTriggerTransition(StateEvent.DetectWall, new Transition<ZombieState>(ZombieState.Idle, ZombieState.AttackWall));

        _zombieFSM.AddTransition(new Transition<ZombieState>(ZombieState.Idle, ZombieState.ChasePlayer, 
                                (transition) => !_isInChasingRange || Vector3.Distance(_player.transform.position, transform.position) <= _agent.stoppingDistance));


        // Attack Player
        _zombieFSM.AddTransition(new Transition<ZombieState>(ZombieState.Idle, ZombieState.AttackPlayer, ShouldMelee, forceInstantly: true));
        _zombieFSM.AddTransition(new Transition<ZombieState>(ZombieState.ChasePlayer, ZombieState.AttackPlayer, ShouldMelee, forceInstantly: true));

        _zombieFSM.AddTransition(new Transition<ZombieState>(ZombieState.AttackPlayer, ZombieState.ChasePlayer, IsWithinIdleRange));
        _zombieFSM.AddTransition(new Transition<ZombieState>(ZombieState.AttackPlayer, ZombieState.Idle, IsNotWithinIdleRange));

        // Attack Wall
/*        _zombieFSM.AddTransition(new Transition<ZombieState>(ZombieState.Idle, ZombieState.AttackWall, ShouldAttackWall, forceInstantly: true));
        _zombieFSM.AddTransition(new Transition<ZombieState>(ZombieState.ChasePlayer, ZombieState.AttackWall, ShouldAttackWall, forceInstantly: true));
        _zombieFSM.AddTransition(new Transition<ZombieState>(ZombieState.AttackPlayer, ZombieState.AttackWall, ShouldAttackWall, forceInstantly: true));*/

/*        _zombieFSM.AddTransition(new Transition<ZombieState>(ZombieState.AttackWall, ZombieState.Idle, IsWithinIdleRange, forceInstantly: true));
        _zombieFSM.AddTransition(new Transition<ZombieState>(ZombieState.AttackWall, ZombieState.ChasePlayer, ShouldAttackWall, forceInstantly: true));
        _zombieFSM.AddTransition(new Transition<ZombieState>(ZombieState.AttackWall, ZombieState.AttackPlayer, ShouldAttackWall, forceInstantly: true));*/

        // Dead State Transition
        _zombieFSM.AddTransition(new Transition<ZombieState>(ZombieState.Idle, ZombieState.Dead, IsDead));
        _zombieFSM.AddTransition(new Transition<ZombieState>(ZombieState.ChasePlayer, ZombieState.Dead, IsDead));
        _zombieFSM.AddTransition(new Transition<ZombieState>(ZombieState.AttackPlayer, ZombieState.Dead, IsDead));
       // _zombieFSM.AddTransition(new Transition<ZombieState>(ZombieState.AttackWall, ZombieState.Dead, IsDead));

        // AddTransitionFromAny
        _zombieFSM.SetStartState(ZombieState.Idle);
        _zombieFSM.Init();

    }

    private void Start()
    {
        _followPlayerSensor.OnPlayerEnter += FollowPlayerSensorOnPlayerEnter;

        _meleePlayerSensor.OnPlayerEnter += MeleePlayerSensorOnPlayerEnter;
        _meleePlayerSensor.OnPlayerExit += MeleePlayerSensorOnPlayerExit;

        _leftHandAttackSensor.OnPlayerEnter += AttackAnimationEvent;
        _rightHandAttackSensor.OnPlayerEnter += AttackAnimationEvent;
    }


    private void OnAttack(State<ZombieState, StateEvent> state)
    {
        transform.LookAt(_player.transform.position);
        _lastAttackTime = Time.time;
    }

    private void Update()
    {
        _zombieFSM.OnLogic();
    }

    private void FollowPlayerSensorOnPlayerEnter(Transform player)
    {
        _zombieFSM.Trigger(StateEvent.DetectPlayer);
        _isInChasingRange = true;
    }

    private void MeleePlayerSensorOnPlayerExit(Vector3 lastKnownPosition) => _isInMeleeRange = false;

    private void MeleePlayerSensorOnPlayerEnter(Transform player) => _isInMeleeRange = true;

    private bool ShouldMelee(Transition<ZombieState> transition) => _lastAttackTime + _attackCooldown <= Time.time && _isInMeleeRange;
    private bool ShouldAttackWall(Transition<ZombieState> transition)
    {
        if (_isInMeleeRange)
        {
            Ray ray = new Ray(_agent.transform.position + Vector3.up, _agent.transform.forward);

            if (Physics.Raycast(ray, out RaycastHit hit, 2f))
            {
                if (hit.transform.TryGetComponent(out BarricadeWall barricade) && !barricade.IsDestroyed)
                {
                    _isInWallRange = true;
                }
            }

            else
            {
                _isInWallRange = false;
            }
        }

        return _isInWallRange;
    } 

    private bool IsNotInWallRange(Transition<ZombieState> transition)
    {
        if (_isInMeleeRange)
        {
            Ray ray = new Ray(_agent.transform.position + Vector3.up, _agent.transform.forward);

            if (Physics.Raycast(ray, out RaycastHit hit, 2f))
            {
                if (hit.transform.TryGetComponent(out BarricadeWall barricade) && !barricade.IsDestroyed)
                {
                    _isInWallRange = true;
                }
            }
        }

        return _isInWallRange;
    }



    private bool IsWithinIdleRange(Transition<ZombieState> transition) => _agent.remainingDistance <= _agent.stoppingDistance;
    private bool IsNotWithinIdleRange(Transition<ZombieState> transition) => !IsWithinIdleRange(transition);
    private bool IsDead(Transition<ZombieState> transition) => _health.isDead;

    public void AttackAnimationEvent(Transform player)
    {
        if (player.TryGetComponent(out PlayerHealth health))
        {
            health.TakeDamage(10, Vector3.forward);
        }
    }
}
