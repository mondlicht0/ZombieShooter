using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WeaponReloadEvents : MonoBehaviour
{
    [SerializeField] private PlayerGunSelector _gunSelector;
    [SerializeField] private PlayerAction _action;

    [SerializeField] private GameObject _magazine;
    [SerializeField] private Transform _hand;

    [SerializeField] private Animator _animator;

    public bool IsShotgun;

    private void Start()
    {
        _action = FindObjectOfType<PlayerAction>();
        _gunSelector = FindObjectOfType<PlayerGunSelector>();
    }

    private void OnEnable()
    {
        _animator.Play("Take");
    }

    private void EndReload()
    {
        _action.EndReload();
    }

    private void EndReloadShotgun()
    {

    }

    private void ShouldReloadYet()
    {
        if (_gunSelector.ActiveGun.AmmoConfig.ClipSize - _gunSelector.ActiveGun.AmmoConfig.CurrentClip > 0)
        {
            _animator.SetBool("ShouldReloadYet", true);
        }

        else
        {
            _animator.SetBool("ShouldReloadYet", false);
        }
    }

    private void EndReloadYet()
    {
        _animator.Play("Reload_End");
    }

    public void AttachMagazine()
    {
        _magazine.transform.SetParent(_hand);
    }

    public void InitMag()
    {
        //_magazine = _gunSelector.ActiveGun.Mag.gameObject;
    }

    public void KnifeAttackStart()
    {
        _gunSelector.ActiveGun.Model.SetActive(false);
        _gunSelector.Knife.Model.SetActive(true);
    }

    public void KnifeAttackEnd()
    {
        _gunSelector.Knife.Model.SetActive(false);
        _gunSelector.ActiveGun.Model.SetActive(true);
    }
}
