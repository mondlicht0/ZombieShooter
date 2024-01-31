using UnityEngine;

public class MedKit : Dropable
{
    private int _amountOfHealth;

    private void OnEnable()
    {
        _amountOfHealth = Random.Range(11, 15);
    }

    protected override void Pick(Player player)
    {
        if (!player.Health.IsHealthFull())
        {
            player.Health.AddHealth(_amountOfHealth);
        }
    }
}
