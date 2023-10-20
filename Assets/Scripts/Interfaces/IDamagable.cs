using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IDamagable
{
    void TakeDamage(int damage, Vector3 direction);

    void Die(Vector3 direction);
}
