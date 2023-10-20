using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class PlayerHealth : MonoBehaviour, IDamagable
{
    public float MaxHealth;
    public float CurrentHealth;
    public bool isDead;

    [SerializeField] private HealthScreen _bloodScreen;
    [SerializeField] private TextMeshProUGUI _healthText;
    [SerializeField] private HeadBobController _cameraShake;

    private float healCD;
    private float maxHealCD;
    private float regenRate;

    private bool startCD;
    private bool canRegen;

    private void Start()
    {
        CurrentHealth = MaxHealth;
    }

    private void Update()
    {
        _healthText.text = $"{CurrentHealth}";

        if (startCD)
        {
            healCD -= Time.deltaTime;

            if (healCD <= 0)
            {
                canRegen = true;
                startCD = false;
            }
        }

        if (canRegen)
        {
            if (CurrentHealth < MaxHealth)
            {
                CurrentHealth += Time.deltaTime * regenRate;
                _bloodScreen.UpdateHealth();
            }  

            else
            {
                CurrentHealth = MaxHealth;
                healCD = maxHealCD;
                canRegen = false;
                _bloodScreen.SetHealthAlpha0();
            }
        }
    }


    public void Die(Vector3 direction)
    {
        Debug.Log("Dead");
        isDead = false;
    }

    public void TakeDamage(int damage, Vector3 direction)
    {
        if (CurrentHealth > 0)
        {
            CurrentHealth -= damage;
            canRegen = false;
            StartCoroutine(_cameraShake.StartShake());
            StartCoroutine(_bloodScreen.HurtFlash());
            _bloodScreen.UpdateHealth();
            healCD = maxHealCD;
            startCD = true;
        }

        if (CurrentHealth <= 0)
        {
            Die(Vector3.zero);
        }
    }
}
