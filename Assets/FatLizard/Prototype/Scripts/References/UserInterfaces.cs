using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SocialPlatforms;

using System;
using System.Collections;
using System.Collections.Generic;

public class UserInterfaces : MonoBehaviour 
{
	[Header("PLAYER INFO")]
	public UserDetails userDetails = new UserDetails();

	[Header("PLAYER RECORDS")]
	public UserRecords userRecords = new UserRecords();

	[Header("MAIN DISPLAYS")]
	public GameObject menuDisplay = null;
	public GameObject gameDisplay = null;

	[Header("AUTHENTICATION")]
	public Text userName = null;
	public Button authButton = null;
	public GameObject authWindow = null;
	public GameObject menuWindow = null;

	[Header("MACHINE SELECTION")]
	public Text machineText = null;
	public GameObject prevButton = null;
	public GameObject nextButton = null;

	[Header("Animator")]
	public Animator playAnim = null;
	public Animator chooseAnim = null;

	public void ConfirmExit()
	{
		SetRaycastOn (false);
		confirmExit.SetActive (false);
		Deauthenticate ();
	}

	public void ToGameplay(bool yes)
	{
		if(yes)
		{
			playAnim.gameObject.SetActive (true);
		}

		else
		{
			chooseAnim.gameObject.SetActive (true);
		}

		playAnim.SetBool ("IsShowing", yes);
		chooseAnim.SetBool ("IsShowing", !yes);
	}

	public void ShowGameplay(bool yes)
	{
		playAnim.gameObject.SetActive (yes);
		chooseAnim.gameObject.SetActive (!yes);
	}

	[Header("MAIN GAMEPLAY")]
	public Text cashText = null;
	public ResultInfo resultInfo = null;
	public List<Image> chips = new List<Image> ();

	[Header("NOTIFICATIONS")]
	public GameObject noBetDisplay = null;
	public GameObject rewardDisplay = null;
	public GameObject lowerBetDisplay = null;
	public GameObject checkingDisplay = null;

	[Header("EXIT MENU")]
	public GameObject confirmExit = null;

	public void ShowConfirm(bool showing)
	{
		if(showing)
		{
			SetRaycastOn (false);
			confirmExit.SetActive (true);
			chooseAnim.SetBool ("IsShowing", false);
		}

		else
		{
			confirmExit.SetActive (false);
			chooseAnim.SetBool ("IsShowing", true);
			SetRaycastOn (true);
		}
	}

	public void Authenticate()
	{
		authButton.interactable = false;
		authWindow.SetActive(false);
		menuWindow.SetActive(true);
	}

	public void StartGame()
	{
		menuDisplay.SetActive (false);
		gameDisplay.SetActive (true);
		CustomReference.Access.objectReferences.gameAnim.SetTrigger ("MenuToGame");
	}

	public void Deauthenticate()
	{
		userDetails.userName = String.Empty;
		userName.text = String.Empty;
		CustomReference.Access.machineGroups.OnFocusedMachine.mCollider.enabled = false;
		CustomReference.Access.objectReferences.gameAnim.SetTrigger ("GameToMenu");

		CustomReference.Access.objectReferences.designObjects.ForEach ((GameObject gobjs) => {
			gobjs.SetActive(true);
		});

		authButton.interactable = true;
		gameDisplay.SetActive (false);
		menuWindow.SetActive(false);
		authWindow.SetActive(true);
	}

	public void UpdateUserInfos()
	{
		cashText.text = userDetails.currentCash.ToString();
	}

	public void UpdateUserRecord(int win, int bet)
	{
		userRecords.totalPlays += 1;
		CustomEvents.OnPlayResultEvent ( (win > bet) );

		if(win > bet)
		{
			userRecords.numberWin += 1;
			userRecords.totalWin += win;
			CustomReference.Access.PlaySound ("Winner");
		}

		if(win < bet) //Update new Total Loss Amount!
		{
			userRecords.numberLoss += 1;
			userRecords.totalLoss += bet - win;
			CustomReference.Access.PlaySound ("Loser");
		}

		if (bet > 0) //Update new Record Details!
		{
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

		else 
		{
			CustomReference.Access.PlaySound ("NoBet");
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
			CustomReference.Access.camScripts.raycastEnabled = active;
		}
	}

	IEnumerator OnRaycasting(bool active)
	{
		yield return new WaitForSeconds (1f);
		CustomReference.Access.camScripts.raycastEnabled = active;
	}
}