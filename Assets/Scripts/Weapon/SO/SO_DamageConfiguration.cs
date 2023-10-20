using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static UnityEngine.ParticleSystem;

[CreateAssetMenu(fileName = "Damage Config", menuName = "Guns/Damage Config", order = 3)]
public class SO_DamageConfiguration : ScriptableObject
{
    public MinMaxCurve DamageCurve;

    private void Reset()
    {
        DamageCurve.mode = ParticleSystemCurveMode.Curve;
    }

    public int GetDamage(float distance)
    {
        return Mathf.CeilToInt(DamageCurve.Evaluate(distance, Random.value));
    }

    public object Clone()
    {
        SO_DamageConfiguration config = CreateInstance<SO_DamageConfiguration>();

        Utilities.CopyValues(this, config);

        return config;
    }
}
