using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SocialPlatforms;

using System;
using System.Collections;
using System.Collections.Generic;

using GooglePlayGames;
using GooglePlayGames.BasicApi;

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

	[Header("MAIN GAMEPLAY")]
	public GameObject playWindow = null;
	public Text cashText = null;
	public ResultInfo resultInfo = null;
	public List<Image> chips = new List<Image> ();

	[Header("NOTIFICATIONS")]
	public GameObject noBetDisplay = null;
	public GameObject rewardDisplay = null;
	public GameObject lowerBetDisplay = null;
	public GameObject checkingDisplay = null;

	void Awake()
	{
		PlayGamesClientConfiguration config = new PlayGamesClientConfiguration.Builder()
			.RequestServerAuthCode(false)
			.RequestIdToken()
			.Build();

		PlayGamesPlatform.InitializeInstance(config);
		PlayGamesPlatform.DebugLogEnabled = true;
		PlayGamesPlatform.Activate();
	}

	public void Authenticate()
	{
		authButton.interactable = false;

		PlayGamesPlatform.Instance.Authenticate((bool success) =>
		{
			if(success)
			{
				userDetails.userName = PlayGamesPlatform.Instance.GetUserDisplayName();
				userName.text = userDetails.userName;

				authButton.interactable = true;
				authWindow.SetActive(false);
				menuWindow.SetActive(true);
			}

			else
			{
				authButton.interactable = true;
			}
		});

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
		PlayGamesPlatform.Instance.SignOut();
		userDetails.userName = String.Empty;
		userName.text = String.Empty;
		CustomReference.Access.machineGroups.OnFocusedMachine.mCollider.enabled = false;
		CustomReference.Access.objectReferences.gameAnim.SetTrigger ("GameToMenu");

		gameDisplay.SetActive (false);
		menuWindow.SetActive(false);
		authWindow.SetActive(true);
	}

	public void ShowAchievement()
	{
		PlayGamesPlatform.Instance.ShowAchievementsUI ();
	}

	public void ShowLeaderboard()
	{
		PlayGamesPlatform.Instance.ShowLeaderboardUI ();
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
		CustomReference.Access.camScripts.raycastEnabled = active;
	}
}