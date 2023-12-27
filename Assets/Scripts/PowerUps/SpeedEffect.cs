using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Speed Effect", menuName = "Power Up/Speed Effect")]
public class SpeedEffect : PowerUpEffect
{
    protected override void ApplyLogic(GameObject target)
    {
        target.GetComponent<Player>().PlayerData.WalkSpeed += Value;
    }

    protected override void RevertLogic(GameObject target)
    {
        target.GetComponent<Player>().PlayerData.WalkSpeed -= Value;
    }
}
