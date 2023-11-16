using UnityEngine;
using UnityEngine.UIElements;

public class HealingMachine : MonoBehaviour
{
    public float HealAmount = 5f;
    public float HealRadius = 5f;
    public float RotateSpeed = 45f;
    public Vector3 RayOffset;

    private PlayerHealth _playerHealth;
    private GameObject _medSign;
    private HealthScreen _healthScreen;

    private void Start()
    {
        _medSign = transform.GetChild(0).gameObject;
        _healthScreen = FindObjectOfType<HealthScreen>();
        _playerHealth = FindObjectOfType<PlayerHealth>();
    }

    private void Update()
    {
        if (_playerHealth.CurrentHealth < _playerHealth.MaxHealth)
        {
            _medSign.SetActive(true);
            _medSign.transform.Rotate(Vector3.forward, RotateSpeed * Time.deltaTime);
            CheckHeal();
        }

        else _medSign.SetActive(false);

    }

    private void CheckHeal()
    {
        Ray ray = new Ray(transform.position + RayOffset, transform.right);
        RaycastHit hit;
        Debug.DrawRay(transform.position + RayOffset, transform.right);
        if (Physics.Raycast(ray, out hit, HealRadius))
        {
            if (hit.collider.TryGetComponent(out _playerHealth))
            {
                _playerHealth.AddHealth(HealAmount);
            }
        }
    }
}
