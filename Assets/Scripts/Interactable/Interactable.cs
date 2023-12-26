using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Interactable : MonoBehaviour
{
    public bool UseEvents;
    public string Text;
    public bool IsInteractable = true;

    public void BaseInteract()
    {
        Debug.Log($"Interacted with {Text}");

        if (UseEvents)
        {
            GetComponent<InteractionEvent>().OnInteract.Invoke();
        }

        Interact();
    }

    protected virtual void Interact()
    {

    }
}
