using Cysharp.Threading.Tasks;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UIElements;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance;

    [SerializeField] private GameObject _loadingScreen;
    [SerializeField] private ProgressBar _progressBar;
    [SerializeField] private AudioClip _mainMenuMusic;

    private List<AsyncOperation> _scenesLoading = new List<AsyncOperation>();
    private AudioSource _audioSource;

    private float _totalSceneProgress;

    private void Awake()
    {
        Instance = this;

        SceneManager.LoadSceneAsync((int)SceneIndexes.MENU, LoadSceneMode.Additive);

        _audioSource = GetComponent<AudioSource>();
    }

    public async void LoadGame()
    {
        _audioSource.Stop();
        _loadingScreen.SetActive(true);

        _scenesLoading.Add(SceneManager.UnloadSceneAsync((int)SceneIndexes.MENU));
        _scenesLoading.Add(SceneManager.LoadSceneAsync((int)SceneIndexes.GAMEPLAY, LoadSceneMode.Additive));

        await GetSceneLoadProgress();
    }

    public async UniTask GetSceneLoadProgress()
    {
        foreach (var scene in _scenesLoading)
        {
            _totalSceneProgress = 0;

            foreach (AsyncOperation operation in _scenesLoading)
            {
                _totalSceneProgress += operation.progress;
            }

            _totalSceneProgress = (_totalSceneProgress / _scenesLoading.Count) * 100f;

            //_progressBar.value = Mathf.RoundToInt(_totalSceneProgress);

            await UniTask.WaitUntil(() => scene.isDone);
        }

        _loadingScreen.SetActive(false);
    }
}
