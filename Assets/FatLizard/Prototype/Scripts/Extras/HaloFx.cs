using UnityEngine;
using System.Collections;
using System.Collections.Generic;

[RequireComponent( typeof(Behaviour) )]
public class HaloFx : MonoBehaviour
{
	public float frequency = 2.0f;
	public Color startColor = Color.white;
	public Color endColor = Color.white;
	public Behaviour halo = null;

	private bool increment = true;
	private float timer = 0f;

	void OnValidate()
	{
		if(halo == null)
		{
			halo = (Behaviour)GetComponent("Halo");
		}
	}

	void Update ()
	{
		if(increment)
		{
			if(timer < frequency)
			{
				timer += Time.deltaTime;
			}

			else
			{
				increment = false; 
			}
		}

		else
		{
			if(timer > 0)
			{
				timer -= Time.deltaTime;
			}

			else
			{
				increment = true; 
			}
		}

		//use timer for halo
		RenderSettings.haloStrength = frequency;
	}
}
