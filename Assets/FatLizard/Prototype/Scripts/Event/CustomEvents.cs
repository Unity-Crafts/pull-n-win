
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
	public static void OnIntroToMenuEvent()
	{
		Debug.Log ("OnMenuEvent: User is currently on authentication display.");
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