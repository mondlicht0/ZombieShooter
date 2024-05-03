using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Health : MonoBehaviour, IDamagable
{
	public bool IsDead { get; private set; }
	[SerializeField] private int _currentHealth;
	[SerializeField] private int _maxHealth = 100;
	[SerializeField] private bool _isInjured = false;
	[SerializeField] private float _dieForce = 2f;
	[SerializeField] private float _fadeOutDelay = 2f;


	public event Action OnHealthChange;
	public event Action OnDied;

	private void Start()
	{
		_currentHealth = _maxHealth;
	}

	public void TakeDamage(int damage, Vector3 direction, int multiplier = 1)
	{
		int damageTaken = Mathf.Clamp(damage, 0, _currentHealth);

		if (_currentHealth > 0)
		{
			_currentHealth -= damageTaken * multiplier;
		}
		if (_currentHealth <= 30f && _currentHealth > 0)
		{
			_isInjured = true;
		}
		if (_currentHealth <= 0 && !IsDead)
		{
			//_isDead = true;
			Die(direction);
		}
	}

	public IEnumerator FadeOut()
	{
		Debug.Log("Fade out");
		yield return new WaitForSeconds(_fadeOutDelay);

		float time = 0;
		while (time < 1)
		{
			transform.position += Vector3.down * Time.deltaTime;
			time += Time.deltaTime;
			yield return null;
		}
		Destroy(gameObject);
	}

	private void OnDestroy()
	{
		if (FindObjectsByType<Zombie>(FindObjectsSortMode.None).Length == 0)
		{
			WaveSpawner.Instance.LaunchWave();
		}
	}

	public void Die(Vector3 direction)
	{
		Debug.Log("Dead");
		IsDead = true;
		OnDied?.Invoke();

		_currentHealth = 0;
		_isInjured = false;
		

		if (WaveSpawner.Instance != null)
		{
			GlobalEventManager.SendEnemyKilled(WaveSpawner.Instance.EnemyCount);
		}

		StartCoroutine(FadeOut());

	}
}
