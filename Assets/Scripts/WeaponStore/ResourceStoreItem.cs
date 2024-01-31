using UnityEngine;

[CreateAssetMenu(fileName = "Resource Store Item", menuName = "Store Items/Resource Item")]
public class ResourceStoreItem : StoreItem
{
    public int Amount;

    protected override void Purchase()
    {
        PlayerHealth health = FindObjectOfType<Player>().Health;

        if (health.CurrentHealth != health.MaxHealth)
        {
            health.AddHealth(Amount);
        }
    }
}
