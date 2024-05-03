using Cysharp.Threading.Tasks;
using System;
using System.Collections;
using UnityEngine;
using DG.Tweening;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEditor.Animations;
using Random = UnityEngine.Random;

public class WaveSpawner : MonoBehaviour
{
    public static WaveSpawner Instance;
    
    public int EnemyCount;

    //[SerializeField] private Zombie _zombiePrefab;
    [SerializeField] private List<Transform> _spawnPoints;
    [SerializeField] private List<AnimatorController> _animators;

    [SerializeField] private Wave[] _waves;
    [SerializeField] private WaveDisplayer _waveDisplayer;
    [SerializeField] private List<Zombie> _zombiesPrefabs;

    // make button, that will be generate random waves

    private int _currentEnemyIndex;
    private int _currentWaveIndex;
    private int _enemiesLeftToSpawn;

    [Header("Audio")]
    private AudioSource _audio;
    [SerializeField] private float _smoothVolumeFade = 0.2f;
    [SerializeField] private float _musicVolume = 0.3f;

    [Header("Weapon Store")]
    //[SerializeField] private WeaponStore _weaponStore;

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
        SpawnEnemyInWaveWithout();
    }

    private void SpawnEnemyInWaveWithout()
    {
        //_weaponStore.gameObject.SetActive(false);

        EnemyCount = _waves[_currentWaveIndex].WaveSettings.Length;

        _waveDisplayer.RemainingEnemies.text = $"ZOMBIES: {EnemyCount}";
        _waveDisplayer.CurrentWave.text = $"WAVE: {_currentWaveIndex}";

        if (_enemiesLeftToSpawn > 0)
        {
            Zombie zombie = Instantiate(_zombiesPrefabs[Random.Range(0, _zombiesPrefabs.Count)],
                        _waves[_currentWaveIndex].WaveSettings[_currentEnemyIndex].Spawner.transform.position,
                        Quaternion.identity);
            
            zombie.Animator.runtimeAnimatorController = _animators[Random.Range(0, _animators.Count)];

            if (zombie.Animator.runtimeAnimatorController.name == "ShineZombie_Fast")
            {
                zombie.Agent.speed = 3;
            }
            
            
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

            Instantiate(_zombiesPrefabs[Random.Range(0, _zombiesPrefabs.Count)], 
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
        //_weaponStore.gameObject.SetActive(false);

        EnemyCount = _waves[_currentWaveIndex].WaveSettings.Length;

        _waveDisplayer.RemainingEnemies.text = $"ZOMBIES: {EnemyCount}";
        _waveDisplayer.CurrentWave.text = $"WAVE: {_currentWaveIndex}";

        // if (_enemiesLeftToSpawn > 0)
        // {
        //     Instantiate(_zombiePrefab,
        //                 GetRandomPosition(),
        //                 Quaternion.identity);

        //     _enemiesLeftToSpawn--;
        //     _currentEnemyIndex++;
        //     SpawnEnemyInWave2();
        // }

        // else
        // {

        //     if (_currentWaveIndex < _waves.Length - 1)
        //     {
        //         _currentWaveIndex++;
        //         _enemiesLeftToSpawn = _waves[_currentWaveIndex].WaveSettings.Length;
        //         _currentEnemyIndex = 0;
        //     }
        // }
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
        //_weaponStore.gameObject.SetActive(true);

        _audio.DOFade(0, _smoothVolumeFade);

        float remainingTime = _breakTime;

        while (remainingTime > 0f)
        {
            _waveDisplayer.UpdateBreaktimeText(remainingTime);

            await UniTask.Delay(1000);

            remainingTime--;

        }

        _waveDisplayer.TurnBreaktime(!on);

        _audio.DOFade(_musicVolume, _smoothVolumeFade);

        SpawnEnemyInWaveWithout();
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
