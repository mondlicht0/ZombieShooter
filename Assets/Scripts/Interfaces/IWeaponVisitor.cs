using UnityEngine;

public interface IWeaponVisitor
{
    public void Visit(EnemyHitBox enemy);
    //public void Visit(EnemyHeadHitBox head);

    public void Visit(SO_Gun gun);
}
