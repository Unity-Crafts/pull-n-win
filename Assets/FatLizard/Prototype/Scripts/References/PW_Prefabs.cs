
using UnityEngine;
using UnityEngine.UI;
using System.Collections.Generic;

public class PW_Prefabs : MonoBehaviour
{
	[Header("INTERACTORS")]
	public Animator gameAnim = null;
	public Transform gameSound = null;
	public Animator offerCoins = null;

	public List<GameObject> designObjects;

	public void AnimatorEvent(string events)
	{
		Debug.Log ("AnimEvent: " + events);

		if(events.Equals("OnMenu"))
		{
			PW_CustomEvents.OnIntroToMenuEvent ();
		}

		if(events.Equals("OnGameplay"))
		{
			PW_CustomEvents.OnPrepareGameplay ();
		}

		else if(events.Equals("OnZoomAM"))
		{
			PW_References.Access.userInterfaces.ShowGameplay (true);
			PW_References.Access.userInterfaces.ToGameplay (true);
			PW_References.Access.machineGroups.machinePrefabs [0].machineInstance.ResetMachine ();
		}

		else if(events.Equals("OnZoomBM"))
		{
			PW_References.Access.userInterfaces.ShowGameplay (true);
			PW_References.Access.userInterfaces.ToGameplay (true);
			PW_References.Access.machineGroups.machinePrefabs [1].machineInstance.ResetMachine ();
		}

		else if(events.Equals("OnZoomCM"))
		{
			PW_References.Access.userInterfaces.ShowGameplay (true);
			PW_References.Access.userInterfaces.ToGameplay (true);
			PW_References.Access.machineGroups.machinePrefabs [2].machineInstance.ResetMachine ();
		}

		else if(events.Equals("OnZoomDM"))
		{
			PW_References.Access.userInterfaces.ShowGameplay (true);
			PW_References.Access.userInterfaces.ToGameplay (true);
			PW_References.Access.machineGroups.machinePrefabs [3].machineInstance.ResetMachine ();
		}

		else if(events.Equals("OnFocused"))
		{
			PW_References.Access.camScripts.raycastEnabled = true;
		}

		else
		{
			Debug.LogWarning ("AnimEvent: " + events + " is not found!");
		}
	}
}