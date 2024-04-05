using Unity.MLAgents;
using UnityEngine;
using UnityEngine.AI;

public class Zombie : MonoBehaviour
{
	[Header("Combat")]
	[SerializeField] private float _attackCd = 2f;
	[SerializeField] private float _attackRange = 3f;
	[SerializeField] private EnemyDamageDealer[] _damageDealers;
	
	private LookAt _lookAt;
	
	private float _attackTimer;
	private bool _isAttacking;
	
	private Vector2 _velocity;
	private Vector2 _smoothDeltaPosition;
	
	private Player _player;
	public Animator Animator { get; private set; }
	public NavMeshAgent Agent { get; private set; }
	public Health Health { get; private set; }
	
	private void Awake()
	{
		Animator = GetComponent<Animator>();
		Health = GetComponent<Health>();
		Agent = GetComponent<NavMeshAgent>();
		_damageDealers = GetComponentsInChildren<EnemyDamageDealer>();
		_lookAt = GetComponent<LookAt>();
		
		// Animator.applyRootMotion = true;
		// Agent.updatePosition = false;
		// Agent.updateRotation = true;
	}
	
	// void OnAnimatorMove()
	// {
	// 	Vector3 rootPosition = Animator.rootPosition;
	// 	rootPosition.y = Agent.nextPosition.y;
	// 	transform.position = rootPosition;
	// 	Agent.nextPosition = rootPosition;
	// 	transform.rotation = Animator.rootRotation;
	// }
	
	private void SynchronizeAnimatorAndAgent()
	{
		Vector3 worldDeltaPosition = Agent.nextPosition - transform.position;
		worldDeltaPosition.y = 0;
		// Map 'worldDeltaPosition' to local space
		float dx = Vector3.Dot(transform.right, worldDeltaPosition);
		float dy = Vector3.Dot(transform.forward, worldDeltaPosition);
		Vector2 deltaPosition = new Vector2(dx, dy);
		


		// Low-pass filter the deltaMove
		float smooth = Mathf.Min(1, Time.deltaTime / 0.1f);
		_smoothDeltaPosition = Vector2.Lerp(_smoothDeltaPosition, deltaPosition, smooth);

		_velocity = _smoothDeltaPosition / Time.deltaTime;
		if (Agent.remainingDistance <= Agent.stoppingDistance)
		{
			_velocity = Vector2.Lerp(Vector2.zero, _velocity, Agent.remainingDistance);
		}

		bool shouldMove = _velocity.magnitude > 0.5f && Agent.remainingDistance > Agent.stoppingDistance;

		//Animator.SetBool("move", shouldMove);
		Animator.SetFloat("DirectionX", _velocity.x);
		Animator.SetFloat("DirectionZ", _velocity.y);

		_lookAt.lookAtTargetPosition = Agent.steeringTarget + transform.forward;

		//float deltaMagnitude = worldDeltaPosition.magnitude;
		//if (deltaMagnitude > Agent.radius / 2)
		//{
		//	transform.position = Vector3.Lerp(Animator.rootPosition, Agent.nextPosition, smooth);
		//}
	}
	
	private void Start()
	{
		_player = FindObjectOfType<Player>();
		
	}
	
	void Update()
	{
		if (!Health.IsDead) 
		{
			Agent.SetDestination(_player.transform.position);
			SyncRootMotion();
			Attack();
		}
		
	}
	
	private void SyncRootMotion() 
	{
		if (Agent.hasPath && !_isAttacking) 
		{
			Vector3 direction = (Agent.steeringTarget - transform.position).normalized;
			Vector3 animDir = transform.InverseTransformDirection(direction);
			bool isFacingMoveDir = Vector3.Dot(direction, transform.forward) > 0.5f;
			
			transform.rotation = Quaternion.RotateTowards(transform.rotation, Quaternion.LookRotation(direction), 180 * Time.deltaTime);
			
			Animator.SetFloat("DirectionX", isFacingMoveDir ? animDir.x : 0, 0.5f, Time.deltaTime);
			Animator.SetFloat("DirectionZ", isFacingMoveDir ? animDir.z : 0, 0.5f, Time.deltaTime);
		}
		
		else 
		{
			Animator.SetFloat("DirectionX", 0, 0.25f, Time.deltaTime);
			Animator.SetFloat("DirectionZ", 0, 0.25f, Time.deltaTime);
		}
	}
	
	private void Attack() 
	{
		if (_attackTimer >= _attackCd && Animator.GetCurrentAnimatorStateInfo(0).normalizedTime > 1.0f) 
		{
			if (Vector3.Distance(_player.transform.position, transform.position) <= _attackRange) 
			{
				_isAttacking = true;
				Animator.SetTrigger("Attack");
				Animator.SetFloat("AttackVars", Random.Range(0, 3));
				_attackTimer = 0;
			}
		}
		
		if (Vector3.Distance(_player.transform.position, transform.position) > _attackRange) 
		{
			_isAttacking = false;
		}
		
		_attackTimer += Time.deltaTime;
	}
	
	public void StartDealDamage() 
	{
		foreach (EnemyDamageDealer dealer in _damageDealers) 
		{
			dealer.StartDealDamage();
		}
	}
	
	public void EndDealDamage() 
	{
		foreach (EnemyDamageDealer dealer in _damageDealers) 
		{
			dealer.EndDealDamage();
		}
	}
}
