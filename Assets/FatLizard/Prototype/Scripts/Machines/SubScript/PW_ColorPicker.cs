
using UnityEngine;
using UnityEngine.UI;
using System.Collections.Generic;

public class PW_ColorPicker : MonoBehaviour
{
	[Header("ANIMATORS")]
	public Animator colorBlocker = null;

	[HideInInspector] public int currentChip = 0;
	[HideInInspector] public bool colorBlock = false;
	[HideInInspector] public GameObject betHolder = null;

	private PW_MInstance mInstance = null;
	public PW_MInstance machine
	{
		get
		{
			if(mInstance == null)
			{
				mInstance = transform.parent.GetComponent<PW_MInstance> ();
			}

			return mInstance;
		}
	}

	void Update () 
	{
		if(!machine.onSelected) { return; }

		if ((Input.touchCount > 0 && Input.GetTouch(0).phase == TouchPhase.Began) || Input.GetMouseButtonDown(0))
		{
			//Mouse or Touch screenpont.
			Vector3 castPos = Vector3.zero;

			if (Application.isMobilePlatform)
			{
				castPos = Input.GetTouch(0).position;
			}
				
			else
			{
				castPos = Input.mousePosition;
			}

			//Initialized Raycast!
			Ray rayTouch = Camera.main.ScreenPointToRay(castPos);
			RaycastHit hitInfo = new RaycastHit();

			if (Physics.Raycast (rayTouch, out hitInfo)) 
			{
				Debug.DrawLine (rayTouch.origin, hitInfo.point, Color.green, 1f, true);
				if(hitInfo.collider != null) { UserRaycastMechanism (hitInfo); }
			}
		}
	}

	void UserRaycastMechanism(RaycastHit hitInfo)
	{
		if(hitInfo.transform.GetComponentInParent<PW_ColorPicker>() != null && machine.onReadyPlay)
		{
			if(PW_References.Access.userInterfaces.userDetails.currentCash > 0)
			{
				PW_BillValue betting = PW_References.Access.machineGroups.OnActiveChipPrefab;

				if(PW_References.Access.userInterfaces.userDetails.currentCash >= betting.amount)
				{
					//ADD THE SPECIFIED AMOUNT TO THE COLOR PICKED!
					machine.playResult.UpdateResult
					(
						hitInfo.collider.transform.GetSiblingIndex (), //GET THE CURRENT INDEX OF THE COLOR PICKED.
						PW_References.Access.machineGroups.OnActiveChipPrefab.amount //GET THE CURRENT AMOUNT OF CHIP PLACED.
					);

					//SUBCTRACT THE AMOUNT FROM THE TOTAL CASH!
					PW_References.Access.userInterfaces.userDetails.currentCash -= betting.amount;

					//CREATE A GAMEOBJECT TO HOLD BET PREFABS!
					if(betHolder == null)
					{
						betHolder = new GameObject("BetHolder"); 
						betHolder.tag = "UserBet" ;
					}

					//INSTANTIATE THE CURRENT BILL PREFAB SELECTED AND PARENT TO BetHolder!
					Vector3 tempPos = new Vector3(hitInfo.point.x, hitInfo.point.y + 0.7F, hitInfo.point.z);
					Instantiate(betting, tempPos, Quaternion.identity, betHolder.transform);

					//RECORDS - THIS IS FOR THE TOTAL BET PLACED BY THE USER!
					PW_References.Access.userInterfaces.userRecords.totalBet += betting.amount;

					//UPDATE USER STATS INFO.
					PW_References.Access.userInterfaces.UpdateUserInfos ();
				}

				else
				{
					ColorBlock (true);
					PW_CustomEvents.OnNotificationEvents (Notification.ChipLowerCash);
				}
			}

			else
			{
				if( !PW_References.Access.objectReferences.offerCoins.GetCurrentAnimatorStateInfo(0).IsTag("Showing") )
				{
					PW_References.Access.objectReferences.offerCoins.SetTrigger ("Show");
					PW_CustomEvents.OnNotificationEvents (Notification.Insufficient);
				}
			}
		}
	}

	public void ColorBlock(bool activeBlockage)
	{
		if(activeBlockage)
		{
			if(!colorBlock)
			{
				PW_References.Access.userInterfaces.lowerBetDisplay.SetActive (true);
				colorBlock = true;
			}
		}

		else
		{
			if(colorBlock)
			{
				PW_References.Access.userInterfaces.lowerBetDisplay.SetActive (false);
				colorBlock = false;
			}
		}
	}
}