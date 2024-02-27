using System.Collections;
using System.Collections.Generic;
using Unity.Barracuda;
using UnityEngine;

public class WeaponReloadEvents : MonoBehaviour, IWeaponVisitor
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

    public void KnifeAttack()
    {
        _gunSelector.Knife.TryToAttack(_gunSelector.GetComponent<PlayerUI>());
    }


    public void KnifeAttackStart()
    {
        _gunSelector.ActiveGun.Model.SetActive(false);
        _gunSelector.Knife.Model.SetActive(true);
    }

    public void KnifeAttackEnd()
    {
        _gunSelector.Knife.Model.SetActive(false);
        
        if (_gunSelector.ActiveGun != null)
        {
            _gunSelector.ActiveGun.Model.SetActive(true);
        }
    }

    public void Visit(EnemyHitBox enemy)
    {
        enemy.Health.TakeDamage(_gunSelector.Knife.Damage, _gunSelector.Knife.Model.transform.forward.normalized, enemy.Type == EnemyHitBoxType.Head ? 1000 : 1);
    }

    public void Visit(SO_Gun gun)
    {
        throw new System.NotImplementedException();
    }
}
