using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Health : MonoBehaviour, IDamagable
{
    private Zombie _zombie;
    public int maxHealth = 100;
    public int currentHealth;
    public bool isDead = false;
    public bool isInjured = false;
    public float dieForce = 2f;
    public float _fadeOutDelay = 2f;
    [SerializeField] private List<BoxCollider> hitBoxes;
    private Rigidbody[] rigidBody;


    public event Action OnHealthChange;
    public event Action OnDied;

    private void Awake()
    {
        _zombie = GetComponent<Zombie>();
    }

    public void Start()
    {
        currentHealth = maxHealth;
        hitBoxes.AddRange(gameObject.GetComponentsInChildren<BoxCollider>());
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
            //isDead = true;
            Die(direction);
        }
    }

    public IEnumerator FadeOut()
    {
        Debug.Log("Fade out");
        yield return new WaitForSeconds(_fadeOutDelay);

        float time = 0;
        while (time < 1)
        {
            transform.position += Vector3.down * Time.deltaTime;
            time += Time.deltaTime;
            yield return null;
        }

        Destroy(gameObject);
        //gameObject.SetActive(false);
    }

    public void Die(Vector3 direction)
    {
        Debug.Log("Dead");
        isDead = true;
        _zombie.ZombieRagdoll.ActivateRagdol();

        currentHealth = 0;
        isInjured = false;
        

        if (WaveSpawner.Instance != null)
        {
            GlobalEventManager.SendEnemyKilled(WaveSpawner.Instance.EnemyCount);
        }

        StartCoroutine(FadeOut());

    }
}
