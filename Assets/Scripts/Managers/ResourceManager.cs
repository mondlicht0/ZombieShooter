using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ResourceManager : MonoBehaviour
{
    [SerializeField] private MoneyDisplayer _moneyDisplayer;

    private void Awake()
    {
        GlobalEventManager.OnEnemyKilled.AddListener(Killed);
    }

    private void Start()
    {
        _moneyDisplayer.UpdateMoneyText(PlayerData.Instance.Money);
    }

    private void Killed(int n)
    {
        PlayerData.Instance.Money += 100;
        _moneyDisplayer.UpdateMoneyText(PlayerData.Instance.Money);
    }
}
