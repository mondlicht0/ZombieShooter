using UnityEngine;

public class ZombieIdleState : ZombieState
{
    [SerializeField] private float _detectionRadius;
    [SerializeField] private LayerMask _detectionLayer;



    [SerializeField] private float _minDetectionAngle = -35f;
    [SerializeField] private float _maxDetectionAngle = -35f;

    private ZombieTargetState _targetState;

    private void Awake()
    {
        _targetState = GetComponent<ZombieTargetState>();
    }

    public override ZombieState Tick(ZombieManager zombieManager)
    {
        if (zombieManager.CurrentTarget)
        {
            return _targetState;
        }

        else
        {
            FindTarget(zombieManager);
            return this;
        }
    }

    private void FindTarget(ZombieManager zombieManager)
    {
        Collider[] colliders = Physics.OverlapSphere(transform.position, _detectionRadius, _detectionLayer);

        for (int i = 0; i < colliders.Length; i++)
        {
            Player player = colliders[i].transform.GetComponent<Player>();

            if (player)
            {
                Vector3 targetDirection = transform.position - player.transform.position;
                float viewableAngle = Vector3.Angle(targetDirection, transform.forward);

                if (viewableAngle > _minDetectionAngle && viewableAngle < _maxDetectionAngle)
                {
                    RaycastHit hit;

                    float characterHeight = 2f;
                    Vector3 playerStartPoint = new Vector3(player.transform.position.x, characterHeight, player.transform.position.z);
                    Vector3 zombieStartPoint = new Vector3(transform.position.x, characterHeight, transform.position.z);
                    if (Physics.Linecast(playerStartPoint, zombieStartPoint, out hit))
                    {

                    }

                    else
                    {
                        zombieManager.CurrentTarget = player;
                    }
                }
            }
        }
    }
}
