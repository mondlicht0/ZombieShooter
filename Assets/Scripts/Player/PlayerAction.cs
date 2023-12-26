using Cinemachine;
using System.Security.Claims;
using UnityEngine;
using UnityEngine.Animations;
using UnityEngine.Animations.Rigging;
using UnityEngine.InputSystem;
using UnityEngine.UI;

[DisallowMultipleComponent]
public class PlayerAction : MonoBehaviour
{
    // public for editor
    public PlayerGunSelector GunSelector;
    public InputHandler Input;
    public Transform WeaponPivot;

    [SerializeField] private CinemachineVirtualCamera _camera;
    [SerializeField] private PlayerUI _UI;
    [SerializeField] private Animator _animator;
    [SerializeField] private Animator _rigController;
    [SerializeField] private MultiParentConstraint _parentConstraint;
    [SerializeField] private TwoBoneIKConstraint _magConstraint;
    [SerializeField] private bool AutoReload = false;

    private Vector3 _origPos;
    public bool IsReloading;

    public void InitAwake()
    {
        _parentConstraint.data.constrainedObject = GunSelector.ActiveGun.Mag;
        _magConstraint.data.target = GunSelector.ActiveGun.Mag;
    }

    public void InitStart()
    {
        _origPos = WeaponPivot.transform.position;
    }

    private void Awake()
    {

    }

    private void Start()
    {
        
        //_origPos = WeaponPivot.transform.position;
    }

    private void Update()
    {
        if (GunSelector.ActiveGun != null)
        {
            GunSelector.ActiveGun.Tick(Input.IsAttack && GunSelector.ActiveGun != null, IsReloading, Input.IsAim, GunSelector, WeaponPivot, _UI);

            if (ShouldAutoReload() || ShouldManualReload())
            {
                GunSelector.ActiveGun.StartReloading();
                IsReloading = true;
                GunSelector.ActiveGun.WeaponAnim.SetTrigger("Reload");
            }
        }
            
    }



    public void KnifeAttack()
    {
        GunSelector.ActiveGun.WeaponAnim.Play("Untake");
    }

    public void KnifeAttackStart()
    {
        GunSelector.ActiveGun.Model.SetActive(false);
        GunSelector.Knife.SetActive(true);
    }

    public void KnifeAttackEnd()
    {
        GunSelector.Knife.SetActive(false);
        GunSelector.ActiveGun.Model.SetActive(true);
    }


    public void EndReload()
    {
        GunSelector.ActiveGun.EndReload();
        IsReloading = false;
    }

    private bool ShouldManualReload()
    {
        return !IsReloading && Input.IsReload && GunSelector.ActiveGun.CanReload();
    }

    private bool ShouldAutoReload()
    {
        return !IsReloading && AutoReload && GunSelector.ActiveGun.AmmoConfig.CurrentClip == 0 && GunSelector.ActiveGun.CanReload();
    }

    private void FixedUpdate()
    {
        
    }

    /*    private void UpdateCrosshair()
        {
            Vector3 gunTipPoint = GunSelector.ActiveGun.GetRaycastOrigin();
            Vector3 forward;
            if (GunSelector.ActiveGun.ShootConfig.ShootType == ShootType.FromGun)
            {
                forward = GunSelector.ActiveGun.GetGunForward();
            }
            else
            {
                forward = GunSelector.Camera.transform.forward;
            }

            Vector3 hitPoint = gunTipPoint + forward * 10;
            if (Physics.Raycast(gunTipPoint, forward, out RaycastHit hit, float.MaxValue, GunSelector.ActiveGun.ShootConfig.HitMask))
            {
                hitPoint = hit.point;
            }

            AimTarget.transform.position = hitPoint;

            if (GunSelector.ActiveGun.ShootConfig.ShootType == ShootType.FromGun)
            {
                Vector3 screenSpaceLocation = GunSelector.Camera.WorldToScreenPoint(hitPoint);

                if (RectTransformUtility.ScreenPointToLocalPointInRectangle(
                    (RectTransform)Crosshair.transform.parent,
                    screenSpaceLocation,
                    null,
                    out Vector2 localPosition))
                {
                    Crosshair.rectTransform.anchoredPosition = localPosition;
                }
                else
                {
                    Crosshair.rectTransform.anchoredPosition = Vector2.zero;
                }
            }
            else
            {
                Crosshair.rectTransform.anchoredPosition = Vector2.zero;
            }
        }

        private bool ShouldManualReload()
        {
            return !IsReloading
                && Keyboard.current.rKey.wasReleasedThisFrame
                && GunSelector.ActiveGun.CanReload();
        }

        private bool ShouldAutoReload()
        {
            return !IsReloading
                && AutoReload
                && GunSelector.ActiveGun.AmmoConfig.CurrentClipAmmo == 0
                && GunSelector.ActiveGun.CanReload();
        }

        private void EndReload()
        {
            GunSelector.ActiveGun.EndReload();
            InverseKinematics.HandIKAmount = 1f;
            InverseKinematics.ElbowIKAmount = 1f;
            IsReloading = false;
        }*/
}
