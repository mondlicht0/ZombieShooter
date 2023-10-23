using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace baponkar.npc.zombie
{
    public class HitBox : MonoBehaviour, IDamagable
    {
        public Transform Root;
        public bool isHead = false;
        public Health health;

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
        }

        private void Awake()
        {
            health = Root.GetComponent<Health>();
        }

        // public void OnRaycastHit(RaycastWeapon weapon, Vector3 direction)
        // {
        //     health.TakeDamage(weapon.damage, direction);
        // }
    }
}
