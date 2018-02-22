using UnityEngine;
using System.Collections;

public class AudioTrigger : MonoBehaviour
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
