using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GunInteractable : Interactable
{
    protected override void Interact()
    {
        Destroy(gameObject);
    }
}
