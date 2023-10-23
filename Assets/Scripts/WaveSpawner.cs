using System.Collections;
using UnityEngine;

public class WaveSpawner : MonoBehaviour
{
    [SerializeField] private Waves[] _waves;

    private int _currentEnemyIndex;
    private int _currentWaveIndex;
    private int _enemiesLeftToSpawn;

    private void Start()
    {
        _enemiesLeftToSpawn = _waves[0].WaveSettings.Length;
        LaunchWave();
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
        StartCoroutine(SpawnEnemyInWave());
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
    public GameObject Spawner { get => _enemyPrefab; }
    public float SpawnDelay { get => _spawnDelay; }
}
