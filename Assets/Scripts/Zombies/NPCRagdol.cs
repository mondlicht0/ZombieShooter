using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class NPCRagdol : MonoBehaviour
{
    [SerializeField] private Transform _ragdollRoot;
    [SerializeField] private float _fadeOutDelay;

    private bool _startRagdoll;
    private CharacterJoint[] _joints;
    private Rigidbody[] rigidBody;
    [SerializeField] private Animator anim;
    private NavMeshAgent _agent;
    [SerializeField] private NPCAgent _npcAgent;

    private void Awake()
    {
        rigidBody = _ragdollRoot.GetComponentsInChildren<Rigidbody>();
        _joints = _ragdollRoot.GetComponentsInChildren<CharacterJoint>();
        //anim = GetComponent<Animator>();
        _agent = GetComponent<NavMeshAgent>();
    }

    void Start()
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

    void Update()
    {

    }

    public void DeActivateRagdol()
    {
        anim.enabled = true;
        foreach (Rigidbody rb in rigidBody)
        {
            rb.isKinematic = true;
        }
    }

    public void ActivateRagdol()
    {
        anim.enabled = false;
        _agent.enabled = false;

        foreach (CharacterJoint joint in _joints)
        {
            joint.enableCollision = true;
        }

        foreach (Rigidbody rb in rigidBody)
        {
            //rb.velocity = Vector3.zero;
            rb.detectCollisions = true;
            rb.useGravity = true;
            rb.isKinematic = false;
        }

        ApplyForce(_npcAgent.playerTransform.forward);
    }

    public void ApplyForce(Vector3 force)
    {
        //var rb = anim.GetBoneTransform(HumanBodyBones.Hips).GetComponent<Rigidbody>();
        var rb = GetComponent<Rigidbody>();
        rb.AddForce(force, ForceMode.VelocityChange);
    }

    public void DisableAllRigidbodies()
    {
        foreach (Rigidbody rigidbody in rigidBody)
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

        _npcAgent.isDead = true;
        Destroy(gameObject);
        //gameObject.SetActive(false);
    }

    public void EnableAnimator()
    {
        anim.enabled = true;
        _agent.enabled = true;

        foreach (CharacterJoint joint in _joints)
        {
            joint.enableCollision = false;
        }

        foreach (Rigidbody rb in rigidBody)
        {
            rb.useGravity = false;
            rb.isKinematic = true;
        }
    }
}
