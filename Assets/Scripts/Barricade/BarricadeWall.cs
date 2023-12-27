using UnityEngine;
using System.Collections.Generic;
using UnityEngine.ProBuilder.Shapes;

public class BarricadeWall : Interactable 
{
	public bool IsDestroyed;
	public int Price;

	[SerializeField] private int _boards, _previousBoards;
    [SerializeField] private int _currentPlanksCount = 0;
    [SerializeField] private List<Transform> _planks;
    [SerializeField] private Animator[] _boardAnim;
    [SerializeField] private GameObject[] _board;
    [SerializeField] private AudioClip _repairSound;
    [SerializeField] private AudioClip _bangSound;
    [SerializeField] private MoneyDisplayer _moneyDisplayer;

    private void Start () {
		_boardAnim = GetComponentsInChildren<Animator>();
		/*
				for (int i = 0; i < 6; i++)
				{
					_boardAnim[i].Play ("boardAnimation" + (i+1).ToString());
				}

				_boards = 6;*/

		IsDestroyedWall();

	}

    protected override void Interact()
    {
        if (PlayerData.Instance.Money >= 0)
        {
            AddBoard();
        }
    }


    private void AddBoard()
	{
		if (_boards < 6 && PlayerData.Instance.Money >= Price)
		{
            _board[_boards].SetActive(true);
			_boardAnim[_boards].Play ("boardAnimation" + (_boards + 1).ToString());
            _boards += 1;
			GetComponent<AudioSource>().PlayOneShot(_repairSound, 1.0f / GetComponent<AudioSource>().volume);
            Invoke ("SlamSound", 1f);

            PlayerData.Instance.Money -= Price;
            _moneyDisplayer.UpdateMoneyText(PlayerData.Instance.Money);

            IsDestroyed = false;
		}
	}

	public void RemoveBoard()
	{
		if(_boards > 0)
		{
			_board[_boards - 1].SendMessage("DisableBoard",SendMessageOptions.RequireReceiver);
            _boards -= 1;

			if (_boards == 0)
				IsDestroyed = true;
				GetComponent<Renderer>().enabled = false;

		}
	}

	private void SlamSound()
	{
		GetComponent<AudioSource>().PlayOneShot (_bangSound, 1.0f / GetComponent<AudioSource>().volume);
	}

	private void IsDestroyedWall()
	{
		IsDestroyed = _boards == 0;
	}
}








