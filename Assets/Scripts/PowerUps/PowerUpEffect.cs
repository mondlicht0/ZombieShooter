using Cysharp.Threading.Tasks;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class PowerUpEffect : ScriptableObject
{
    public float Value;
    public float Duration;

    protected abstract void ApplyLogic(GameObject target);
    protected abstract void RevertLogic(GameObject target);

    public async void Apply(GameObject target)
    {
        ApplyLogic(target);

        float remainingTime = Duration;

        while (remainingTime > 0f)
        {
            await UniTask.Delay(1000);
            remainingTime--;
        }

        RevertLogic(target);
    }
}
