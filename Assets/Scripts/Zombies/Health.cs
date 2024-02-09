using System;
using UnityEngine;

public class Health : MonoBehaviour, IDamagable
{
    public int maxHealth = 100;
    public int currentHealth;
    public bool isDead = false;
    public bool isInjured = false;
    public float dieForce = 2f;
    private HitBox hitBox;
    private Rigidbody[] rigidBody;


    public event Action OnHealthChange;
    public event Action OnDied;

    public void Start()
    {
        currentHealth = maxHealth;
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
        if (currentHealth <= 0)
        {
            Die(direction);
        }
    }

    public void Die(Vector3 direction)
    {
        Debug.Log("Dead");

        currentHealth = 0;
        isInjured = false;
        isDead = true;

        if (WaveSpawner.Instance != null)
        {
            GlobalEventManager.SendEnemyKilled(WaveSpawner.Instance.EnemyCount);
        }
    }
}
