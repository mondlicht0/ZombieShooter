using System.Linq;
using UnityEngine;

[CreateAssetMenu(fileName = "Ammo Config", menuName = "Guns/Ammo Config", order = 3)]
public class SO_AmmoConfiguration : ScriptableObject
{
    public int MaxAmmo = 120;
    public int ClipSize = 30;

    public int CurrentAmmo = 120;
    public int CurrentClip = 30;

    public void Reload()
    {
        int maxReload = Mathf.Min(ClipSize, CurrentAmmo);
        int avaliableBulletsinClip = ClipSize - CurrentClip;
        int reloadAmount = Mathf.Min(maxReload, avaliableBulletsinClip);

        CurrentClip += reloadAmount;
        CurrentAmmo -= reloadAmount;
    }

    public bool CanReload()
    {
        return CurrentClip < ClipSize && CurrentAmmo > 0;
    }

    public object Clone()
    {
        SO_AmmoConfiguration config = CreateInstance<SO_AmmoConfiguration>();

        Utilities.CopyValues(this, config);

        return config;
    }
}