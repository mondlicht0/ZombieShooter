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
        _gunSelector.Knife.SetActive(true);
    }

    public void KnifeAttackEnd()
    {
        _gunSelector.Knife.SetActive(false);
        _gunSelector.ActiveGun.Model.SetActive(true);
    }
}