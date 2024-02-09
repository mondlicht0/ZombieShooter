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

    private void Update()
    {
        if (GunSelector.ActiveGun != null)
        {
            GunSelector.ActiveGun.Tick(Input.IsAttack && GunSelector.ActiveGun != null, IsReloading, Input.IsAim, GunSelector, WeaponPivot, _UI);

            if (ShouldAutoReload() || ShouldManualReload())
            {
                GunSelector.ActiveGun.StartReloading();
                IsReloading = true;
                GunSelector.ActiveGun.WeaponAnim.Play("Reload");
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
}
