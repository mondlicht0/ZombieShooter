using System.Collections.Generic;
using UnityEngine;

public class BarricadeSpawner : Interactable
{
    [SerializeField] private List<Transform> _planks;

    public int WoodPlanks = 0;

    private BoxCollider _barricadeCollider;
    private int _currentPlanksCount = 0;

    private void Awake()
    {
        _barricadeCollider = GetComponent<BoxCollider>();
    }

    private void Start()
    {
        WoodPlanks = PlayerData.Instance.Woods;

        if (WoodPlanks <= 0) IsInteractable = false;

        SetPlanksList();
    }

    private void Update()
    {
        if (WoodPlanks <= 0) IsInteractable = false;
        else IsInteractable = true; // TODO: Change it to events
    }

    protected override void Interact()
    {
        if (PlayerData.Instance.Woods <= 0 || _currentPlanksCount == _planks.Count) return;

        else
        {
            for (int i = 0; i < _planks.Count && PlayerData.Instance.Woods != 0; i++)
            {
                if (_planks[i].gameObject.activeSelf == false)
                {
                    _planks[i].gameObject.SetActive(true);

                    _currentPlanksCount++;
                    PlayerData.Instance.Woods--;
                }
            }

            if (_currentPlanksCount == _planks.Count) IsInteractable = false;
        }
    }

    private void SetPlanksList()
    {
        gameObject.GetComponentsInChildren(true, _planks);
        _planks.Remove(_planks[0]);

        foreach (Transform plank in _planks) plank.gameObject.SetActive(false);
    }
}

