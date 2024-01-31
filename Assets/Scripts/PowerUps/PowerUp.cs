using UnityEngine;

public class PowerUp : MonoBehaviour
{
    [SerializeField] private PowerUpEffect _powerUpEffect;

    private void OnTriggerEnter(Collider other)
    {
        Destroy(gameObject);
        _powerUpEffect.Apply(other.gameObject);
    }
}
