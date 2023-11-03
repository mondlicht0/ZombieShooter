using System.Collections;
using System.Collections.Generic;
using Unity.MLAgents;
using UnityEngine;

public class Health : MonoBehaviour, IDamagable
{
    public int maxHealth = 100;
    public int currentHealth;
    public bool isDead = false;
    public bool isInjured = false;
    public float dieForce = 2f;
    private NPCRagdol ragdol;
    private HitBox hitBox;
    private Rigidbody[] rigidBody;

    public NPCRagdol Ragdol { get { return ragdol; } }

    public void Start()
    {
        ragdol = GetComponent<NPCRagdol>();

        /*
        rigidBody = GetComponentsInChildren<Rigidbody>();
        for (int i = 0; i < rigidBody.Length; i++)
        {
            hitBox = rigidBody[i].gameObject.AddComponent<HitBox>();
            if (rigidBody[i].gameObject.CompareTag("Head")) hitBox.isHead = true;
            hitBox.health = this;
        }

        currentHealth = maxHealth;

        HitBox[] hitBoxes = gameObject.GetComponentsInChildren<HitBox>();
        
        foreach (HitBox hitBox in hitBoxes)
        {
            hitBox.health = this;
        }*/

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
            currentHealth = 0;
            isInjured = false;
            isDead = true;
            Die(direction);
        }
    }

    public void Die(Vector3 direction)
    {
        ragdol.GetComponent<NPCAgent>().isDead = true;
        ragdol.ActivateRagdol();
    }
}
