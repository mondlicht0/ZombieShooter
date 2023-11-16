using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GunInteractable : Interactable
{
    [SerializeField] private SO_Gun _gun;
    private PlayerGunSelector _gunSelector;

    private void Start()
    {
        _gunSelector = FindAnyObjectByType<PlayerGunSelector>();
    }

    protected override void Interact()
    {
        if (_gunSelector.GunsSlots.Count < 4)
        {
            _gunSelector.Equip(_gun);
            Destroy(gameObject);
        }
    }
}
