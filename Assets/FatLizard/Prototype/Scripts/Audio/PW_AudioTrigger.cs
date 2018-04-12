using UnityEngine;
using System.Collections;

public class PW_AudioTrigger : MonoBehaviour
{
	private AudioSource audioSource = null;

	void Start()
	{
		audioSource = GetComponent<AudioSource> ();
	}

	void OnTriggerEnter (Collider col)
	{
		audioSource.Play ();
	}
}
