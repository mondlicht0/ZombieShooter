using UnityEngine;
using UnityEngine.Events;

public class InteractionEvent : MonoBehaviour
{
    [SerializeField] private GameObject _player;



    public UnityEvent OnInteract;
}
