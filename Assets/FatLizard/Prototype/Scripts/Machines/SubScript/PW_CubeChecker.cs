﻿using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class PW_CubeChecker : MonoBehaviour
{
	[Range(0f, 1f)]
	public float percentCoG = 0.49f;

	[Header("CUBE RIGIDBODIES")]
	public List<Rigidbody> cubePhysics = new List<Rigidbody> ();

	//Make a list of object.
	[HideInInspector] public List<GameObject> cubeChecked = new List<GameObject> ();

	//Current Rotation before scen start.
	[HideInInspector] public Vector3[] cubesPosition = new Vector3[3];

	//Current Rotation before scen start.
	[HideInInspector] public Quaternion[] cubesRotation = new Quaternion[3];

	//Random rotation variable for all axis. - Done!
	[HideInInspector] public float[] rotRandom = new float[4]{0F, 90F, 180F, 270F};

	[HideInInspector] public bool allCubePassed = false;
	//[HideInInspector]
	public bool cubeFallDown = false;

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

	/// Raises the trigger enter event when a cube pass.
	void OnTriggerEnter(Collider collider)
	{
		if(collider.tag == "ColorCube")
		{
			if( !cubeChecked.Exists(x => x.name == collider.name) ) 
			{
				if( cubeChecked.Count <= 3 ) //Follow max cube list.
				{
					cubeChecked.Add(collider.gameObject);
				}
			}

			if(cubeChecked.Count > 0)
			{
				cubeFallDown = true;
			}

			if(cubeChecked.Count == 3)
			{
				allCubePassed = true;
			}
		}
	}

	public bool isAllCubesSteady()
	{
		return (cubePhysics.FindAll (x => x.IsSleeping () == true).Count == 3);
	}

	public Vector3 sizes = Vector3.zero;

	void Update()
	{
		if (machine.onSelected)
		{
			Statictify (false); 

			if(allCubePassed)
			{
				if(cubeChecked.Count == 3)
				{
					machine.CheckMachine ();
					cubeChecked = new List<GameObject> ();
					allCubePassed = false;
				}
			}
		}

		else
		{
			Statictify (true);
		}
	}

	/// <summary>
	/// Statictify the all Cubes or make it not affected by gravity.
	/// </summary>
	/// <param name="staticCubes">If set to <c>true</c> static cubes.</param>
	public void Statictify (bool staticCubes) 
	{
		foreach(Rigidbody rBody in cubePhysics)
		{
			if(rBody.isKinematic != staticCubes)
			{
				rBody.isKinematic = staticCubes;
			}
		}
	}

	public void GetAllReference()
	{
		Rigidbody[] rigidBodies = GetComponentsInChildren<Rigidbody> ();
		cubePhysics = new List<Rigidbody> (rigidBodies.Length);
		cubePhysics.AddRange ( rigidBodies );

		cubesPosition = new Vector3[cubePhysics.Count];
		cubesRotation = new Quaternion[cubePhysics.Count];

		for (int index = 0; index < cubePhysics.Count; index++)
		{
			cubesPosition[index] = cubePhysics[index].transform.position;
			cubesRotation[index] = cubePhysics[index].transform.rotation;
		}

		if(mInstance == null)
		{
			mInstance = transform.parent.GetComponent<PW_MInstance> ();
		}
	}

	/// <summary>
	/// Randomize center of gravity for every cubes.
	/// </summary>
	public void RandomCubeTransform () 
	{
		for(int index = 0; index < transform.childCount; index++)
		{
			float cogMax = cubePhysics[index].transform.lossyScale.x * percentCoG;

			//Randomize cube center of gravity.
			float cogX = Random.Range (-cogMax, cogMax);
			float cogY = Random.Range (-cogMax, cogMax);
			float cogZ = Random.Range (-cogMax, cogMax);
			cubePhysics[index].centerOfMass = new Vector3(cogX, cogY, cogZ);

			//Set to their default positions
			cubePhysics[index].transform.position = cubesPosition[index];

			//Randomized cube rotation by 45 degrees in 3 axis.
			float quatX = cubesRotation [index].x + rotRandom [Random.Range (0, 3)];
			float quatY = cubesRotation [index].y + rotRandom [Random.Range (0, 3)];
			float quatZ = cubesRotation [index].z + rotRandom [Random.Range (0, 3)];
			cubePhysics[index].transform.localRotation = Quaternion.Euler (quatX, quatY, quatZ);
		}
	}

	/// <summary>
	/// Check every cube if what is the result through the child index.
	/// </summary>
	/// <returns>The cube result in integer.</returns>
	public int[] CheckCubeResult()
	{
		int[] cubesResult = new int[6];
		for(int index = 0; index < machine.cubeChecker.transform.childCount; index++)
		{
			RaycastHit hitCheck = new RaycastHit();
			if(Physics.Raycast (transform.GetChild(index).position, Vector3.up, out hitCheck))
			{
				if(hitCheck.transform.IsChildOf(transform.GetChild(index)))
				{
					cubesResult[hitCheck.collider.transform.GetSiblingIndex ()] += 1;
					Debug.DrawLine(transform.GetChild(index).position, hitCheck.point, Color.red, 1f);
					//Debug.Log (index + " - " + hitCheck.collider.transform.GetSiblingIndex ());
				}
			}
		}
		//Debug.Log ("C0-" + cubesResult[0] + " " + "C1-" + cubesResult[1] + " " + "C2-" + cubesResult[2] + " " + "C3-" + cubesResult[3] + " " + "C4-" + cubesResult[4] + " " + "C5-" + cubesResult[5] );
		return cubesResult;
	}
}