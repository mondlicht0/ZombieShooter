using UnityEngine;
using UnityEngine.AI;

public class NPCLocomotion : MonoBehaviour
{
    [SerializeField] private NPCAgent _agent;
    [SerializeField] private NavMeshAgent _navMeshAgent;
    [SerializeField] private Animator _animator;
    private float _speed;


    private Vector2 _velocity;
    private Vector2 _smoothDeltaPosition;


    void Awake()
    {
        _speed = 0;   
    }

    void Update()
    {
        /*_speed = Mathf.Clamp(_agent.navMeshAgent.velocity.magnitude, 0, 1);
        _agent.animator.SetFloat("Speed", _speed);*/

        SynchAnimatorAndAgent();
    }

    private void OnAnimatorMove()
    {
        Vector3 rootPosition = _animator.rootPosition;
        rootPosition.y = _navMeshAgent.nextPosition.y;
        transform.position = rootPosition;
        _navMeshAgent.nextPosition = rootPosition;
    }

    private void SynchAnimatorAndAgent()
    {
        Vector3 worldDeltaPosition = _navMeshAgent.nextPosition - transform.position;
        worldDeltaPosition.y = 0;

        float dx = Vector3.Dot(transform.right, worldDeltaPosition);
        float dy = Vector3.Dot(transform.forward, worldDeltaPosition);
        Vector2 deltaPosition = new Vector2(dx, dy);

        float smooth = Mathf.Min(1, Time.deltaTime / 0.1f);
        _smoothDeltaPosition = Vector2.Lerp(_smoothDeltaPosition, deltaPosition, smooth);

        _velocity = _smoothDeltaPosition / Time.deltaTime;

        if (!_agent.isDead)
        {
            if (_navMeshAgent.remainingDistance <= _navMeshAgent.stoppingDistance)
            {
                _velocity = Vector2.Lerp(Vector2.zero, _velocity, _navMeshAgent.remainingDistance / _navMeshAgent.stoppingDistance);
            }

            bool shouldMove = _velocity.magnitude > 0.5f && _navMeshAgent.remainingDistance > _navMeshAgent.stoppingDistance;

            _animator.SetFloat("Speed", _velocity.magnitude);

            float deltaMagnitude = worldDeltaPosition.magnitude;
            if (deltaMagnitude > _navMeshAgent.radius / 2f)
            {
                transform.position = Vector3.Lerp(_animator.rootPosition, _navMeshAgent.nextPosition, smooth);
            }
        }
    }
}