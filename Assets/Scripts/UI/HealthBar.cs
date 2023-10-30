using TMPro;
using UnityEngine;

public class HealthBar : MonoBehaviour
{
    [SerializeField] private PlayerHealth _playerHealth;

    private TextMeshProUGUI _healthText;

    private void Awake()
    {
        _healthText = GetComponent<TextMeshProUGUI>();
    }

    private void UpdateText()
    {
        Debug.Log("Health changed");
        _healthText.text = $"{_playerHealth.CurrentHealth}";
    }

    private void OnEnable()
    {
        _playerHealth.OnHealthChange += UpdateText;
    }

    private void OnDisable()
    {
        _playerHealth.OnHealthChange -= UpdateText;
    }
}
