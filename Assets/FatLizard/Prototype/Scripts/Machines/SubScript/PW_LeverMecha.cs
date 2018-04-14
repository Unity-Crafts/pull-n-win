/// <summary>
/// This script is attached into the leverObject.
/// * Detect mouse / touch input if currently on the lever.
/// * Now we will try to sync the angular value of the pivot.
/// * This will also interact on the paddle default angle.
/// * Aside from that this script is responsible for sending event
///   for other related script whether the lever is down or up.
/// </summary>

using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class PW_LeverMecha : MonoBehaviour
{
	[Header("LEVER ANGLE CLAMP")]
	[Range(0f, 1f)] public float ioValue = 0f;

	[Header("LEVER SETTINGS")]
	public float springs = 2.9f;
	public float amplitude = 1.2f;
	public float maxValue = 25f;
	public float minLever = 25f;
	public float maxLever = 172f;

	[Header("OBJECT REFERENCE")]
	public Collider handleCollider = null;
	public Transform pivotLever = null;

	private float damping = 7f;
	private Transform handleObj = null;

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

	public void GetAllReference ()
	{
		if(Application.isEditor)
		{
			if(handleCollider == null)
			{
				handleCollider = GetComponentInChildren<Collider> ();
			}

			Rotation ();
		}
	}

	public float maxDamp = 0.29f;

	//Mouse or Touch screenpont. - Good Flexible.
	private bool currentlyLeveling = false;
	void Update()
	{
		Rotation ();

		if (machine == null)
			return;

		if(handleObj != null)
		{
			if(!machine.cubeChecker.cubeFallDown && ioValue > maxDamp)
			{
				machine.cubeChecker.cubeFallDown = true;
				currentlyLeveling = true;
			}
		}

		if(currentlyLeveling)
		{
			if(ioValue < 1f)
			{
				ioValue = Mathf.Clamp (ioValue + (springs * Time.deltaTime), minLever, maxLever);
			}

			else
			{
				machine.cubeChecker.cubeFallDown = false;
				currentlyLeveling = false;
			}
		}

		//UPDATE CUBE RELEASE ROTATION FROM THE HANDLEBAR VALUE!
		machine.paddleSet.Rotate(ioValue);

		if(!machine.onSelected || !machine.onReadyPlay) { return; }

		Vector3 castPos = Vector3.zero;

		if (Application.isMobilePlatform)
		{
			if(Input.touchCount > 0)
			{
				castPos = Input.GetTouch(0).position; 
				Ray rayInfo = Camera.main.ScreenPointToRay(castPos);
				RaycastHit hitInfo = new RaycastHit();

				if(Input.GetTouch(0).phase == TouchPhase.Began)
				{
					OnInteractStart (rayInfo, hitInfo);
				}

				else if(Input.GetTouch(0).phase == TouchPhase.Moved)
				{
					OnInteractMoved (castPos, rayInfo, hitInfo);
				}

				else if(Input.GetTouch(0).phase == TouchPhase.Ended)
				{
					handleObj = null;
				}
			}
		}

		else
		{
			castPos = Input.mousePosition;
			Ray rayInfo = Camera.main.ScreenPointToRay(castPos);
			RaycastHit hitInfo = new RaycastHit();

			if(Input.GetMouseButtonDown(0))
			{
				OnInteractStart (rayInfo, hitInfo);
			}

			else if(Input.GetMouseButton(0))
			{
				OnInteractMoved (castPos, rayInfo, hitInfo);
			}

			else if(Input.GetMouseButtonUp(0))
			{
				handleObj = null;
			}
		}
	}

	void OnInteractStart(Ray rayInfo, RaycastHit hitInfo)
	{
		if (Physics.Raycast (rayInfo, out hitInfo))
		{
			if(hitInfo.collider != null)
			{
				if(hitInfo.collider.GetInstanceID().Equals( handleCollider.GetInstanceID() ))
				{
					if(machine.colorPicker.betHolder == null)
					{
						PW_References.Access.userInterfaces.alertDialog.Show("Its is not allowed to pull the lever if you have no bet placed.");
						PW_CustomEvents.OnNotificationEvents (Notification.NoChipsBet); return;
					}

					else
					{
						handleObj = hitInfo.transform; //Debug.Log ("OnDragTrue!");
						Debug.DrawLine (rayInfo.origin, hitInfo.point, Color.green, 3f, true);
					}
				}
			}
		}
	}

	void OnInteractMoved(Vector3 castPos, Ray rayInfo, RaycastHit hitInfo)
	{
		if (Physics.Raycast (rayInfo, out hitInfo))
		{
			if(hitInfo.collider != null)
			{
				if(hitInfo.collider.GetInstanceID().Equals( handleCollider.GetInstanceID() ))
				{
					Debug.DrawLine (rayInfo.origin, hitInfo.point, Color.white, 1f, true);
				}
			}
		}

		if(handleObj != null && machine.colorPicker.betHolder != null)
		{
			Vector3 objPos = Camera.main.WorldToScreenPoint(handleObj.position);

			if( (objPos.y - damping) > castPos.y )
			{
				amplitude = (objPos.y + damping) - castPos.y;
				float sensitivity = ( amplitude / maxValue ); //Debug.Log ("Increasing");
				ioValue = Mathf.Clamp01(ioValue + (Time.smoothDeltaTime * sensitivity));
			}

			if( (objPos.y + damping) < castPos.y )
			{
				//amplitude = castPos.y - (objPos.y - damping);
				//float sensitivity = ( amplitude / maxValue ); //Debug.Log ("Decreasing");
				//ioValue = Mathf.Clamp01(ioValue - (Time.smoothDeltaTime * sensitivity));
			}
		}
	}

	void Rotation()
	{
		if(pivotLever != null)
		{
			pivotLever.eulerAngles = new Vector3
			(
				(112f * ioValue) - 90f, pivotLever.eulerAngles.y, pivotLever.eulerAngles.z
			);
		}
	}
}