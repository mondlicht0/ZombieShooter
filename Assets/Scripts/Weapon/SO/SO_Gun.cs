using System.Collections;
using UnityEngine;
using UnityEngine.Pool;
using Cinemachine;
using Image = UnityEngine.UI.Image;
using Cysharp.Threading.Tasks;

[CreateAssetMenu(fileName = "Gun", menuName = "Guns/Gun", order = 0)]
public class SO_Gun : ScriptableObject, IWeaponVisitor
{
	public string Name;
	public GunType Type;
	public int Price;
	public Sprite Sprite;
	public ImpactType ImpactType;
	public GameObject ModelPrefab;
	public GameObject ModelWithoutHands;
	
	[Header("Particles")]
	public GameObject BloodParticle;
	public GameObject StoneParticle;
	public GameObject MetalParticle;

	public AnimationClip AnimationClip;

	[Header("Spawn")]
	public Vector3 SpawnPoint;
	public Vector3 SpawnRotation;

	public Vector3 PivotPoint;

	[Header("Aim")]
	public Vector3 AimPosition; // -0.01, -0.42, 0.79
	public Vector3 AimRotation; // 0 0 0
	public float AimSpeed;
	public float AimFOV;
	public float BaseFOV;

	[Header("Configs")]
	public SO_ShootConfiguration ShootConfig;
	public SO_TrailConfiguration TrailConfig;
	public SO_AmmoConfiguration AmmoConfig;
	public SO_DamageConfiguration DamageConfig;
	public SO_AudioConfiguration AudioConfig;

	[Header("UI Icons")]
	public Sprite WeaponIcon;
	public Sprite BulletIcon;

	private MonoBehaviour _activeMonoBehaviour;
	private GameObject _model;
	private AudioSource _audioSource;
	private float _lastShootTime;
	private ParticleSystem _shootSystem;
	private ObjectPool<TrailRenderer> _trailPool;
	private ObjectPool<GameObject> _environmentParticlePool;
	private ObjectPool<GameObject> _zombieParticlePool;

	private Recoil _recoil;
	private InputHandler _input;
	private CinemachineVirtualCamera _camera;
	private Transform _mag;
	private Animator _weaponAnim;
	private Rigidbody _weaponRigidbody;

	public GameObject Model { get => _model; }
	public Transform Mag { get => _mag; }
	public Animator WeaponAnim { get => _weaponAnim; }
	public Rigidbody WeaponRigidbody { get => _weaponRigidbody; }

	public void SetEmptyModel()
	{
		Destroy(_model);
	}

	private void OnDisable()
	{
		AmmoConfig.CurrentClip = AmmoConfig.ClipSize;
		AmmoConfig.CurrentAmmo = AmmoConfig.MaxAmmo;
	}
	public void Visit(EnemyHitBox enemy)
	{
		enemy.Health.TakeDamage(DamageConfig.GetDamage(5), _shootSystem.transform.forward.normalized, enemy.Type == EnemyHitBoxType.Head ? 1000 : 1);
	}
	
	public void Visit(GameObject environmentObject)
	{
		if (environmentObject.layer == 20 && environmentObject.TryGetComponent(out Rigidbody rigidbody)) 
		{
			rigidbody.AddForce(_camera.transform.forward * 20f, ForceMode.Impulse);
		}
	}

	public void Spawn(Transform parent, MonoBehaviour activeMonoBehaviour)
	{
		this._activeMonoBehaviour = activeMonoBehaviour;
		_lastShootTime = 0;
		_trailPool = new ObjectPool<TrailRenderer>(CreateTrail);
		_environmentParticlePool = new ObjectPool<GameObject>(CreateParticle);
		_zombieParticlePool = new ObjectPool<GameObject>(CreateParticle);

		/*AmmoConfig.CurrentClip = AmmoConfig.ClipSize;
		AmmoConfig.CurrentAmmo = AmmoConfig.MaxAmmo;*/

		_model = Instantiate(ModelPrefab);
		_model.transform.SetParent(parent, false);
		_model.transform.localPosition = SpawnPoint;
		_model.transform.localRotation = Quaternion.Euler(SpawnRotation);

		_weaponRigidbody = ModelWithoutHands.GetComponent<Rigidbody>();
		_weaponAnim = _model.GetComponent<Animator>();

		_recoil = FindObjectOfType<Recoil>();
		_input = FindObjectOfType<InputHandler>();
		_camera = FindObjectOfType<CinemachineVirtualCamera>();

		_shootSystem = _model.GetComponentInChildren<ParticleSystem>();
		_audioSource = _model.GetComponent<AudioSource>();
	}

	public void TryToShoot(PlayerUI playerUI)
	{
		if (Time.time > ShootConfig.FireRate + _lastShootTime)
		{
			_lastShootTime = Time.time;
			if (AmmoConfig.CurrentClip == 0)
			{
				AudioConfig.PlayOutOfAmmoClip(_audioSource);
				return;
			}
			_shootSystem.Play();
			AudioConfig.PlayShootingClip(_audioSource, AmmoConfig.CurrentClip == 1);

			_weaponAnim.SetTrigger("Shoot");
			AmmoConfig.CurrentClip--;

			for (int i = 0; i < ShootConfig.BulletsPerShot; i++)
			{
				Vector3 spreadAmount = ShootConfig.GetSpread();
				Ray ray = Camera.main.ViewportPointToRay(new Vector3(0.5f, 0.5f, 0) + spreadAmount / 10);
				Vector3 shootDirection = ray.origin - _shootSystem.transform.forward;
				shootDirection.Normalize();

				_recoil.RecoilFire();

				if (Physics.Raycast(ray, out RaycastHit hit, float.MaxValue, ShootConfig.HitMask))
				{
					_activeMonoBehaviour.StartCoroutine(PlayTrail(_shootSystem.transform.position, hit.point, hit));

					if (hit.collider.TryGetComponent(out HitBox hitbox))
					{
						_activeMonoBehaviour.StartCoroutine(PlayParticle(BloodParticle, hit.point + hit.normal * 0.001f, hit));
						
						//CreateParticle(BloodParticle, hit.point, Quaternion.Euler(hit.point - _shootSystem.transform.position));

						hitbox.Accept(this);
						playerUI.CrosshairHit();
					}

					else if (hit.collider.TryGetComponent(out Rigidbody rigidbody) && hit.collider.gameObject.layer == 20)
					{
						Visit(hit.collider.gameObject);
					}
					
					// else if (hit.collider.CompareTag("Stone"))
					// {
					// 	//GameObject particle = CreateParticle(StoneParticle, hit.point + hit.normal * 0.001f, Quaternion.identity);
					// 	//particle.transform.LookAt(hit.point + hit.normal);
						
					// 	_activeMonoBehaviour.StartCoroutine(PlayParticle(StoneParticle, hit.point + hit.normal * 0.001f, hit, false));
					// }
					
					else 
					{
						_activeMonoBehaviour.StartCoroutine(PlayParticle(MetalParticle, hit.point + hit.normal * 0.01f, hit, true));
					}
				}
				else
				{
					_activeMonoBehaviour.StartCoroutine(PlayTrail(_shootSystem.transform.position, (ray.GetPoint(75) - _shootSystem.transform.forward), new RaycastHit()));
				}
			}
		}
	}

	private void CrosshairtCheckEnemy(PlayerUI playerUI)
	{
		Vector3 spreadAmount = ShootConfig.GetSpread();
		Ray ray = Camera.main.ViewportPointToRay(new Vector3(0.5f, 0.5f, 0) + spreadAmount / 10);
		if (Physics.Raycast(ray, out RaycastHit hit, float.MaxValue))
		{
			if (hit.collider.TryGetComponent(out EnemyHitBox hitbox))
			{
				playerUI.ChangeCrosshairColor(Color.red);
			}
			else
			{
				playerUI.ChangeCrosshairColor(Color.white);
			}
		}
	}

	public void StartReloading()
	{
		AudioConfig.PlayReloadClip(_audioSource);
	}

	public bool CanReload()
	{
		return AmmoConfig.CanReload();
	}

	public void EndReload()
	{
		if (Type != GunType.Shotgun)
		{
			AmmoConfig.Reload();
		}

		else
		{
			AmmoConfig.ReloadShotgunSingle();
		}
	}

	public void Tick(bool isAttack, bool isReload, bool isAim, PlayerGunSelector gunSelector, Transform weaponPivot, PlayerUI playerUI)
	{
		if (isAttack)
		{
			gunSelector.ActiveGun.TryToShoot(playerUI);

			/*if (isReload)
			{
				//gunSelector.ActiveGun.Reload(isReload);
			}*/
		}
		CrosshairtCheckEnemy(playerUI);
		gunSelector.ActiveGun.Aim(isAim, weaponPivot, _camera, playerUI.Crosshair);

	}

	public void Aim(bool isAim, Transform weaponPivot, CinemachineVirtualCamera camera, Image crosshair)
	{
		_model.transform.localPosition = Vector3.Lerp(_model.transform.localPosition, isAim ? AimPosition : SpawnPoint, AimSpeed * Time.deltaTime);
		//_model.transform.localRotation = Quaternion.Lerp(_model.transform.localRotation, isAim == true ? Quaternion.Euler(AimRotation) : Quaternion.Euler(SpawnRotation), AimSpeed * Time.deltaTime);
		camera.m_Lens.FieldOfView = Mathf.Lerp(camera.m_Lens.FieldOfView, isAim ? AimFOV : BaseFOV, AimSpeed * AimSpeed * Time.deltaTime);
		crosshair.gameObject.SetActive(!isAim);
	}

	public void Reload(bool isReloading)
	{
		Debug.Log("Reload");
		_weaponAnim.SetTrigger("Reload");
	}

	private IEnumerator PlayTrail(Vector3 startPoint, Vector3 endPoint, RaycastHit hit) 
	{
		TrailRenderer instance = _trailPool.Get();
		instance.gameObject.SetActive(true);
		instance.transform.position = startPoint;

		yield return null;

		instance.emitting = true;

		float distance = Vector3.Distance(startPoint, endPoint);
		float remainingDistance = distance;

		while (remainingDistance > 0)
		{
			instance.transform.position = Vector3.Lerp(startPoint, endPoint, Mathf.Clamp01(1 - (remainingDistance / distance)));
			remainingDistance -= TrailConfig.SimulationSpeed * Time.deltaTime;

			yield return null;
		}

		instance.transform.position = endPoint;

		yield return new WaitForSeconds(TrailConfig.Duration);
		yield return null;
		instance.emitting = false;
		instance.gameObject.SetActive(false);
		_trailPool.Release(instance);
	}

	private TrailRenderer CreateTrail()
	{
		GameObject instance = new GameObject("Bullet Trail");
		TrailRenderer trail = instance.AddComponent<TrailRenderer>();
		trail.colorGradient = TrailConfig.Color;
		trail.material = TrailConfig.Material;
		trail.widthCurve = TrailConfig.WidthCurve;
		trail.time = TrailConfig.Duration;
		trail.minVertexDistance = TrailConfig.MinVertexDistance;

		trail.emitting = false;
		trail.shadowCastingMode = UnityEngine.Rendering.ShadowCastingMode.Off;

		return trail;
	}
	
	private IEnumerator PlayParticle(GameObject particlePrefab, Vector3 position, RaycastHit hit, bool isMetal) 
	{
		GameObject instance = _environmentParticlePool.Get();
		GameObject particle;
		
		if (instance.transform.GetComponentInChildren<ParticleSystem>(true) == null) 
		{
			particle = Instantiate(particlePrefab);
			particle.transform.SetParent(instance.transform);
		}
		
		particle = instance.transform.GetComponentInChildren<ParticleSystem>(true).gameObject;
		
		instance.SetActive(true);
		particle.SetActive(true);
		
		instance.transform.position = position;
		particle.transform.position = position;
		
		instance.transform.LookAt(hit.point - hit.normal);
		particle.transform.LookAt(hit.point - hit.normal);
		
		yield return new WaitForSeconds(1f);
		
		instance.SetActive(false);
		particle.SetActive(false);
		_environmentParticlePool.Release(instance);
	}
	
		private IEnumerator PlayParticle(GameObject particlePrefab, Vector3 position, RaycastHit hit) 
	{
		GameObject instance = _zombieParticlePool.Get();
		GameObject particle;
		
		if (instance.transform.GetComponentInChildren<ParticleSystem>(true) == null) 
		{
			particle = Instantiate(particlePrefab);
			particle.transform.SetParent(instance.transform);
		}
		
		particle = instance.transform.GetComponentInChildren<ParticleSystem>(true).gameObject;
		
		instance.SetActive(true);
		particle.SetActive(true);
		
		instance.transform.position = position;
		particle.transform.position = position;
		
		instance.transform.LookAt(hit.point - hit.normal);
		particle.transform.LookAt(hit.point - hit.normal);
		
		yield return new WaitForSeconds(1f);
		
		instance.SetActive(false);
		particle.SetActive(false);
		_zombieParticlePool.Release(instance);
	}

	private GameObject CreateParticle()
	{
		GameObject bloodParticle = new GameObject("Particle");
		return bloodParticle;
	}

	public void Despawn()
	{
		// We do a bunch of other stuff on the same frame, so we really want it to be immediately destroyed, not at Unity's convenience.
		//Model.SetActive(false);
		Destroy(Model);
		_trailPool.Clear();
		_environmentParticlePool.Clear();
		_zombieParticlePool.Clear();
		//if (BulletPool != null)
		//{
			//BulletPool.Clear();
		//}

		//ShootingAudioSource = null;
		//ShootSystem = null;
	}

	public object Clone()
	{
		SO_Gun config = CreateInstance<SO_Gun>();

		config.ImpactType = ImpactType;
		config.Type = Type;
		config.Name = Name;
		config.name = name;
		config.DamageConfig = DamageConfig.Clone() as SO_DamageConfiguration;
		config.ShootConfig = ShootConfig.Clone() as SO_ShootConfiguration;
		config.AmmoConfig = AmmoConfig.Clone() as SO_AmmoConfiguration;
		config.TrailConfig = TrailConfig.Clone() as SO_TrailConfiguration;
		//config.AudioConfig = AudioConfig.Clone() as AudioConfigScriptableObject;
		//config.BulletPenConfig = BulletPenConfig.Clone() as BulletPenetrationConfigScriptableObject;

		config.ModelPrefab = ModelPrefab;
		config.SpawnPoint = SpawnPoint;
		config.SpawnRotation = SpawnRotation;

		return config;
	}

	public void Visit(SO_Gun gun)
	{
		throw new System.NotImplementedException();
	}
}
