using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ZombieManager : MonoBehaviour
{
    [SerializeField] private ZombieIdleState _startingState;

    [SerializeField] private ZombieState _currentState;

    public Player CurrentTarget;

    private void Awake()
    {
        _currentState = _startingState;
    }

    private void FixedUpdate()
    {
        HandleStateMachine();
    }

    private void HandleStateMachine()
    {
        ZombieState nextState;

        if (_currentState)
        {
            nextState = _currentState.Tick(this);

            if (nextState != null)
            {
                _currentState = nextState;
            }
        }
    }
}
