using UnityEngine;

public interface IWeaponVisitor
{
    public void Visit(EnemyHitBox enemy);
    public void Visit(EnemyHeadHitBox head);
}
