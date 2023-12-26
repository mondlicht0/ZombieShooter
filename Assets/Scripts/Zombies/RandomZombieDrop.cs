using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RandomZombieDrop : MonoBehaviour
{
    [SerializeField] private GameObject _woodPrefab;
    private Health _zombieHealth;

    private float _dropChance;

    private void Awake()
    {
        _zombieHealth = GetComponent<Health>();

        GlobalEventManager.OnEnemyKilled.AddListener(OnZombieDied);
    }

    public void OnZombieDied(int remainingEnemies)
    {
        DropWood();
    }

    public void DropWood()
    {
        float dropChance = Random.Range(0, _dropChance);

        Instantiate(_woodPrefab);
    }
}
