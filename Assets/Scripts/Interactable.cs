using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Interactable : MonoBehaviour
{
    public string Text;
    public bool IsInteractable = true;

    public void BaseInteract()
    {
        Debug.Log($"Interacted with {Text}");
        Interact();
    }

    protected virtual void Interact()
    {

    }

    protected virtual void Interact(PlayerData playerData)
    {

    }
}
