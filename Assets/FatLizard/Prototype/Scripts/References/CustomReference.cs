using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using System.Collections;

public class CustomReference 
{
	private static CustomReference access = null;
	public static CustomReference Access 
	{
		get 
		{
			if (access == null) 
			{
				access = new CustomReference();
			}
			return access;
		}
	}

	private UserInterfaces userInterface = null;
	public UserInterfaces userInterfaces
	{
		get
		{
			if(userInterface == null)
			{
				userInterface = GameObject.FindObjectOfType<UserInterfaces> ();
			}

			return userInterface;
		}
	}

	private MachineGroup machineGroup = null;
	public MachineGroup machineGroups
	{
		get
		{
			if(machineGroup == null)
			{
				machineGroup = GameObject.FindObjectOfType<MachineGroup> ();
			}

			return machineGroup;
		}
	}

	private ObjectReferences objectReference = null;
	public ObjectReferences objectReferences
	{
		get
		{
			if(objectReference == null)
			{
				objectReference = GameObject.FindObjectOfType<ObjectReferences> ();
			}

			return objectReference;
		}
	}

	private CamScript camScript = null;
	public CamScript camScripts
	{
		get
		{
			if(camScript == null)
			{
				camScript = GameObject.FindObjectOfType<CamScript> ();
			}

			return camScript;
		}
	}

	public void PlaySound(string audio)
	{
		//objectReferences.transform.Find (audio).GetComponent<AudioSource> ().Play ();
	}
}