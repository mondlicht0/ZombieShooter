using UnityEngine;
using UnityEngine.UIElements;

public class HealingMachine : MonoBehaviour
{
    public float HealAmount = 5f;
    public float HealRadius = 5f;
    public float RotateSpeed = 45f;
    public Vector3 RayOffset;

    [SerializeField] private PlayerHealth _playerHealth;

    private RaycastHit _hit;
    private GameObject _medSign;
    private HealthScreen _healthScreen;

    private void Start()
    {
        _medSign = transform.GetChild(0).gameObject;
        _healthScreen = FindObjectOfType<HealthScreen>();
        //_playerHealth = FindObjectOfType<PlayerHealth>();
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
        if (Physics.Raycast(ray, out _hit, HealRadius))
        {
            if (_hit.collider.TryGetComponent(out _playerHealth))
            {
                Debug.Log("check heal");
                _healthScreen.HealFlash();
                float maxCurrentDifference = _playerHealth.MaxHealth - _playerHealth.CurrentHealth;
                _playerHealth.CurrentHealth += maxCurrentDifference > HealAmount ? HealAmount : maxCurrentDifference;
            }
        }
    }
}
