using UnityEngine;

public class ZombieAnimationEvents : MonoBehaviour
{
    [SerializeField] private SphereCollider _leftArmCollider;
    [SerializeField] private SphereCollider _rightArmCollider;

    public void EnableColliders()
    {
        _leftArmCollider.enabled = true;
        _rightArmCollider.enabled = true;
    }

    public void DisableColliders()
    {
        _leftArmCollider.enabled = false;
        _rightArmCollider.enabled = false;
    }
}
