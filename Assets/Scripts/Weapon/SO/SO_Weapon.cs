using UnityEngine;


public class SO_Weapon 
{
    public string Name;
    public float FireRate;
    public GameObject Prefab;
    public float Bloom;
    public float Recoil;
    public float Damage;
    public float AimSpeed;
    public float Kickback;
    public AudioClip[] FireClip;
    public bool Automatic;

    [Header("Hipfire Recoil")]
    public float RecoilX;
    public float RecoilY;
    public float RecoilZ;

    [Header("ADS")]
    public float AimRecoilX;
    public float AimRecoilY;
    public float AimRecoilZ;

    [Header("Settings")]
    public float Snapinness;
    public float ReturnSpeed;
}

