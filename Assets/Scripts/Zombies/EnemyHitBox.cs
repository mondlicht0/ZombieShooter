using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyHitBox : HitBox
{
    public Transform Root;
    public Health Health;

    public override void Accept(IWeaponVisitor visitor)
    {
        visitor.Visit(this);
    }

    private void Awake()
    {
        Health = Root.GetComponent<Health>();
    }



    /*    public Transform Root;
        public bool isHead = false;
        public Health health;*/

    /*    private void Awake()
        {
            health = Root.GetComponent<Health>();
        }

        public void Die(Vector3 direction)
        {
            health.GetComponent<NPCAgent>().isDead = true;
            health.Ragdol.ActivateRagdol();
        }

        public void TakeDamage(int damage, Vector3 direction)
        {
            int damageTaken = Mathf.Clamp(damage, 0, health.currentHealth);

            if (health.currentHealth > 0)
            {
                health.currentHealth -= isHead ? damage + 500 : damage;
            }
            if (health.currentHealth <= 30f && health.currentHealth > 0)
            {
                health.isInjured = true;
            }
            if (health.currentHealth <= 0)
            {
                health.currentHealth = 0;
                health.isInjured = false;
                health.isDead = true;
                Die(direction);
            }
        }*/
}
