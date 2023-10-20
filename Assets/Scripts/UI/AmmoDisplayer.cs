using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

[DisallowMultipleComponent]
[RequireComponent(typeof(TextMeshProUGUI))]
public class AmmoDisplayer : MonoBehaviour
{
    [SerializeField] private PlayerGunSelector _gunSelector;
    private TextMeshProUGUI _ammoText;

    private void Awake()
    {
        _ammoText = GetComponent<TextMeshProUGUI>();
    }

    private void Start()
    {
        //_gunSelector = FindObjectOfType<PlayerGunSelector>();
    }

    private void Update()
    {
        _ammoText.SetText($"{_gunSelector.ActiveGun.AmmoConfig.CurrentClip} / {_gunSelector.ActiveGun.AmmoConfig.CurrentAmmo}");
    }
}
