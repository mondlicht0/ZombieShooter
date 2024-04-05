using UnityEngine;

public class EnemyDamageDealer : MonoBehaviour
{
	[SerializeField] private float _armLength;
	[SerializeField] private LayerMask _playerMask;
	
	private bool _canDealDamage = false;
	private bool _hasDealtDamage = false;
	
	void Update()
	{
		if (_canDealDamage && !_hasDealtDamage) 
		{
			RaycastHit hit;
			
			if (Physics.Raycast(transform.position, -transform.right, out hit, _armLength, _playerMask)) 
			{
				if (hit.transform.TryGetComponent(out PlayerHealth health)) 
				{
					health.TakeDamage(5, Vector3.zero);
					_hasDealtDamage = true;
				}
			}
		}
	}
	
	public void StartDealDamage() 
	{
		_canDealDamage = true;
		_hasDealtDamage = false;
	}
	
	public void EndDealDamage() 
	{
		_canDealDamage = false;
	}
	
	void OnDrawGizmos()
	{
		Gizmos.color = Color.yellow;
		Gizmos.DrawLine(transform.position, transform.position - transform.right * _armLength);
	}
}
