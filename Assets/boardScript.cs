using UnityEngine;
using System.Collections;

public class boardScript : MonoBehaviour {

	public int boards, previousBoards;
	public Animator[] boardAnim;
	public GameObject[] board;
	public AudioClip repairSound;
	public AudioClip bangSound;

	private void Start () {

		boardAnim = GetComponentsInChildren<Animator> ();

		for (int i = 0; i < 6; i++)
		{
			boardAnim[i].Play ("boardAnimation" + (i+1).ToString());
		}

		boards = 6;

	}

    private void AddBoard()
	{
		if (boards < 6)
		{
			board[boards].SetActive(true);
			boardAnim[boards].Play ("boardAnimation" + (boards+1).ToString()); 
			boards += 1;
			GetComponent<AudioSource>().PlayOneShot(repairSound, 1.0f / GetComponent<AudioSource>().volume);
			Invoke ("SlamSound", 1f);
		}
	}

    private void RemoveBoard()
	{
		if(boards > 0)
		{
			board[boards - 1].GetComponent<playAnimation>().DisableBoard();
			boards -= 1;
			
			if(boards == 0)
				GetComponent<Renderer>().enabled = false;

		}
	}

    private void SlamSound()
	{
		GetComponent<AudioSource>().PlayOneShot (bangSound, 1.0f / GetComponent<AudioSource>().volume);
	}
}








