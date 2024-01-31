using UnityEngine;

public class AmmoBox : Dropable
{
    private int _amountOfAmmo;

    private void OnEnable()
    {
        _amountOfAmmo = Random.Range(9, 13);
    }


    protected override void Pick(Player player)
    {
        if (player.GunSelector.ActiveGun != null)
        {
            Debug.Log("JJ");
            player.GunSelector.ActiveGun.AmmoConfig.AddAmmo(_amountOfAmmo);
        }
    }
}
