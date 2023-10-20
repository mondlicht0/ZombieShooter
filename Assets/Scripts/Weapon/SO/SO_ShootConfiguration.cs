using UnityEngine;
using System.Linq;

[CreateAssetMenu(fileName = "Shoot Config", menuName = "Guns/Shoot Configuration", order = 2)]
public class SO_ShootConfiguration : ScriptableObject, System.ICloneable
{
    public LayerMask HitMask;
    public Vector3 Spread = new Vector3(0.1f, 0.1f, 0.1f);
    public float FireRate = 0.25f;

    public Vector3 GetSpread(float shootTime = 0)
    {
        Vector3 shootDirection = new Vector3(Random.Range(-Spread.x, Spread.x),
                                             Random.Range(-Spread.y, Spread.y),
                                             Random.Range(-Spread.z, Spread.z));

        return shootDirection;
    }

    public object Clone()
    {
        SO_ShootConfiguration config = CreateInstance<SO_ShootConfiguration>();

        Utilities.CopyValues(this, config);

        return config;
    }
}
