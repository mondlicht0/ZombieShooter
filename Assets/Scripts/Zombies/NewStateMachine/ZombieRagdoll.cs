using System.Collections;
using UnityEngine;

public class ZombieRagdoll : MonoBehaviour
{
    [SerializeField] private Zombie _zombie;
    [SerializeField] private Health _health;

    [SerializeField] private Transform _ragdollRoot;
    [SerializeField] private float _fadeOutDelay;

    private bool _startRagdoll;
    private CharacterJoint[] _joints;
    private Rigidbody[] _rigidBodies;

    private void Awake()
    {
        _zombie = GetComponent<Zombie>();
        _rigidBodies = _ragdollRoot.GetComponentsInChildren<Rigidbody>();
        _joints = _ragdollRoot.GetComponentsInChildren<CharacterJoint>();
    }

    private void Start()
    {
        if (_startRagdoll)
        {
            ActivateRagdol();
        }

        else
        {
            EnableAnimator();
        }
    }

    public void DeActivateRagdol()
    {
        _zombie.Animator.enabled = true;
        foreach (Rigidbody rb in _rigidBodies)
        {
            rb.isKinematic = true;
        }
    }

    public void ActivateRagdol()
    {
        _zombie.Animator.enabled = false;
        _zombie.Agent.enabled = false;

        foreach (CharacterJoint joint in _joints)
        {
            joint.enableCollision = true;
        }

        foreach (Rigidbody rb in _rigidBodies)
        {
            rb.detectCollisions = true;
            rb.useGravity = true;
            rb.isKinematic = false;

            rb.gameObject.layer = 0;
        }

        ApplyForce(_zombie.transform.forward);
    }

    public void ApplyForce(Vector3 force)
    {
        //var rb = anim.GetBoneTransform(HumanBodyBones.Hips).GetComponent<Rigidbody>();
        var rb = GetComponent<Rigidbody>();
        rb.AddForce(force, ForceMode.VelocityChange);
    }

    public void DisableAllRigidbodies()
    {
        foreach (Rigidbody rigidbody in _rigidBodies)
        {
            rigidbody.detectCollisions = false;
            rigidbody.useGravity = false;
            rigidbody.isKinematic = true;
        }
    }

    public IEnumerator FadeOut()
    {
        yield return new WaitForSeconds(_fadeOutDelay);

        DisableAllRigidbodies();

        float time = 0;
        while (time < 1)
        {
            transform.position += Vector3.down * Time.deltaTime;
            time += Time.deltaTime;
            yield return null;
        }

        _health.isDead = true;
        Destroy(gameObject);
    }

    public void EnableAnimator()
    {
        _zombie.Animator.enabled = true;
        _zombie.Agent.enabled = true;

        foreach (CharacterJoint joint in _joints)
        {
            joint.enableCollision = false;
        }

        foreach (Rigidbody rb in _rigidBodies)
        {
            rb.useGravity = false;
            rb.isKinematic = true;
        }
    }
}
