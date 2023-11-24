using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UIElements;

[RequireComponent(typeof(UIDocument))]
public class MenuUIInitalizator : MonoBehaviour
{
    [SerializeField] private List<ButtonEntry> _buttonEntries;
    [SerializeField] private VisualTreeAsset _buttonTemplate;

    private VisualElement _container;

    private void Start()
    {
        MenuInit();
    }

    private void MenuInit()
    {
        _container = GetComponent<UIDocument>().rootVisualElement.Q<VisualElement>("LeftColumn");

        foreach (ButtonEntry buttonEntry in _buttonEntries)
        {
            VisualElement newElement = _buttonTemplate.CloneTree();
            Button button = newElement.Q<Button>("menu-button");
            button.text = buttonEntry.Name;

            if (buttonEntry.Name == "Play")
            {
                button.clicked += delegate { GameManager.Instance.LoadGame(); };
            }

            _container.Add(newElement);
        }

    }

    private void OnClick(ButtonEntry buttonEntry)
    {
        Debug.Log($"Clicked on {buttonEntry.Name}");
        buttonEntry.Callback.Invoke();
    }
}

public abstract class MenuEntry
{
    public string Name;
}

[System.Serializable]
public class ButtonEntry : MenuEntry
{
    public UnityEvent Callback;
}
