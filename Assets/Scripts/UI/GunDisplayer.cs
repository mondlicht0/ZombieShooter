using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class GunDisplayer : MonoBehaviour
{
    public static GunDisplayer Instance;

    [SerializeField] private PlayerGunSelector _gunSelector;
    [SerializeField] private TextMeshProUGUI _ammoText;
    [SerializeField] private Image _weaponIcon;
    [SerializeField] private Image _bulletIcon;

    private void Awake()
    {
        Instance = this;
    }

    private void Start()
    {
        _gunSelector = FindObjectOfType<PlayerGunSelector>();
    }

    private void Update()
    {
        _ammoText.SetText($"{_gunSelector.ActiveGun?.AmmoConfig.CurrentClip} / {_gunSelector.ActiveGun?.AmmoConfig.CurrentAmmo}");
    }

    public void ChangeWeaponIcons(Sprite weaponIcon, Sprite bulletIcon)
    {
        _weaponIcon.sprite = weaponIcon;
        _bulletIcon.sprite = bulletIcon;
    }
}
