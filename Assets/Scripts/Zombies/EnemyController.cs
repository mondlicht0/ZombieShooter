using UnityEngine;
using System.Collections;

public class EnemyController : MonoBehaviour {
	UnityEngine.AI.NavMeshAgent nav; 
	Transform player;
	Animator controller;
	Animator anim;     
	float health;
	bool isDead = false;
	int damage = 20;
	int pointValue = 10;
	bool behindWall;

	public float timeToAttack;
	float attackTimer;
	public AudioClip zombieAttackClip;

	CapsuleCollider capsuleCollider;
	// Use this for initialization
	void Awake () {
		nav = GetComponent <UnityEngine.AI.NavMeshAgent> ();
		player = GameObject.FindGameObjectWithTag ("Player").transform;
		controller = GetComponentInParent<Animator> ();
		capsuleCollider = GetComponent <CapsuleCollider> ();
		anim = GetComponent <Animator> ();
	}
	
	// Update is called once per frame
	void Update () {
		if(!isDead)
		{
			nav.SetDestination (player.position);
			controller.SetFloat ("speed", Mathf.Abs(nav.velocity.x) + Mathf.Abs (nav.velocity.z));
			float distance = Vector3.Distance(transform.position, player.position);
			if(distance < 3  || behindWall)
			{
				attackTimer += Time.deltaTime;
			}
			else if (attackTimer > 0)
			{
				attackTimer -= Time.deltaTime*2;
			}
			else
				attackTimer = 0;


		}
	}

	bool Attack()
	{
		if(attackTimer > timeToAttack)
		{
			//sound
			attackTimer = 0;
			return true;
		}
		return false;
	}


	void Death ()
	{
		// The enemy is dead.
		isDead = true;
		nav.Stop ();
		// Turn the collider into a trigger so shots can pass through it.
		capsuleCollider.isTrigger = true;
		capsuleCollider.enabled = false;
		
		// Tell the animator that the enemy is dead.
		//anim.SetTrigger ("Dead");
		anim.SetTrigger ("Dead");
		// Change the audio clip of the audio source to the death clip and play it (this will stop the hurt clip playing).
		//enemyAudio.clip = deathClip;
		//enemyAudio.Play ();
		
		Destroy (gameObject, 4f);
	}


	void ApplyDamage(float damage)
	{
		//print (damage);
		health -= damage;

		if (!isDead)
		{
			if (health <= 0)
			{
				Death ();
				//whichPlayer.GetComponent<PlayerDamageNew>().scoreManager.AddScore(10);
			}
		}


	}
	void OnCollisionStay(Collision collisionInfo)
	{
		if(collisionInfo.gameObject.tag == "Player")
		{
			if(Attack ())
				collisionInfo.collider.SendMessageUpwards("PlayerDamage", damage, SendMessageOptions.RequireReceiver);
		}


	}

	void OnTriggerStay(Collider collide)
	{
		//print ("collision");
		if(collide.gameObject.tag == "SpawnWall")
		{
			if(collide.gameObject.GetComponent<Renderer>().enabled)
			{
				behindWall = true;
				nav.Stop ();
				if(Attack ())
				{
					collide.GetComponent<Collider>().SendMessageUpwards("RemoveBoard", SendMessageOptions.RequireReceiver);
				}
				
			}
			else
			{
				nav.Resume();
				behindWall = false;
			}
		}
	}

}










