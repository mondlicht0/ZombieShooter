using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class BarricadeSpawner : Interactable
{
    [SerializeField] private List<Transform> _planks;
    [SerializeField] private Transform _planksParent;

    [Header("UI")]
    [SerializeField] private Canvas _canvas;
    [SerializeField] private TextMeshProUGUI _text;
    [Space]
    [SerializeField] private string _lackOfWoods = "Not enough woods";

    private int _woodPlanks = 0;

    private BoxCollider _barricadeCollider;
    private int _currentPlanksCount = 0;

    private void Awake()
    {
        _barricadeCollider = GetComponent<BoxCollider>();
    }

    private void Start()
    {
        _woodPlanks = PlayerData.Instance.Woods;

        SetPlanksList();
        CheckBarricadeWoodsAndFull();

        _text.text = Text;
    }

    protected override void Interact()
    {
        if (PlayerData.Instance.Woods >= 0 && _currentPlanksCount != _planks.Count)
        {
            for (int i = 0; i < _planks.Count && PlayerData.Instance.Woods != 0; i++)
            {
                if (_planks[i].gameObject.activeSelf == false)
                {
                    _planks[i].gameObject.SetActive(true);

                    _currentPlanksCount++;
                    PlayerData.Instance.Woods--;

                    CheckBarricadeWoodsAndFull();
                }
            }
        }
    }

    private void SetPlanksList()
    {
        _planksParent.GetComponentsInChildren(true, _planks);
        _planks.Remove(_planks[0]);
    }

    private bool IsFull()
    {
        Debug.Log(_currentPlanksCount);
        Debug.Log(_currentPlanksCount == _planks.Count);
        return _currentPlanksCount == _planks.Count;
    }

    private void CheckBarricadeWoodsAndFull()
    {
        if (_woodPlanks <= 0 || IsFull())
        {
            Text = !IsFull() ? _lackOfWoods : "";
            IsInteractable = false;
            return;
        }
    }
}

