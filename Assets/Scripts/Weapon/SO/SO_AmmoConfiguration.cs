using System.Collections;
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
        CurrentClip += GetReloadAmount();
        CurrentAmmo -= GetReloadAmount();
    }

    public IEnumerator ReloadShotgun(Animator animator)
    {
        for (int i = 0; i < GetReloadAmount(); i++)
        {
            animator.Play("Reload_Shell");

            while (animator.GetCurrentAnimatorStateInfo(0).normalizedTime < 1f)
            {
                yield return null;
            }

            AnimatorStateInfo stateInfo = animator.GetCurrentAnimatorStateInfo(0);

            if (stateInfo.normalizedTime >= 1f)
            {
                ReloadShotgunSingle();
            }

        }

    }

    public int GetReloadAmount()
    {
        int maxReload = Mathf.Min(ClipSize, CurrentAmmo);
        int avaliableBulletsinClip = ClipSize - CurrentClip;

        return avaliableBulletsinClip;
    }

    public void ReloadShotgunSingle()
    {
        int maxReload = Mathf.Min(ClipSize, CurrentAmmo);
        int avaliableBulletsinClip = ClipSize - CurrentClip;
        int reloadAmount = Mathf.Min(maxReload, avaliableBulletsinClip);

        CurrentClip++;
        CurrentAmmo--;
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

    public void AddAmmo(int amount)
    {
        CurrentAmmo += amount;
    }
}
