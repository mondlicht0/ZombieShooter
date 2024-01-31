using UnityEngine;

public abstract class Dropable : MonoBehaviour
{
    protected BoxCollider Collider;
    protected Rigidbody Rigidbody;
    private CharacterController characterController;

    private void Awake()
    {
        Collider = GetComponent<BoxCollider>();
        Rigidbody = GetComponent<Rigidbody>();
    }

    private void OnTriggerEnter(Collider collision)
    {
        if (collision.gameObject.layer == 9)
        {
            Pick(collision.GetComponent<Player>());
            Destroy(gameObject);
        }

    }

    protected abstract void Pick(Player player);

}
