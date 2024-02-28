using Unity.MLAgents;
using UnityEngine;
using UnityEngine.AI;
using UnityHFSM;

[RequireComponent(typeof(Animator), typeof(NavMeshAgent))]
public class Zombie : MonoBehaviour
{
	[SerializeField] private Player _player;

	private StateMachine<ZombieState, StateEvent> _zombieFSM;
	public ZombieRagdoll ZombieRagdoll { get; private set; }
	public Animator Animator { get; private set; }
	public Health Health { get; private set; }
	public NavMeshAgent Agent { get; private set; }

	[Header("Player Sensors")]
	[SerializeField] private PlayerSensor _followPlayerSensor;
	[SerializeField] private PlayerSensor _meleePlayerSensor;

	[field: SerializeField] public ZombieDamageDealer LeftDealer { get; private set; }
	[field: SerializeField] public ZombieDamageDealer RightDealer { get; private set; }

	[Header("Attack Config")]
	[SerializeField]
	[Range(0.1f, 5f)]
	private float _attackCooldown = 2;

	public int Damage { get; private set; } = 10;

	[Space]
	[Header("Debug Info")]
	[SerializeField] private bool _isInMeleeRange;
	[SerializeField] private bool _isInWallRange;
	[SerializeField] private bool _isInSpitRange;
	[SerializeField] private bool _isInChasingRange;
	[SerializeField] private float _lastAttackTime;
	[SerializeField] private float _lastBounceTime;
	[SerializeField] private float _lastRollTime;

	private bool _isPlayerOnAttackSensor;
	private Ray ray;
	private RaycastHit hit;
	[SerializeField] private float distance;

	private void Awake()
	{
		Animator = GetComponent<Animator>();
		Health = GetComponent<Health>();
		Agent = GetComponent<NavMeshAgent>();
		ZombieRagdoll = GetComponent<ZombieRagdoll>();
		_zombieFSM = new();

		ray = new Ray(transform.position, transform.forward);

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
								(transition) => !_isInChasingRange || Vector3.Distance(_player.transform.position, transform.position) <= Agent.stoppingDistance));


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

	private void OnDestroy()
	{
		int enemiesLeft = 0;
		enemiesLeft = WaveSpawner.Instance.EnemyCount;

		if (enemiesLeft == 0) 
		{
			if (WaveSpawner.Instance != null) 
			{
				WaveSpawner.Instance.LaunchWave();
			}
		}
	}


	private void MeleePlayerSensorOnPlayerExit(Vector3 lastKnownPosition) => _isInMeleeRange = false;

	private void MeleePlayerSensorOnPlayerEnter(Transform player) => _isInMeleeRange = true;

	private bool ShouldMelee(Transition<ZombieState> transition) => _lastAttackTime + _attackCooldown <= Time.time && _isInMeleeRange;

	private bool ShouldAttackWall(Transition<ZombieState> transition)
	{
		if (_isInMeleeRange)
		{
			Ray ray = new Ray(Agent.transform.position + Vector3.up, Agent.transform.forward);

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
			Ray ray = new Ray(Agent.transform.position + Vector3.up, Agent.transform.forward);

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

	private bool IsWithinIdleRange(Transition<ZombieState> transition) => Agent.enabled ? Agent.remainingDistance <= Agent.stoppingDistance : false;

	private bool IsNotWithinIdleRange(Transition<ZombieState> transition) => !IsWithinIdleRange(transition);

	private bool IsDead(Transition<ZombieState> transition) => Health.isDead;
}
