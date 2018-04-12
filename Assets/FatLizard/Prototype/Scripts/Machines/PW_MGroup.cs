using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using System.Collections.Generic;

public class PW_MGroup : MonoBehaviour
{
	/// <summary>
	/// List of machines available on the current build.
	/// </summary>
	public List<PW_MPrefabs> machinePrefabs = new List<PW_MPrefabs>();

	/// <summary>
	/// Get the current machine on focused. Set if the machineInstance gameobject is activeInHeirachy
	/// set by animator when scrolling upon user selecting the machine.
	/// </summary>
	/// <value>The on focused machine.</value>
	public PW_MInstance OnFocusedMachine
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

	public PW_MInstance OnSelectedMachine
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
		
	public PW_BillValue OnActiveChipPrefab
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
			PW_MPrefabs prefab = machinePrefabs.Find (x => x.machineInstance.onSelected);

			for(int i = 0; i < PW_References.Access.userInterfaces.chips.Count; i++)
			{
				PW_References.Access.userInterfaces.chips [i].sprite = prefab.billTexture[i];
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
				if(PW_References.Access.userInterfaces.userDetails.currentCash >= OnActiveChipPrefab.amount)
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
			foreach(PW_Bets bets in OnSelectedMachine.playResult.colorBets)
			{
				foreach(PW_Bet bet in bets.list)
				{
					PW_References.Access.userInterfaces.userDetails.currentCash += bet.chipAmount * bet.chipNumber;
				}
			}

			Destroy(OnSelectedMachine.colorPicker.betHolder);
		} OnSelectedMachine.playResult = new PW_PlayResult ();

		PW_References.Access.userInterfaces.UpdateUserInfos ();
		OnSelectedMachine.ResetMachine ();
	}

	public void ExitSelectedMachine()
	{
		if (OnSelectedMachine == null)
			return;

		ResetSelectedMachine ();
		OnSelectedMachine.cubeChecker.Statictify (true);

		PW_References.Access.userInterfaces.ToGameplay (false);

		PW_References.Access.objectReferences.gameAnim.SetTrigger(OnSelectedMachine.name + "Out");
		StartCoroutine ( WaitForAnim() );
	}

	IEnumerator WaitForAnim()
	{
		yield return new WaitForSeconds (1f);
		OnSelectedMachine.onReadyPlay = false;
		OnSelectedMachine.onSelected = false;
	}
}