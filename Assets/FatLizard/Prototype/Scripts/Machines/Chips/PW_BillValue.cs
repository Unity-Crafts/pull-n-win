using UnityEngine;
using System.Collections;

public class PW_BillValue : MonoBehaviour
{
	public int amount;

	[Header("References")]
	[Range(0.49f, 2.0f)]
	public float restWait = 0.75f;
	public Rigidbody rigidBody = null;

	void OnValidate()
	{
		if(rigidBody == null)
		{
			rigidBody = GetComponent<Rigidbody> ();
		}
	}

	IEnumerator Start()
	{
		yield return new WaitForSeconds ( restWait );
		StartCoroutine ( Resting() );
	}

	IEnumerator Resting()
	{
		yield return new WaitForSeconds (restWait);
		rigidBody.isKinematic = true;
		rigidBody.useGravity = false;
	}
}