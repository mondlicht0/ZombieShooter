using UnityEngine;
using DG.Tweening;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class GameOverDisplayer : MonoBehaviour
{
    [SerializeField] private CanvasGroup _gameOverScreen;
    [SerializeField] private PlayerHealth _playerHealth;
    [SerializeField] private GameObject _gunSelec;

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
        _gameOverScreen.DOFade(1, 2f);
        _gunSelec.SetActive(false);
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
