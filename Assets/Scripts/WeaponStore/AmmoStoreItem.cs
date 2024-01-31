using UnityEngine;

[CreateAssetMenu(fileName = "Ammo Store Item", menuName = "Store Items/Ammo Item")]
public class AmmoStoreItem : StoreItem
{
    public SO_Gun Gun;
    public int AmountOfAmmo;

    protected override void Purchase()
    {
        Gun.AmmoConfig.AddAmmo(AmountOfAmmo);
        
    }
}
