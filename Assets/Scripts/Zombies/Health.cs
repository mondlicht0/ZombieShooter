using System;
using System.Collections.Generic;
using UnityEngine;

public class Health : MonoBehaviour, IDamagable
{
    public int maxHealth = 100;
    public int currentHealth;
    public bool isDead = false;
    public bool isInjured = false;
    public float dieForce = 2f;
    [SerializeField] private List<HitBox> hitBoxes;
    private Rigidbody[] rigidBody;


    public event Action OnHealthChange;
    public event Action OnDied;

    public void Start()
    {
        currentHealth = maxHealth;
        hitBoxes.AddRange(gameObject.GetComponentsInChildren<HitBox>());
    }

    public void TakeDamage(int damage, Vector3 direction, int multiplier = 1)
    {
        int damageTaken = Mathf.Clamp(damage, 0, currentHealth);

        if (currentHealth > 0)
        {
            currentHealth -= damageTaken * multiplier;
        }
        if (currentHealth <= 30f && currentHealth > 0)
        {
            isInjured = true;
        }
        if (currentHealth <= 0 && !isDead)
        {
            Die(direction);
        }
    }

    public void Die(Vector3 direction)
    {
        Debug.Log("Dead");
        isDead = true;
        foreach (var box in hitBoxes)
        {
            box.enabled = false;
        }

        currentHealth = 0;
        isInjured = false;
        

        if (WaveSpawner.Instance != null)
        {
            GlobalEventManager.SendEnemyKilled(WaveSpawner.Instance.EnemyCount);
        }
    }
}
