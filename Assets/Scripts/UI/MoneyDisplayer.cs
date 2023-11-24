using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class MoneyDisplayer : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI _moneyText;

    public void UpdateMoneyText(int amount)
    {
        _moneyText.text = $"$: {amount}";
    }
}
