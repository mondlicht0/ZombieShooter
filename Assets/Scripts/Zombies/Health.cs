using baponkar.npc.zombie;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Health : MonoBehaviour, IDamagable
{
    public int maxHealth = 100;
    public int currentHealth;
    public bool isDead = false;
    public bool isInjured = false;
    public float dieForce = 2f;
    NPCRagdol ragdol;
    HitBox hitBox;
    Rigidbody[] rigidBody;

    public void Start()
    {
        ragdol = GetComponent<NPCRagdol>();
        rigidBody = GetComponentsInChildren<Rigidbody>();
        for (int i = 0; i < rigidBody.Length; i++)
        {
            hitBox = rigidBody[i].gameObject.AddComponent<HitBox>();
            hitBox.health = this;
        }
        currentHealth = maxHealth;

        HitBox[] hitBoxes = gameObject.GetComponentsInChildren<HitBox>();
        
        foreach (HitBox hitBox in hitBoxes)
        {
            hitBox.health = this;
        }
    }

    public void TakeDamage(int damage, Vector3 direction)
    {
        int damageTaken = Mathf.Clamp(damage, 0, currentHealth);

        if (currentHealth > 0)
        {
            currentHealth -= damage;
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
        ragdol.ActivateRagdol();
    }
}
