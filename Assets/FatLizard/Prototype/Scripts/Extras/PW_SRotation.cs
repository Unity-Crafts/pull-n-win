using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PW_SRotation : MonoBehaviour
{
	public float speed = 3.0f;

	void Update ()
	{
		Vector3 rotation = transform.eulerAngles;
		if(rotation.z >= 360f)
		{
			rotation.z = rotation.z - 360f;
		}
		rotation.z += Time.deltaTime * speed;

		transform.rotation = Quaternion.Euler (rotation);
	}
}