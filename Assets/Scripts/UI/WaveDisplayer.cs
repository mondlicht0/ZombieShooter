using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using DG.Tweening;
using Cysharp.Threading.Tasks;

public class WaveDisplayer : MonoBehaviour
{
	[SerializeField] private TextMeshProUGUI _remainingEnemies;
	[SerializeField] private TextMeshProUGUI _currentWave;
	[SerializeField] private TextMeshProUGUI _breaktime;
	[SerializeField] private CanvasGroup _waveComplete;

	public TextMeshProUGUI RemainingEnemies { get => _remainingEnemies;}
	public TextMeshProUGUI CurrentWave { get => _currentWave; }

	private void Awake()
	{
		GlobalEventManager.OnEnemyKilled.AddListener(Killed);
	}

	private void Killed(int remainingEnemies)
	{
		WaveSpawner.Instance.EnemyCount--;
		_remainingEnemies.text = $"ZOMBIES: {WaveSpawner.Instance.EnemyCount}";
	}

	public void UpdateRemainingText(string text)
	{
		_remainingEnemies.text = text;
	}

	public void UpdateWaveText(string text)
	{
		_currentWave.text = text;
	}

	public void UpdateBreaktimeText(float time)
	{
		_breaktime.text = $"BREAK: {time}";
	}

	public async void TurnBreaktime(bool on)
	{
		_waveComplete.DOFade(1, 0.2f);
		
		_remainingEnemies.transform.parent.gameObject.SetActive(!on);
		_currentWave.transform.parent.gameObject.SetActive(!on);

		_breaktime.transform.parent.gameObject.SetActive(on);
		
		await UniTask.Delay(1500);
		
		_waveComplete.DOFade(0, 0.2f);
	}
}
