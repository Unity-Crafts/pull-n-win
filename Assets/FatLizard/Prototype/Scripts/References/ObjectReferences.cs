
using UnityEngine;
using UnityEngine.UI;
using System.Collections.Generic;

public class ObjectReferences : MonoBehaviour
{
	[Header("INTERACTORS")]
	public Animator gameAnim = null;
	public Transform gameSound = null;
	public Animator offerCoins = null;

	public void AnimatorEvent(string events)
	{
		Debug.Log ("AnimEvent: " + events);

		if(events.Equals("OnMenu"))
		{
			CustomReference.Access.machineGroups.machinePrefabs[0].machineInstance.mCollider.enabled = true;
			CustomReference.Access.userInterfaces.menuDisplay.SetActive (true);
			CustomEvents.OnIntroToMenuEvent ();
		}

		else if(events.Equals("OnZoomAM"))
		{
			CustomReference.Access.userInterfaces.playWindow.SetActive (true);
			CustomReference.Access.machineGroups.machinePrefabs [0].machineInstance.ResetMachine ();
		}

		else if(events.Equals("OnZoomBM"))
		{
			CustomReference.Access.userInterfaces.playWindow.SetActive (true);
			CustomReference.Access.machineGroups.machinePrefabs [1].machineInstance.ResetMachine ();
		}

		else if(events.Equals("OnZoomCM"))
		{
			CustomReference.Access.userInterfaces.playWindow.SetActive (true);
			CustomReference.Access.machineGroups.machinePrefabs [2].machineInstance.ResetMachine ();
		}

		else if(events.Equals("OnZoomDM"))
		{
			CustomReference.Access.userInterfaces.playWindow.SetActive (true);
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