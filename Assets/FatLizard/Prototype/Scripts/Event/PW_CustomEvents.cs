
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

public class PW_CustomEvents
{
	//Called when intro animation is about to enter main menu idling.
	public static void OnIntroToMenuEvent()
	{
		//Temporary - Remove upon addition of the server side!
		PW_UserDetails ud = new PW_UserDetails ();
		ud.currentChips = 1000000;
		ud.currentGems = 10;
		PW_References.Access.userInterfaces.RefreshUserDetails (ud);

		PW_References.Access.userInterfaces.menuDisplay.SetActive (true);
		PW_References.Access.userInterfaces.menuDisplay.GetComponent<CanvasGroup> ().alpha = 1f;
		PW_References.Access.userInterfaces.menuAnim.SetTrigger ("Show");
		PW_References.Access.userInterfaces.
			DebugLog (PW_Interfaces.Debugs.Log, "OnMenuEvent: User is currently on Main Menu display.");
	}

	//Called when camera is about to idle on the first machine and wait for user interactions.
	public static void OnPrepareGameplay()
	{
		PW_References.Access.userInterfaces.ShowGameplay (false);
		PW_References.Access.userInterfaces.ToGameplay (false);

		PW_References.Access.objectReferences.designObjects.ForEach ((GameObject gobjs) => {
			gobjs.SetActive(false);
		});

		PW_References.Access.userInterfaces.SetRaycastOn (true);
		PW_References.Access.machineGroups.machinePrefabs[0].machineInstance.mCollider.enabled = true;
		PW_References.Access.userInterfaces.machineText.text = "Bronze Machine";
		PW_References.Access.userInterfaces.prevButton.SetActive (false);

		PW_References.Access.userInterfaces.chipGameDisplay.count = 
			PW_References.Access.userInterfaces.userDetails.currentChips;
		PW_References.Access.userInterfaces.gemGameDisplay.count = 
			PW_References.Access.userInterfaces.userDetails.currentGems;

		PW_References.Access.userInterfaces.
			DebugLog (PW_Interfaces.Debugs.Log, "OnGameEvent: User is currently on machine chooser display.");
	}

	public static void OnPlayResultEvent(bool winner)
	{
		PW_References.Access.userInterfaces.
			DebugLog (PW_Interfaces.Debugs.Log, "OnResultEvent: Machine is finished calculating result: Winner: " + winner);
	}

	public static void OnNotificationEvents(Notification notify)
	{
		if(notify == Notification.NoChipsBet)
		{
			PW_References.Access.userInterfaces.
				DebugLog (PW_Interfaces.Debugs.Log, "OnNotifyEvent: There is no chips bet, yet trying to pull the lever.");
		}

		else if(notify == Notification.ChipLowerCash)
		{
			PW_References.Access.userInterfaces.
				DebugLog (PW_Interfaces.Debugs.Log, "OnNotifyEvent: The insufficient virtual cash in relation to selected chip.");
		}

		else if(notify == Notification.Insufficient)
		{
			PW_References.Access.userInterfaces.
				DebugLog (PW_Interfaces.Debugs.Log, "OnNotifyEvent: The insufficient virtual cash or almost zero balance.");
		}
	}
}