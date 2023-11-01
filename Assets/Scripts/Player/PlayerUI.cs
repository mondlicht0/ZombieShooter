using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class PlayerUI : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI _interactText;

    public void UpdateText(string text)
    {
        _interactText.text = text;
    }
}
