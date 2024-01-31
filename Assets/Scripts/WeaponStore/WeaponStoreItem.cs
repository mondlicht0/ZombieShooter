using UnityEngine;

[CreateAssetMenu(fileName = "Weapon Store Item", menuName = "Store Items/Weapon Item")]
public class WeaponStoreItem : StoreItem
{
    public SO_Gun _gun;

    protected override void Purchase()
    {
        if (FindObjectOfType<Player>().GunSelector.GunsSlots.Count < 4)
        {
            FindObjectOfType<Player>().GunSelector.Equip(_gun);
        }
    }
}
