using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum Help
{
	Choose, RemoveChildCollider
}

public class RemoveCollider : MonoBehaviour
{
	public Help actions = Help.Choose;

	void OnValidate()
	{
		if(actions == Help.RemoveChildCollider)
		{
			Collider[] cols = transform.GetComponentsInChildren<Collider> ();

			foreach (Collider col in cols)
			{
				Destroy ( col );
			}

			actions = Help.Choose;
		}
	}
}
