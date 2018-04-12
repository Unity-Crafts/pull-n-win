
using UnityEngine;
using UnityEngine.UI;

using System.Collections;
using System.Collections.Generic;

public class PW_CamScript : MonoBehaviour
{
	[Header("DEBUG INFORMATION")]
	public bool raycastEnabled = true;

	private Vector3 prevScreenPos = Vector3.zero;
	private Vector3 curScreenPos = Vector3.zero;

	void Update ()
	{
		if(PW_References.Access.machineGroups.OnFocusedMachine != null)
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

			if ((Input.touchCount > 0 && Input.GetTouch(0).phase == TouchPhase.Began) || Input.GetMouseButtonDown(0))
			{
				prevScreenPos = castPos; curScreenPos = castPos;
			}

			if ((Input.touchCount > 0 && Input.GetTouch (0).phase == TouchPhase.Moved) || Input.GetMouseButton (0))
			{
				curScreenPos = castPos;
			}

			if ((Input.touchCount > 0 && Input.GetTouch (0).phase == TouchPhase.Ended) || Input.GetMouseButtonUp (0))
			{
				if (!raycastEnabled)
					return;

				if(curScreenPos.Equals(prevScreenPos))
				{
					Raycast(castPos);
				}

				else
				{
					if(PW_References.Access.machineGroups.OnSelectedMachine == null)
					{
						float deltaSwipe = curScreenPos.x - prevScreenPos.x;

						if(deltaSwipe > 0)
						{
							float maxSwipeRight = Screen.width * 0.25f;

							if(deltaSwipe > maxSwipeRight)
							{
								ChangeMachine (false);
								//Debug.Log ( maxSwipeRight + "Pointers was moved!" + deltaSwipe);
							}
						}

						else if(deltaSwipe < 0)
						{
							float maxSwipeLeft = -Screen.width * 0.25f;

							if(maxSwipeLeft > deltaSwipe)
							{
								ChangeMachine (true);
								//Debug.Log ( maxSwipeLeft + "Pointers was moved!" + deltaSwipe);
							}
						}
					}
				}
			}
		}
	}

	private void Raycast(Vector3 castPos)
	{
		if (Camera.main == null)
			return;

		//Initialized Raycast!
		Ray rayTouch = Camera.main.ScreenPointToRay(castPos);
		RaycastHit hitInfo = new RaycastHit();

		if (Physics.Raycast (rayTouch, out hitInfo)) 
		{
			Debug.DrawLine (rayTouch.origin, hitInfo.point, Color.blue, 1f, true);

			if(PW_References.Access.machineGroups.OnFocusedMachine.onSelected)
			{
				return;
			}

			if(hitInfo.collider != null && PW_References.Access.objectReferences.gameAnim.GetCurrentAnimatorStateInfo(0).IsTag("Out"))
			{
				if(hitInfo.collider.transform.GetSiblingIndex() == 0 && !PW_References.Access.objectReferences.gameAnim.IsInTransition(0))
				{
					PW_References.Access.objectReferences.gameAnim.SetTrigger ("AMzoomIn");
				}

				else if(hitInfo.collider.transform.GetSiblingIndex() == 1 && !PW_References.Access.objectReferences.gameAnim.IsInTransition(0))
				{
					PW_References.Access.objectReferences.gameAnim.SetTrigger ("BMzoomIn");
				}

				else if(hitInfo.collider.transform.GetSiblingIndex() == 2 && !PW_References.Access.objectReferences.gameAnim.IsInTransition(0))
				{
					PW_References.Access.objectReferences.gameAnim.SetTrigger ("CMzoomIn");
				}

				else if(hitInfo.collider.transform.GetSiblingIndex() == 3 && !PW_References.Access.objectReferences.gameAnim.IsInTransition(0))
				{
					PW_References.Access.objectReferences.gameAnim.SetTrigger ("DMzoomIn");
				}
			}

			StartCoroutine ( OnSelected() );
		}
	}

	IEnumerator OnSelected()
	{
		//PW_References.Access.userInterfaces.gameUI.transform..gameObject.SetActive (false);
		PW_References.Access.userInterfaces.ToGameplay (true);

		yield return new WaitForSeconds (1f);
		PW_References.Access.userInterfaces.gameDisplay.SetActive (true);
		PW_References.Access.machineGroups.OnFocusedMachine.onSelected = true;
		PW_References.Access.machineGroups.RefreshChipsDisplay ();
		PW_References.Access.userInterfaces.chips [0].transform.parent.GetComponent<Toggle> ().isOn = true;
		//OnFocusedMachine.cubeChecker.Statictify (false);
	}

	public void ChangeMachine(bool next)
	{
		if( PW_References.Access.machineGroups.machinePrefabs.Exists (x => x.machineInstance.mCollider.enabled == true) )
		{
			int curIndex = PW_References.Access.machineGroups.machinePrefabs.FindIndex (x => x.machineInstance.mCollider.enabled == true);
			int mCount = PW_References.Access.machineGroups.machinePrefabs.Count;

			if(next)
			{
				if(curIndex != mCount - 1)
				{
					//Check if change button will show or hide.
					if( curIndex.Equals(mCount - 2) ) { PW_References.Access.userInterfaces.nextButton.SetActive (false); }
					if( curIndex.Equals(0) ) { PW_References.Access.userInterfaces.prevButton.SetActive (true); }

					int newIndex = curIndex + 1; //Change the active or focused machine!
					ChangingMachine(curIndex, newIndex);
				}
			}

			else
			{
				if(curIndex != 0)
				{
					//Check if change button will show or hide.
					if( curIndex.Equals(mCount - 1) ) { PW_References.Access.userInterfaces.nextButton.SetActive (true); }
					if( curIndex.Equals(1) ) { PW_References.Access.userInterfaces.prevButton.SetActive (false); }

					int newIndex = curIndex - 1; //Change the active or focused machine!
					ChangingMachine(curIndex, newIndex);
				}
			}
		}

		else
		{
			Debug.LogWarning ("BugRepport: " + "for some reason, all machine collider is disable. This should not happen.");
		}
	}

	private void ChangingMachine(int curIndex, int newIndex)
	{
		//Diable the collider for the current machine!
		PW_References.Access.machineGroups.machinePrefabs [curIndex].machineInstance.mCollider.enabled = false;

		string fromName = PW_References.Access.machineGroups.machinePrefabs [curIndex].name;
		string toName = PW_References.Access.machineGroups.machinePrefabs [newIndex].name;
		PW_References.Access.objectReferences.gameAnim.SetTrigger(fromName + "To" + toName);

		//Change machine name on display.
		PW_References.Access.machineGroups.machinePrefabs [newIndex].machineInstance.mCollider.enabled = true;
		PW_References.Access.userInterfaces.machineText.text = toName + " Machine";

		PW_References.Access.camScripts.raycastEnabled = false;
	}
}