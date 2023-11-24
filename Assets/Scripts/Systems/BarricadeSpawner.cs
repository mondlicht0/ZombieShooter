using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.AI;

public class BarricadeSpawner : Interactable
{
    public bool _isDestroyed = false;

    [SerializeField] private List<Transform> _planks;
    [SerializeField] private Transform _planksParent;
    [SerializeField] private int _currentPlanksCount = 0;

    [Header("UI")]
    [SerializeField] private Canvas _canvas;
    [SerializeField] private TextMeshProUGUI _text;
    [SerializeField] private MoneyDisplayer _moneyDisplayer;
    [Space]
    [SerializeField] private string _lackOfWoods = "NOT ENOUGH MONEY";

    private int _woodPlanks = 0;
    private int _price = 100;

    private BoxCollider _barricadeCollider;
    private NavMeshObstacle _obstacle;

    public bool IsDestroyed { get => _isDestroyed; }

    private void Awake()
    {
        _barricadeCollider = GetComponent<BoxCollider>();
        _obstacle = GetComponent<NavMeshObstacle>();
    }

    private void Start()
    {
        _woodPlanks = PlayerData.Instance.Woods;

        SetPlanksList();
        CheckBarricadeWoodsAndFull();

        _text.text = Text;

        //_currentPlanksCount = _planks.Count;
    }

    protected override void Interact()
    {
        if (PlayerData.Instance.Money >= 0 && _currentPlanksCount != _planks.Count)
        {
            for (int i = 0; i < _planks.Count && PlayerData.Instance.Money != 0; i++)
            {
                if (_planks[i].gameObject.activeSelf == false)
                {
                    _planks[i].gameObject.SetActive(true);

                    _currentPlanksCount++;
                    PlayerData.Instance.Money -= _price;
                    _moneyDisplayer.UpdateMoneyText(PlayerData.Instance.Money);

                    _obstacle.enabled = true;
                    _barricadeCollider.enabled = true;

                    CheckBarricadeWoodsAndFull();
                }
            }
        }
    }

    public void AddBoard()
    {

    }

    private bool IsBarricadeDestroyed()
    {
        return _currentPlanksCount == 0;
    }

    public void RemoveBoard()
    {
        if (!IsBarricadeDestroyed())
        {
            for (int i = 0; i <= _planks.Count; i++)
            {
                if (_planks[i].transform.gameObject.activeSelf)
                {
                    _planks[i].transform.gameObject.SetActive(false);
                    _currentPlanksCount--;
                    Debug.Log("Remove Board end");
                    return;
                }
            }
        } else
        {
            _isDestroyed = true;
            _obstacle.enabled = false;
            _barricadeCollider.enabled = false;
        }
    }

    private void SetPlanksList()
    {
        _planksParent.GetComponentsInChildren(true, _planks);
        _planks.Remove(_planks[0]);
    }

    private bool IsFull()
    {
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


    private void OnTriggerEnter(Collider other)
    {
        if (other.TryGetComponent(out NPCAgent agent))
        {
            if (agent.attackWall) RemoveBoard();
        }
    }
}

