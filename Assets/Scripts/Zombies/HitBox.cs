using UnityEngine;

public abstract class HitBox : MonoBehaviour
{
    public abstract void Accept(IWeaponVisitor visitor);
}
