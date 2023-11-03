using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyHitBox : HitBox
{
    public EnemyHitBoxType Type;
    public Transform Root;

    private Health _health;

    public Health Health { get => _health; }

    public override void Accept(IWeaponVisitor visitor)
    {
        visitor.Visit(this);
    }

    private void Awake()
    {
        _health = Root.GetComponent<Health>();
    }

}

public enum EnemyHitBoxType
{
    Head,
    Body
}
