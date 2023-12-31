﻿
using UnityEngine;
using UnityEngine.UI;
using System.Collections.Generic;
using HungryCannibal.UnderTheSeaUIKit.Dialogs;
using HungryCannibal.UnderTheSeaUIKit.ProgressBars;

public class PW_ResultInfo : MonoBehaviour
{
	public string winTitle = "Congratulation!"; 
	public string loseTitle = "Sorry, try again!"; 

	[Header("REFERENCES")]
	public DialogBehaviour dialog = null;
	public Text headerResult = null;
	public CounterBar winningResult = null;
	public ImageGifier resultLights = null;
	public List<Image> cubeDisplay = new List<Image>();
	public Sprite neutral = null;
	public List<Sprite> cubes = new List<Sprite>();

	public void ShowDisplay(PW_PlayResult playResult)
	{
		cubeDisplay.ForEach((Image display) => {
			display.sprite = neutral;
		});

		dialog.Show ();
		headerResult.text = playResult.getTotalPlayWin > playResult.getTotalPlayBet ? winTitle : loseTitle;
		winningResult.count = 0f;
		winningResult.IncrementCount(playResult.getTotalPlayWin, true);

		//Refresh cube display.
		int curCubeIndex = 0;
		for(int i = 0; i < playResult.result.Length; i++)
		{
			if(playResult.result[i] >= 1)
			{
				cubeDisplay [curCubeIndex].sprite = cubes [i];
				curCubeIndex += 1;
			}

			if(playResult.result[i] >= 2)
			{
				cubeDisplay [curCubeIndex].sprite = cubes [i];
				curCubeIndex += 1;
			}

			if(playResult.result[i] == 3)
			{
				cubeDisplay [curCubeIndex].sprite = cubes [i];
			}
		}
	}

	public void HideDisplay()
	{
		resultLights.speed = 1f;
		dialog.Hide ();
	}
}