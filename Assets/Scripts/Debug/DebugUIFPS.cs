using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class DebugUIFPS : MonoBehaviour
{
    public TextMeshProUGUI LeftMouse;
    public TextMeshProUGUI RightMouse;

    public InputHandler Input;

    private void Update()
    {
        LeftMouse.text = $"Left Mouse: {Input.IsAttack}";
        RightMouse.text = $"Right Mouse: {Input.IsAim}";
    }
}
