
using UnityEngine;
using UnityEngine.UI;
using System.Collections.Generic;

public class PW_ResultInfo : MonoBehaviour
{
	public Text headerResult = null;
	public Text winningResult = null;
	public Text totalChipPlace = null;

	public void ShowDisplay(PW_PlayResult playResult)
	{
		if(playResult.getTotalPlayWin > playResult.getTotalPlayBet)
		{
			headerResult.text = "YOU WIN!";
		}

		else
		{
			headerResult.text = "YOU LOSE!";
		}

		totalChipPlace.text = playResult.getTotalPlayBet + "";
		winningResult.text = playResult.getTotalPlayWin + "";

		gameObject.SetActive (true);
	}

	//"P" + colorPicked[index] + ".00 X " + colorBets[index] + " = P" + (colorPicked[index] * colorBets[index]) + ".00";

	public void HideDisplay()
	{
		headerResult.text = "PLAY n' WIN";

		totalChipPlace.text = "P0.00"; 
		winningResult.text = "P0.00"; 

		gameObject.SetActive (false);
	}
}