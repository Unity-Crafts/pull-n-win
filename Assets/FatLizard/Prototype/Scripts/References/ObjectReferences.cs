
using UnityEngine;
using UnityEngine.UI;
using System.Collections.Generic;

public class ObjectReferences : MonoBehaviour
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
			CustomEvents.OnIntroToMenuEvent ();
		}

		if(events.Equals("OnGameplay"))
		{
			CustomEvents.OnPrepareGameplay ();
		}

		else if(events.Equals("OnZoomAM"))
		{
			CustomReference.Access.userInterfaces.ShowGameplay (true);
			CustomReference.Access.userInterfaces.ToGameplay (true);
			CustomReference.Access.machineGroups.machinePrefabs [0].machineInstance.ResetMachine ();
		}

		else if(events.Equals("OnZoomBM"))
		{
			CustomReference.Access.userInterfaces.ShowGameplay (true);
			CustomReference.Access.userInterfaces.ToGameplay (true);
			CustomReference.Access.machineGroups.machinePrefabs [1].machineInstance.ResetMachine ();
		}

		else if(events.Equals("OnZoomCM"))
		{
			CustomReference.Access.userInterfaces.ShowGameplay (true);
			CustomReference.Access.userInterfaces.ToGameplay (true);
			CustomReference.Access.machineGroups.machinePrefabs [2].machineInstance.ResetMachine ();
		}

		else if(events.Equals("OnZoomDM"))
		{
			CustomReference.Access.userInterfaces.ShowGameplay (true);
			CustomReference.Access.userInterfaces.ToGameplay (true);
			CustomReference.Access.machineGroups.machinePrefabs [3].machineInstance.ResetMachine ();
		}

		else if(events.Equals("OnFocused"))
		{
			CustomReference.Access.camScripts.raycastEnabled = true;
		}

		else
		{
			Debug.LogWarning ("AnimEvent: " + events + " is not found!");
		}
	}
}