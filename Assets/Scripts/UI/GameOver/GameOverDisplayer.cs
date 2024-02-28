using UnityEngine;
using DG.Tweening;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class GameOverDisplayer : MonoBehaviour
{
    [SerializeField] private Image _gameOverScreen;
    [SerializeField] private PlayerHealth _playerHealth;

    private void OnEnable()
    {
        _playerHealth.OnDied += EnableGameOverScreen;
    }

    private void OnDisable()
    {
        _playerHealth.OnDied -= EnableGameOverScreen;
    }

    private void Start()
    {
        _gameOverScreen.gameObject.SetActive(false);
    }

    public void EnableGameOverScreen()
    {
        _gameOverScreen.DOFade(1, 3f);
        _gameOverScreen.gameObject.SetActive(true);
    }

    public void LoadNewGame()
    {
        SceneManager.LoadScene((int)SceneIndexes.GAMEPLAY);
    }

    public void LoadMenu()
    {
        SceneManager.LoadScene((int)SceneIndexes.BOOTSTRAP);
    }
}
