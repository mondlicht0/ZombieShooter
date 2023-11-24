using System;
using TMPro;
using UnityEngine;

public class PlayerHealth : MonoBehaviour, IDamagable
{
    [SerializeField] private float _maxHealth;
    [SerializeField] private float _currentHealth;
    [SerializeField] private bool _isDead;

    [SerializeField] private HealthScreen _bloodScreen;
    [SerializeField] private HeadBobController _cameraShake;

    private float _healCD;
    private float _maxHealCD;
    private float _regenRate;

    private bool _startCD;
    private bool _canRegen;

    public event Action OnHealthChange;
    public event Action OnHealthAdd;
    public event Action OnDied;

    #region
    public float MaxHealth { get => _maxHealth; }
    public float CurrentHealth { get => _currentHealth;}
    public bool IsDead { get => _isDead; }
    #endregion

    public void AddHealth(float amount)
    {
        float maxCurrentDifference = _maxHealth - _currentHealth;
        _currentHealth += maxCurrentDifference > amount ? amount : maxCurrentDifference;

        if (OnHealthAdd != null)
        {
            OnHealthAdd();
        }
    }

    private void Start()
    {
        _currentHealth = _maxHealth;
    }

    private void Update()
    {
        if (_startCD)
        {
            _healCD -= Time.deltaTime;

            if (_healCD <= 0)
            {
                _canRegen = true;
                _startCD = false;
            }
        }

        if (_canRegen)
        {
            if (CurrentHealth < _maxHealth)
            {
                _currentHealth += Time.deltaTime * _regenRate;
                _bloodScreen.UpdateHealth();
            }  

            else
            {
                _currentHealth = _maxHealth;
                _healCD = _maxHealCD;
                _canRegen = false;
                _bloodScreen.SetHealthAlpha0();
            }
        }
    }


    public void Die(Vector3 direction)
    {
        Debug.Log("Player Dead");
        _isDead = false;
    }

    public void TakeDamage(int damage, Vector3 direction, int multiplier = 1)
    {
        if (_currentHealth > 0)
        {
            _currentHealth -= damage;
            _canRegen = false;

            if (OnHealthChange != null)
            {
                OnHealthChange();
            }
            //StartCoroutine(_cameraShake.StartShake());
            //StartCoroutine(_bloodScreen.HurtFlash());
            _bloodScreen.UpdateHealth();
            _healCD = _maxHealCD;
            _startCD = true;
        }

        if (_currentHealth <= 0)
        {
            Die(Vector3.zero);
        }
    }
}
