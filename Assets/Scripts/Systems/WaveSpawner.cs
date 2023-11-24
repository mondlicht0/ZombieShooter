using Cysharp.Threading.Tasks;
using System;
using System.Collections;
using UnityEngine;

public class WaveSpawner : MonoBehaviour
{
    public static WaveSpawner Instance;
    public int EnemyCount;

    [SerializeField] private Waves[] _waves;
    [SerializeField] private WaveDisplayer _waveDisplayer;

    private int _currentEnemyIndex;
    private int _currentWaveIndex;
    private int _enemiesLeftToSpawn;

    private float _breakTime = 5;

    public int CurrentEnemyIndex { get => _currentEnemyIndex; }


    private void Awake()
    {
        Instance = this;
    }

    private void Start()
    {
        _enemiesLeftToSpawn = _waves[0].WaveSettings.Length;
        SpawnEnemyInWaveWithout();
    }

    private void SpawnEnemyInWaveWithout()
    {
        EnemyCount = _waves[_currentWaveIndex].WaveSettings.Length;

        _waveDisplayer.RemainingEnemies.text = $"ZOMBIES: {EnemyCount}";
        _waveDisplayer.CurrentWave.text = $"WAVE: {_currentWaveIndex}";

        if (_enemiesLeftToSpawn > 0)
        {
            //yield return new WaitForSeconds(_waves[_currentWaveIndex].WaveSettings[_currentEnemyIndex].SpawnDelay);
            Instantiate(_waves[_currentWaveIndex].WaveSettings[_currentEnemyIndex].EnemyPrefab,
                        _waves[_currentWaveIndex].WaveSettings[_currentEnemyIndex].Spawner.transform.position,
                        Quaternion.identity);
            _enemiesLeftToSpawn--;
            _currentEnemyIndex++;
            SpawnEnemyInWaveWithout();
            //StartCoroutine(SpawnEnemyInWave());
        }

        else
        {
            
            if (_currentWaveIndex < _waves.Length - 1)
            {
                _currentWaveIndex++;
                _enemiesLeftToSpawn = _waves[_currentWaveIndex].WaveSettings.Length;
                _currentEnemyIndex = 0;
            }
        }
    }


    private IEnumerator SpawnEnemyInWave()
    {
        if (_enemiesLeftToSpawn > 0)
        {
            yield return new WaitForSeconds(_waves[_currentWaveIndex].WaveSettings[_currentEnemyIndex].SpawnDelay);
            Instantiate(_waves[_currentWaveIndex].WaveSettings[_currentEnemyIndex].EnemyPrefab, 
                        _waves[_currentWaveIndex].WaveSettings[_currentEnemyIndex].Spawner.transform.position, 
                        Quaternion.identity);
            _enemiesLeftToSpawn--;
            _currentEnemyIndex++;
            StartCoroutine(SpawnEnemyInWave());
        }

        else
        {
            if (_currentWaveIndex < _waves.Length - 1)
            {
                _currentWaveIndex++;
                _enemiesLeftToSpawn = _waves[_currentWaveIndex].WaveSettings.Length;
                _currentEnemyIndex = 0;
            }
        }
    }

    public void LaunchWave()
    {
        Breaktime(true);
        //StartCoroutine(SpawnEnemyInWave());
    }

    public async void Breaktime(bool on)
    {
        _waveDisplayer.TurnBreaktime(on);

        float remainingTime = _breakTime;

        while (remainingTime > 0f)
        {
            Debug.Log("Turn");
            _waveDisplayer.UpdateBreaktimeText(remainingTime);

            await UniTask.Delay(1000);

            remainingTime--;

        }

        //await UniTask.Delay(TimeSpan.FromSeconds(_breakTime));

        _waveDisplayer.TurnBreaktime(!on);

        SpawnEnemyInWaveWithout();
    }
}

[System.Serializable]
public class Waves
{
    [SerializeField] private WaveSettings[] _waveSettings;
    public WaveSettings[] WaveSettings { get => _waveSettings; }
}

[System.Serializable]
public class WaveSettings
{
    [SerializeField] private GameObject _enemyPrefab;
    [SerializeField] private GameObject _spawner;
    [SerializeField] private float _spawnDelay;

    public GameObject EnemyPrefab { get => _enemyPrefab; }
    public GameObject Spawner { get => _spawner; }
    public float SpawnDelay { get => _spawnDelay; }
}
