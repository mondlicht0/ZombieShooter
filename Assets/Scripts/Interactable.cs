using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Interactable : MonoBehaviour
{
    public string text;

    public void BaseInteract()
    {
        Debug.Log($"Interacted with {text}");
        Interact();
    }

    protected virtual void Interact()
    {

    }
}
