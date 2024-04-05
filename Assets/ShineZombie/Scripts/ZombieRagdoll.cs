using System.Collections;
using UnityEngine;

public class ZombieRagdoll : MonoBehaviour
{
	private Zombie _zombie;
	private Health _health;

	[SerializeField] private Transform _ragdollRoot;

	private bool _startRagdoll;
	private CharacterJoint[] _joints;
	private Rigidbody[] _rigidBodies;

	private void Awake()
	{
		_zombie = GetComponent<Zombie>();
		_health = GetComponent<Health>();
		_rigidBodies = _ragdollRoot.GetComponentsInChildren<Rigidbody>();
		_joints = _ragdollRoot.GetComponentsInChildren<CharacterJoint>();
	}
	
	void OnEnable()
	{
		_health.OnDied += ActivateRagdoll;
	}
	
	void OnDisable()
	{
		_health.OnDied -= ActivateRagdoll;
	}

	private void Start()
	{
		if (_startRagdoll)
		{
			ActivateRagdoll();
		}

		else
		{
			EnableAnimator();
		}
	}

	public void DeActivateRagdoll()
	{
		_zombie.Animator.enabled = true;
		foreach (Rigidbody rb in _rigidBodies)
		{
			rb.isKinematic = true;
		}
	}

	public void ActivateRagdoll()
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