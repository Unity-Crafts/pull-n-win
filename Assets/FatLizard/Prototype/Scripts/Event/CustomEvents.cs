
using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public enum Notification
{
	/// <summary>
	/// There is no chips bet, yet trying to pull the lever.
	/// </summary>
	NoChipsBet, 
	/// <summary>
	/// The chip is higher than the current virtual cash.
	/// </summary>
	ChipLowerCash,
	/// <summary>
	/// The insufficient virtual cash or almost zero balance.
	/// </summary>
	Insufficient
}

public class CustomEvents
{
	//Called when intro animation is about to enter main menu idling.
	public static void OnIntroToMenuEvent()
	{
		CustomReference.Access.userInterfaces.menuDisplay.SetActive (true);
		CustomReference.Access.userInterfaces.authWindow.SetActive (true);
		CustomReference.Access.userInterfaces.menuWindow.SetActive (false);

		Debug.Log ("OnMenuEvent: User is currently on authentication display.");
	}

	//Called when camera is about to idle on the first machine and wait for user interactions.
	public static void OnPrepareGameplay()
	{
		CustomReference.Access.userInterfaces.ShowGameplay (false);
		CustomReference.Access.userInterfaces.ToGameplay (false);
		CustomReference.Access.objectReferences.designObjects.ForEach ((GameObject gobjs) => {
			gobjs.SetActive(false);
		});

		CustomReference.Access.userInterfaces.SetRaycastOn (true);
		CustomReference.Access.machineGroups.machinePrefabs[0].machineInstance.mCollider.enabled = true;
		CustomReference.Access.userInterfaces.machineText.text = "Bronze Machine";
		CustomReference.Access.userInterfaces.prevButton.SetActive (false);

		Debug.Log ("OnGameEvent: User is currently on machine chooser display.");
	}

	public static void OnAuthenticationEvent(bool success)
	{
		Debug.Log ("OnAuthEvent: User is finished authenticating: Success: " + success);
	}

	public static void OnPlayResultEvent(bool winner)
	{
		Debug.Log ("OnResultEvent: Machine is finished calculating result: Winner: " + winner);
	}

	public static void OnNotificationEvents(Notification notify)
	{
		if(notify == Notification.NoChipsBet)
		{
			Debug.Log ("OnNotifyEvent: There is no chips bet, yet trying to pull the lever.");
		}

		else if(notify == Notification.ChipLowerCash)
		{
			Debug.Log ("OnNotifyEvent: The insufficient virtual cash in relation to selected chip.");
		}

		else if(notify == Notification.Insufficient)
		{
			Debug.Log ("OnNotifyEvent: The insufficient virtual cash or almost zero balance.");
		}
	}
}