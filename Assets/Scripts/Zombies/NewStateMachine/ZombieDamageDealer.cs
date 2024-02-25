using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ZombieDamageDealer : MonoBehaviour
{
    [SerializeField] private Zombie _zombie;

    private bool _canDealDamage = false;
    private bool _hasDealtDamage = false;

    [SerializeField] private float _armLength = 2f;
    [SerializeField] private LayerMask _playerLayer;

    private void Update()
    {
        Debug.DrawRay(transform.position, transform.forward, Color.red);

        if (_canDealDamage && !_zombie.Health.isDead && !_hasDealtDamage)
        {
            RaycastHit hit;

            if (Physics.Raycast(transform.position, transform.up, out hit, _armLength, _playerLayer))
            {
        Debug.Log("Start");
                
                if (hit.collider.TryGetComponent(out PlayerHealth playerHealth))
                {
                    playerHealth.TakeDamage(_zombie.Damage, Vector3.zero);
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

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.yellow;
        Gizmos.DrawLine(transform.position, transform.position - transform.up * _armLength);
    }
}
