﻿using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SocialPlatforms;

using System;
using System.Collections;
using System.Collections.Generic;

using HungryCannibal.UnderTheSeaUIKit.Dialogs;
using HungryCannibal.UnderTheSeaUIKit.ProgressBars;
using HungryCannibal.UnderTheSeaUIKit;

public class PW_Interfaces : MonoBehaviour 
{
	[Header("PLAYER INFO")]
	public PW_UserDetails userDetails = new PW_UserDetails();
	public PW_UserRecords userRecords = new PW_UserRecords();

	[Header("LIGHT FLICKERINGS")]
	public FlickerValue fickerValue = new FlickerValue();

	[Header("COMBO PANEL")]
	public SpinnerValue spinValue = new SpinnerValue();

	[Header("MAIN DISPLAYS")]
	public GameObject menuDisplay = null;
	public GameObject gameDisplay = null;
	public Button powerUps = null;

	[Header("MACHINE SELECTION")]
	public Text machineText = null;
	public GameObject prevButton = null;
	public GameObject nextButton = null;

	[Header("Animator")]
	public Animator menuAnim = null;
	public Animator chooseAnim = null;
	public Animator playAnim = null;

	[Header("MAIN GAMEPLAY")]
	public ViewSwitcher gameplaySwitcher = null;
	public CounterBar chipMenuDisplay = null;
	public CounterBar gemMenuDisplay = null;
	public CounterBar chipGameDisplay = null;
	public CounterBar gemGameDisplay = null;
	public PW_ResultInfo resultInfo = null;
	public List<Image> chips = new List<Image> ();

	[Header("NOTIFICATIONS")]
	public ViewSwitcher viewSwitcher = null;
	public DialogBehaviour alertDialog = null;
	public GameObject rewardDisplay = null;
	public GameObject checkingDisplay = null;
	public DialogBehaviour confirmDialog = null;

	[Header("DEBUG INFORMATION")]
	public bool debugConsole = true;
	public enum Debugs { Log, Warn, Error }
	public void DebugLog(Debugs debugs, string content)
	{
		if(debugConsole)
		{
			if(debugs == Debugs.Warn)
			{
				Debug.LogWarning (content);
			}

			else if(debugs == Debugs.Error)
			{
				Debug.LogError (content);
			}

			else
			{
				Debug.Log (content);
			}
		}
	}

	void Awake()
	{
		Screen.sleepTimeout = SleepTimeout.NeverSleep;
	}

	public void StartGame()
	{
		StartCoroutine (StartingGame());
	}

	private IEnumerator StartingGame()
	{
		menuAnim.SetTrigger ("Hide");
		gameDisplay.SetActive (true);
		viewSwitcher.SwitchView (1);
		PW_References.Access.objectReferences.gameAnim.SetTrigger ("MenuToGame");

		yield return new WaitForSeconds (1f);
		menuDisplay.SetActive (false);
	}

	public void ConfirmExit()
	{
		confirmDialog.Hide ();
		viewSwitcher.viewIndex = 0;
		SetRaycastOn (false);

		PW_References.Access.userInterfaces.gameplaySwitcher.viewIndex = 1;
		PW_References.Access.machineGroups.OnFocusedMachine.mCollider.enabled = false;
		PW_References.Access.objectReferences.gameAnim.SetTrigger ("GameToMenu");

		PW_References.Access.objectReferences.designObjects.ForEach ((GameObject gobjs) => {
			gobjs.SetActive(true);
		});
		//gameDisplay.SetActive (false); //Remove as chooseDisplay animate on init.
	}

	public void ShowConfirm(bool showing)
	{
		if(showing)
		{
			SetRaycastOn (false);
			confirmDialog.Show ("Do you really want to switch to other account?");
			chooseAnim.SetBool ("IsShowing", false);
		}

		else
		{
			confirmDialog.Hide ();
			chooseAnim.SetBool ("IsShowing", true);
			SetRaycastOn (true);
		}
	}

	public void ToGameplay(bool yes)
	{
		if(yes)
		{
			//chooseAnim.gameObject.SetActive (false);
			//playAnim.gameObject.SetActive (true);
			PW_References.Access.userInterfaces.gameplaySwitcher.viewIndex = 0;
			PW_References.Access.userInterfaces.gameplaySwitcher.SwitchView (1);

		}

		else
		{
			//playAnim.gameObject.SetActive (false);
			//chooseAnim.gameObject.SetActive (true);
			PW_References.Access.userInterfaces.gameplaySwitcher.viewIndex = 1;
			PW_References.Access.userInterfaces.gameplaySwitcher.SwitchView (0);
		}

		//chooseAnim.SetBool ("IsShowing", !yes);
		//playAnim.SetBool ("IsShowing", yes);
	}

	public void RefreshUserDetails(PW_UserDetails _userDetails)
	{
		userDetails = _userDetails;
		PW_References.Access.userInterfaces.chipMenuDisplay.count = userDetails.currentChips;
		PW_References.Access.userInterfaces.gemMenuDisplay.count = userDetails.currentGems;
	}

	public void UpdateUserInfos()
	{
		chipGameDisplay.count = userDetails.currentChips;
	}

	public void UpdateUserRecord(int win, int bet)
	{
		userRecords.totalPlays += 1;
		PW_CustomEvents.OnPlayResultEvent ( (win > bet) );

		if(win > bet)
		{
			userRecords.numberWin += 1;
			userRecords.totalWin += win;
			PW_References.Access.PlaySound ("Winner");
		}

		if(win < bet) //Update new Total Loss Amount!
		{
			userRecords.numberLoss += 1;
			userRecords.totalLoss += bet - win;
			PW_References.Access.PlaySound ("Loser");
		}

		userRecords.numberBet += 1;
		userRecords.totalBet += bet;

		//Update new Max Bet Amount!
		if(userRecords.maxBet < bet)
		{
			userRecords.maxBet = bet;
		}

		//Update new Max Win Amount!
		if(userRecords.maxWin < win) 
		{
			userRecords.maxWin = win;
		}

		//Update new Max Loss Amount!
		if(userRecords.maxLoss < bet - win)
		{
			userRecords.maxLoss = bet - win;
		}
	}

	public void SetRaycastOn(bool active)
	{
		if(active)
		{
			StartCoroutine (OnRaycasting(active));
		}

		else
		{
			PW_References.Access.camScripts.raycastEnabled = active;
		}

		DebugLog (Debugs.Log, "Machine raycasting is turned " + active);
	}

	IEnumerator OnRaycasting(bool active)
	{
		yield return new WaitForSeconds (1f);
		PW_References.Access.camScripts.raycastEnabled = active;
	}
}