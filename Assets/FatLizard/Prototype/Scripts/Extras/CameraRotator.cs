using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraRotator : MonoBehaviour
{
	public float lerp = 7f;

	void Update () 
	{
		transform.Rotate (Vector3.up * Time.deltaTime * lerp);
	}
}
