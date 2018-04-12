
using UnityEngine;
using System.Collections.Generic;

public class PW_PaddleSet : MonoBehaviour
{
	public Transform left = null;
	public Transform right = null;

	public void Rotate(float rotation)
	{
		left.eulerAngles = new Vector3 (left.eulerAngles.x, left.eulerAngles.y, -180 + (-117f * rotation));
		right.eulerAngles = new Vector3 (right.eulerAngles.x, right.eulerAngles.y, -117f * rotation);
	}
}