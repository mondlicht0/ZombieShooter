using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bootstrap : MonoBehaviour
{
    [SerializeField] private Player _player;
    [SerializeField] private PlayerGunSelector _playerGunSelector;
    [SerializeField] private PlayerAction _playerAction;

/*    private void Awake()
    {
        _playerGunSelector.InitAwake();
        _playerAction.InitAwake();
    }

    private void Start()
    {
        _playerGunSelector.InitStart();
        _playerAction.InitStart();
    }*/
}
