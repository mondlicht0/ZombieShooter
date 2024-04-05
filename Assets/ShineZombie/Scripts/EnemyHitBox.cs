using UnityEngine;

public class EnemyHitBox : HitBox
{
    public EnemyHitBoxType Type;
    public Transform SlicedPart = null;

    public Health Health { get; private set; }

    public override void Accept(IWeaponVisitor visitor)
    {
        if (SlicedPart != null) { SlicedPart.gameObject.SetActive(false); }

        visitor.Visit(this);
    }

    private void Awake()
    {
        Health = transform.root.GetComponent<Health>();
    }

}

public enum EnemyHitBoxType
{
    Head,
    Body
}
