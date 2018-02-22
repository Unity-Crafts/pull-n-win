using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using System.Collections.Generic;

public enum InstaceAction
{
	ChooseAction, GetChildScript, GetChildBulbs
}

public class MachineInstance : MonoBehaviour
{
	[Header("MACHINE STATUS")]
	[Tooltip("Machine is in playable state. It means that machine lever can be pulled.")] //[HideInInspector]
	public bool onReadyPlay = true;
	[Tooltip("Machine is currently selected or can be interacted with on play.")] //[HideInInspector]
	public bool onSelected = true;
	[Tooltip("Machine is currently resetting and preparing to be playable asap.")] //[HideInInspector]
	public bool onResetting = false;

	[Header("SUBSCRIPT REFERENCE")]
	public CubeChecker cubeChecker = null;
	public LeverMecha leverMecha = null;
	public PaddleSet paddleSet = null;
	public ColorPicker colorPicker = null;
	public ComboPanel comboPanel = null;

	[Header("MACHINE ACTION")]
	public InstaceAction instanceAction = InstaceAction.ChooseAction;

	[HideInInspector] public Vector3 selector = Vector3.zero;
	[HideInInspector] public PlayResult playResult = new PlayResult ();
	[HideInInspector] public Collider[] colliders;
	[HideInInspector] public LedLight[] ledLights;

	private BoxCollider mainCol = null;
	public BoxCollider mCollider
	{
		get
		{
			if(mainCol == null) { mainCol = GetComponent<BoxCollider>(); }
			return mainCol;
		}
	}

	void OnValidate()
	{
		if(!mCollider.size.Equals(selector))
		{
			mCollider.size = selector;
		}

		if(instanceAction == InstaceAction.GetChildScript)
		{
			cubeChecker = GetComponentInChildren<CubeChecker> (true);
			if(cubeChecker != null) { cubeChecker.GetAllReference (); }

			leverMecha = GetComponentInChildren<LeverMecha> (true);
			if(leverMecha != null) { leverMecha.GetAllReference (); }

			paddleSet = GetComponentInChildren<PaddleSet> (true);
			colorPicker = GetComponentInChildren<ColorPicker> (true);
			comboPanel = GetComponentInChildren<ComboPanel> (true);
			colliders = GetComponentsInChildren<Collider> ();
		}

		if(instanceAction == InstaceAction.GetChildBulbs)
		{
			ledLights = GetComponentsInChildren<LedLight> ();
		}

		instanceAction = InstaceAction.ChooseAction;
	}

	void Update()
	{
		if(this.Equals(CustomReference.Access.machineGroups.OnFocusedMachine))
		{
			if(onSelected)
			{

				mCollider.size = Vector3.zero;
			}

			else
			{
				mCollider.size = selector;
			}
		}

		else
		{
			if(onSelected)
			{
				onSelected = false;
			}

			mCollider.size = selector;
		}

		foreach(Collider collide in colliders)
		{
			if(!mCollider.Equals(collide))
			{
				collide.enabled = onSelected;
			}
		}
	}

	public void ChangeLightFlicker(float frequency)
	{
		foreach(LedLight ledLight in ledLights)
		{
			ledLight.ledOn.duration = frequency;
		}
	}

	/// <summary>
	/// Resets the machine to playMode. This must be call when players want
	/// to start another cube roll or onFocusedMachine.
	/// </summary>
	public void ResetMachine()
	{
		playResult = new PlayResult (); //Create default instance of resultDatils.
		CustomReference.Access.userInterfaces.resultInfo.HideDisplay (); //Hide the resultInfo display.
		transform.rotation = Quaternion.identity;

		leverMecha.ioValue = 0F;
		cubeChecker.RandomCubeTransform ();
		onReadyPlay = true;

		Debug.Log("RESETTING...");
		ChangeLightFlicker(0.49f);
		Destroy (colorPicker.betHolder); //Destroy the BetHolder of bets.
		//onResetting = true; 
	}

	/// <summary>
	/// Initiate checking of cube result. The best time to call this is when the cube is all actually static.
	/// </summary>
	public void CheckMachine()
	{
		if (onReadyPlay) 
		{
			Debug.Log("CHECKING...");
			onReadyPlay = false;
			CustomReference.Access.PlaySound ("Release");
			StartCoroutine ( CheckingMachine() );

		} 
	}

	IEnumerator CheckingMachine() 
	{
		yield return new WaitUntil(() => cubeChecker.isAllCubesSteady() == true);

		CustomReference.Access.userInterfaces.checkingDisplay.SetActive(true);

		yield return new WaitForSeconds (1f);

		CustomReference.Access.userInterfaces.checkingDisplay.SetActive(false);

		//CHECK FOR CUBE COLOR RESULT!
		playResult.result = cubeChecker.CheckCubeResult ();

		//CHECK IF THERE IS A SPINNER TARGET.
		for(int index = 0; index < playResult.result.Length; index++)
		{
			if(playResult.result[index] == 1)
			{
				playResult.result[index] = playResult.result[index] * comboPanel.spinValue.oneColor;
			}

			if(playResult.result[index] == 2)
			{
				playResult.result[index] = playResult.result[index] * comboPanel.spinValue.twoColor;
			}

			if(playResult.result[index] == 3)
			{
				playResult.result[index] = playResult.result[index] * comboPanel.spinValue.threeColor;
			}
		}

		if(playResult.getTotalPlayWin > playResult.getTotalPlayBet)
		{
			//headerResult.text = "YOU WIN!";
			ChangeLightFlicker(0.12f);
		}

		else
		{
			//headerResult.text = "YOU LOSE!";
			ChangeLightFlicker(0.95f);
		}

		//UPDATE THE GAME INFO DISPLAY!
		CustomReference.Access.userInterfaces.resultInfo.ShowDisplay(playResult);

		ProcessPlayResult (playResult.getTotalPlayWin, playResult.getTotalPlayBet); 
	}

	/// <summary>
	/// Processes the play result whether to received cash from the wins or do nothing, just refresh the result info display.
	/// </summary>
	/// <param name="win">Window.</param>
	/// <param name="bet">Bet.</param>
	public void ProcessPlayResult(int win, int bet)
	{
		CustomReference.Access.userInterfaces.userDetails.currentCash += win;
		CustomReference.Access.userInterfaces.UpdateUserInfos ();
		CustomReference.Access.userInterfaces.UpdateUserRecord(win, bet);

		leverMecha.handleCollider.enabled = true;
		//CustomReference.Access.scriptInterface.CloudSave_AutoIniteractions (true);
	}
}