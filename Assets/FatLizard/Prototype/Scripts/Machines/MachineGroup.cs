using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using System.Collections.Generic;

public class MachineGroup : MonoBehaviour
{
	/// <summary>
	/// List of machines available on the current build.
	/// </summary>
	public List<MachinePrefabs> machinePrefabs = new List<MachinePrefabs>();

	/// <summary>
	/// Get the current machine on focused. Set if the machineInstance gameobject is activeInHeirachy
	/// set by animator when scrolling upon user selecting the machine.
	/// </summary>
	/// <value>The on focused machine.</value>
	public MachineInstance OnFocusedMachine
	{
		get
		{
			if(machinePrefabs.Exists(x => x != null && x.machineInstance.mCollider.enabled))
			{
				return machinePrefabs.Find (x => x != null && x.machineInstance.mCollider.enabled).machineInstance;
			}

			else
			{
				//Debug.LogError ("BugReport: DID NOT FOUND ANY FOCUSED MACHINE");
				return null;
			}
		}
	}

	public MachineInstance OnSelectedMachine
	{
		get
		{
			if(machinePrefabs.Exists(x => x.machineInstance.onSelected))
			{
				return machinePrefabs.Find (x => x.machineInstance.onSelected).machineInstance;
			}

			else
			{
				Debug.LogWarning ("CURRENTLY NO SELECTED MACHINE");
				return null;
			}
		}
	}
		
	public BillValue OnActiveChipPrefab
	{
		get
		{
			if(machinePrefabs.Exists(x => x.machineInstance.onSelected))
			{
				int index = machinePrefabs.Find (x => x.machineInstance.onSelected).machineInstance.colorPicker.currentChip;
				return machinePrefabs.Find (x => x.machineInstance.onSelected).billPrefab[index];
			}

			else
			{
				Debug.LogError ("BugReport: DID NOT FOUND SELECTED MACHINE");
				return null;
			}
		}
	}

	public void RefreshChipsDisplay()
	{
		if(machinePrefabs.Exists(x => x.machineInstance.onSelected))
		{
			MachinePrefabs prefab = machinePrefabs.Find (x => x.machineInstance.onSelected);

			for(int i = 0; i < CustomReference.Access.userInterfaces.chips.Count; i++)
			{
				CustomReference.Access.userInterfaces.chips [i].sprite = prefab.billTexture[i];
			}
		}

		else
		{
			Debug.LogError ("BugReport: DID NOT FOUND SELECTED MACHINE");
		}
	}

	/// <summary>
	/// Set the current bill on ready to be place as bet on color picker area.
	/// Inside this method includes total cash decrease by the placed bet.
	/// </summary>
	/// <param name="target">Target.</param>
	public void SetCurrentBillBet(Toggle target)
	{
		if (OnSelectedMachine == null)
			return;

		if(target.isOn)
		{
			OnSelectedMachine.colorPicker.currentChip = target.transform.GetSiblingIndex ();

			if(machinePrefabs.Exists(x => x.machineInstance.onSelected))
			{
				if(CustomReference.Access.userInterfaces.userDetails.currentCash >= OnActiveChipPrefab.amount)
				{
					OnSelectedMachine.colorPicker.ColorBlock (false);
				}

				else			
				{
					OnSelectedMachine.colorPicker.ColorBlock (true);
				}
			}

			else
			{
				Debug.LogError ("BugReport: DID NOT FOUND ANY SELECTED MACHINE");
			}
		}
	}

	public void ResetSelectedMachine()
	{
		if (OnSelectedMachine == null)
			return;

		if(OnSelectedMachine.colorPicker.betHolder != null)
		{
			foreach(Bets bets in OnSelectedMachine.playResult.colorBets)
			{
				foreach(Bet bet in bets.list)
				{
					CustomReference.Access.userInterfaces.userDetails.currentCash += bet.chipAmount * bet.chipNumber;
				}
			}

			Destroy(OnSelectedMachine.colorPicker.betHolder);
		} OnSelectedMachine.playResult = new PlayResult ();

		CustomReference.Access.userInterfaces.UpdateUserInfos ();
		OnSelectedMachine.ResetMachine ();
	}

	public void ExitSelectedMachine()
	{
		if (OnSelectedMachine == null)
			return;

		ResetSelectedMachine ();
		OnSelectedMachine.cubeChecker.Statictify (true);

		CustomReference.Access.userInterfaces.ToGameplay (false);

		CustomReference.Access.objectReferences.gameAnim.SetTrigger(OnSelectedMachine.name + "Out");
		StartCoroutine ( WaitForAnim() );
	}

	IEnumerator WaitForAnim()
	{
		yield return new WaitForSeconds (1f);
		OnSelectedMachine.onReadyPlay = false;
		OnSelectedMachine.onSelected = false;
	}
}