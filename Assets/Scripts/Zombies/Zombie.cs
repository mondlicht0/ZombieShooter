using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class Zombie : MonoBehaviour
{
    private NavMeshAgent _agent;
    private Animator _animator;

    private Transform _target;
    private Transform _targetLook;

    private PlayerHealth _health;

    private void Awake()
    {
        _agent = GetComponent<NavMeshAgent>();
        _animator = GetComponent<Animator>();
        _health = FindObjectOfType<PlayerHealth>();

        _target = GameObject.FindGameObjectWithTag("Player").transform;
        _targetLook = transform.Find("Look At");
    }

    private void Update()
    {
        _agent.SetDestination(_target.transform.position);

        if (_agent.velocity == Vector3.zero)
        {
            _targetLook.transform.LookAt(_target);
            transform.rotation = Quaternion.Euler(0, _targetLook.eulerAngles.y, 0);
            _animator.SetBool("isWalking", false);

            //_health.TakeDamage(1, Vector3.zero);
        }

        else
        {
            _animator.SetBool("isWalking", true);
        }
    }
}
