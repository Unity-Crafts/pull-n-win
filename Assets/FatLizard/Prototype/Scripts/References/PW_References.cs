using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using System.Collections;

public class PW_References 
{
	private static PW_References access = null;
	public static PW_References Access 
	{
		get 
		{
			if (access == null) 
			{
				access = new PW_References();
			}
			return access;
		}
	}

	private PW_Interfaces userInterface = null;
	public PW_Interfaces userInterfaces
	{
		get
		{
			if(userInterface == null)
			{
				userInterface = GameObject.FindObjectOfType<PW_Interfaces> ();
			}

			return userInterface;
		}
	}

	private PW_MGroup machineGroup = null;
	public PW_MGroup machineGroups
	{
		get
		{
			if(machineGroup == null)
			{
				machineGroup = GameObject.FindObjectOfType<PW_MGroup> ();
			}

			return machineGroup;
		}
	}

	private PW_Prefabs objectReference = null;
	public PW_Prefabs objectReferences
	{
		get
		{
			if(objectReference == null)
			{
				objectReference = GameObject.FindObjectOfType<PW_Prefabs> ();
			}

			return objectReference;
		}
	}

	private PW_CamScript camScript = null;
	public PW_CamScript camScripts
	{
		get
		{
			if(camScript == null)
			{
				camScript = GameObject.FindObjectOfType<PW_CamScript> ();
			}

			return camScript;
		}
	}

	public void PlaySound(string audio)
	{
		//objectReferences.transform.Find (audio).GetComponent<AudioSource> ().Play ();
	}
}