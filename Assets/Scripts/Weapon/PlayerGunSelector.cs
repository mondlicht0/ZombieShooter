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
    [SerializeField] private Animator _animator;
    [SerializeField] private Animator _rigController;
    [SerializeField] private PlayerAction _action;
    [SerializeField] private GameObject _knife;

    [SerializeField] private GameObject _magazine;

    private PlayerIK _playerIK;

    [Space]
    [Header("Runtime Filled")]
    public SO_Gun ActiveGun;

    [SerializeField] private SO_Gun ActiveBaseGun;

    public GameObject Knife { get { return _knife; } }

    public SO_Gun GunActive { get { return ActiveBaseGun; } }

    public void InitAwake()
    {
        _playerIK = GetComponent<PlayerIK>();
    }

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
        _playerIK = GetComponent<PlayerIK>();
    }

    private void Start()
    {
        SO_Gun gun = Guns.Find(gun => gun.Type == Gun);

        //if (gun == null)
        //{
            //Debug.LogError($"No GunScriptableObject found for GunType: {gun}");
            //return;
        //}
        SetupGun(gun);
    }

    private void Update()
    {
        //GunParent.transform.localPosition = ActiveGun.PivotPoint;

        if (Keyboard.current.digit1Key.wasPressedThisFrame)
        {
            Equip(Guns[0]);
            _action.IsReloading = false;
        }

        if (Keyboard.current.digit2Key.wasPressedThisFrame)
        {
            Equip(Guns[1]);
            _action.IsReloading = false;
        }

        if (Keyboard.current.vKey.wasPressedThisFrame)
        {
            _action.KnifeAttack();
        }
    }

    public void Equip(SO_Gun Gun)
    {
        ActiveGun?.Despawn();
        SetupGun(Gun);
    }

    private void SetupGun(SO_Gun Gun)
    {
        ActiveBaseGun = Gun;
        ActiveGun = Gun.Clone() as SO_Gun;
        ActiveGun = Gun;
        ActiveGun.Spawn(GunParent, this);

/*        _animator.SetBool("Is1Handed", ActiveGun.Type == GunType.Glock);
        _animator.SetBool("Is2Handed", ActiveGun.Type == GunType.MCX);


        _playerIK.Setup(GunParent);

        _rigController.Play("equip_" + Gun.Name);*/

    }

    public void DespawnActiveGun()
    {
        ActiveGun.Despawn();
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
