using Cinemachine;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Animations.Rigging;
using UnityEngine.InputSystem;

[DisallowMultipleComponent]
public class PlayerGunSelector : MonoBehaviour
{
	public CinemachineVirtualCamera Camera;
	public AnimationClip Clip;

	[SerializeField] private GunType Gun;
	[SerializeField] private Transform GunParent;
	[SerializeField] private List<SO_Gun> Guns;
	[SerializeField] private List<SO_Gun> _gunsSlots = new List<SO_Gun>(4);
	[field: SerializeField] public SO_Knife Knife { get; private set; }
	[SerializeField] private Animator _animator;
	[SerializeField] private Animator _rigController;
	[SerializeField] private PlayerAction _action;
	//[SerializeField] private GameObject _knife;
	
	[SerializeField] private GameObject _magazine;
	private Player _player;

	[Space]
	[Header("Runtime Filled")]
	public SO_Gun ActiveGun;

	[SerializeField] private SO_Gun ActiveBaseGun;

	

	public SO_Gun GunActive { get => ActiveGun; }
	public List<SO_Gun> GunsSlots { get => _gunsSlots; }

	public void InitStart()
	{
		SO_Gun gun = Guns.Find(gun => gun.Type == Gun);

		if (gun == null)
		{
			Debug.LogError($"No GunScriptableObject found for GunType: {gun}");
			return;
		}
		SetupGun(gun);
	}


	private void Awake()
	{
		_player = GetComponent<Player>();
	}

	private void Start()
	{
		SO_Gun gun = Guns?.Find(gun => gun.Type == Gun);

		if (gun == null)
		{
			//Debug.LogError($"No GunScriptableObject found for GunType: {gun}");
			return;
		}
		SetupGun(gun);
		Knife.Spawn(GunParent, this);
	}

	private void Update()
	{
		//GunParent.transform.localPosition = ActiveGun.PivotPoint;

		if (Keyboard.current.digit1Key.wasPressedThisFrame)
		{
			SelectWeapon(_gunsSlots[0]);
			//Equip(Guns[0]);
			//Equip(_gunsSlots?[0]);
			//_action.IsReloading = false;
		}

		if (Keyboard.current.digit2Key.wasPressedThisFrame)
		{
			SelectWeapon(_gunsSlots[1]);
			//Equip(_gunsSlots?[1]);
			//_action.IsReloading = false;
		}

		if (Keyboard.current.digit3Key.wasPressedThisFrame)
		{
			SelectWeapon(_gunsSlots?[2]);
			//Equip(_gunsSlots?[2]);
			//_action.IsReloading = false;
		}

		if (Keyboard.current.digit4Key.wasPressedThisFrame)
		{
			SelectWeapon(_gunsSlots?[3]);
			//Equip(_gunsSlots?[3]);
			//_action.IsReloading = false;
		}

		/*if (Keyboard.current.digit5Key.wasPressedThisFrame)
		{
			SelectWeapon(Knife);
		}*/

		if (Keyboard.current.vKey.wasPressedThisFrame)
		{
			_action.KnifeAttack();
		}
	}

	public void Drop()
	{
		// logic of weapon slots
	
		if (ActiveGun != null)
		{
			GameObject droppedGun = Instantiate(ActiveGun.ModelWithoutHands, transform.position, Quaternion.identity);
			Rigidbody gunRb = droppedGun.GetComponent<Rigidbody>();
			_gunsSlots.Remove(ActiveGun);
			DespawnActiveGun();
			gunRb.isKinematic = false;
			//gunRb.velocity = _player.PlayerVelocity;

			gunRb.AddForce(_player.Camera.transform.forward * 2, ForceMode.Impulse);
			gunRb.AddForce(_player.Camera.transform.up * 2, ForceMode.Impulse);

			float random = Random.Range(-1f, 1f);
			gunRb.AddTorque(new Vector3(random, random, random) * 10);

			//ActiveGun.SetEmptyModel();
			//ActiveGun = null;
		}
			
	}

	public void Equip(SO_Gun Gun)
	{
		ActiveGun?.Despawn();
		SetupGun(Gun);
		_gunsSlots.Add(Gun);
		_action.IsReloading = false;
	}

	public void SelectWeapon(SO_Gun Gun)
	{
		if (Gun != null)
		{
			DespawnActiveGun();
			SetupGun(Gun);
		}
	}

	private void SetupGun(SO_Gun Gun)
	{
		ActiveBaseGun = Gun;
		ActiveGun = Gun.Clone() as SO_Gun;
		ActiveGun = Gun;
		ActiveGun.Spawn(GunParent, this);

		GunDisplayer.Instance.ChangeWeaponIcons(Gun.WeaponIcon, Gun.BulletIcon);
	}

	public void DespawnActiveGun()
	{
		ActiveGun.Despawn();
		ActiveGun = null;
		//Destroy(ActiveGun);
	}

	/*    private void SetAnimationDelayed()
		{
			Debug.Log(ActiveGun.Name);
			_overrides[Clip] = ActiveGun.AnimationClip;
		}*/

	/*    private void OnDisable()
		{
			_overrides[Clip] = null;
		}*/

	/*    private void SetupGun(SO_Gun Gun)
		{
			ActiveBaseGun = Gun;
			ActiveGun = Gun.Clone() as SO_Gun;
			ActiveGun.Spawn(GunParent, this, Camera);

			InverseKinematics.SetGunStyle(ActiveGun.Type == GunType.Glock);
			InverseKinematics.Setup(GunParent);
		}

		public void DespawnActiveGun()
		{
			ActiveGun.Despawn();
			Destroy(ActiveGun);
		}

		public void PickupGun(SO_Gun Gun)
		{
			DespawnActiveGun();
			SetupGun(Gun);
		}

		public void ApplyModifiers(IModifier[] Modifiers)
		{
			DespawnActiveGun();
			SetupGun(ActiveBaseGun);

			foreach (IModifier modifier in Modifiers)
			{
				modifier.Apply(ActiveGun);
			}
		}*/
}
