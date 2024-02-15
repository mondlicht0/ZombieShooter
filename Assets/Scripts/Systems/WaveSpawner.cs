using Cysharp.Threading.Tasks;
using System;
using System.Collections;
using UnityEngine;
using DG.Tweening;
using System.Collections.Generic;

public class WaveSpawner : MonoBehaviour
{
    public static WaveSpawner Instance;
    
    public int EnemyCount;

    [SerializeField] private Zombie _zombiePrefab;
    [SerializeField] private List<Transform> _spawnPoints;

    [SerializeField] private Wave[] _waves;
    [SerializeField] private WaveDisplayer _waveDisplayer;

    // make button, that will be generate random waves

    private int _currentEnemyIndex;
    private int _currentWaveIndex;
    private int _enemiesLeftToSpawn;

    [Header("Audio")]
    private AudioSource _audio;
    [SerializeField] private float _smoothVolumeFade = 0.2f;
    [SerializeField] private float _musicVolume = 0.3f;

    [Header("Weapon Store")]
    [SerializeField] private WeaponStore _weaponStore;

    [SerializeField] private float _breakTime = 60;

    public int CurrentEnemyIndex { get => _currentEnemyIndex; }


    private void Awake()
    {
        Instance = this;

        _audio = GetComponent<AudioSource>();
    }

    private void Start()
    {
        _enemiesLeftToSpawn = _waves[0].WaveSettings.Length;
        //SpawnEnemyInWaveWithout();
        SpawnEnemyInWave2();
    }

    private void SpawnEnemyInWaveWithout()
    {
        _weaponStore.gameObject.SetActive(false);

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

    private void SpawnEnemyInWave2()
    {
        _weaponStore.gameObject.SetActive(false);

        EnemyCount = _waves[_currentWaveIndex].WaveSettings.Length;

        _waveDisplayer.RemainingEnemies.text = $"ZOMBIES: {EnemyCount}";
        _waveDisplayer.CurrentWave.text = $"WAVE: {_currentWaveIndex}";

        if (_enemiesLeftToSpawn > 0)
        {
            //yield return new WaitForSeconds(_waves[_currentWaveIndex].WaveSettings[_currentEnemyIndex].SpawnDelay);
            Instantiate(_zombiePrefab,
                        GetRandomPosition(),
                        Quaternion.identity);

            _enemiesLeftToSpawn--;
            _currentEnemyIndex++;
            SpawnEnemyInWave2();
            //StartCoroutine(SpawnEnemyInWave());
        }

        else
        {

            if (_currentWaveIndex < _waves.Length - 1)
            {
                _currentWaveIndex++;
                _enemiesLeftToSpawn = 3 + _currentWaveIndex;
                _currentEnemyIndex = 0;
            }
        }
    }

    private Vector3 GetRandomPosition()
    {
        return _spawnPoints[UnityEngine.Random.Range(0, _spawnPoints.Count)].position;
    }

    public void LaunchWave()
    {
        Breaktime(true);
        //StartCoroutine(SpawnEnemyInWave());
    }

    public async void Breaktime(bool on)
    {
        _waveDisplayer.TurnBreaktime(on);
        _weaponStore.gameObject.SetActive(true);

        _audio.DOFade(0, _smoothVolumeFade);

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

        _audio.DOFade(_musicVolume, _smoothVolumeFade);

        SpawnEnemyInWave2();
    }
}

[System.Serializable]
public class Wave
{
    public string Name = "Wave";
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
