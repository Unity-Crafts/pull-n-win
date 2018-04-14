// FX Quest 0.4.5
//
// Author:	Gold Experience Team (http://www.ge-team.com)
// Details:	https://www.assetstore.unity3d.com/en/#!/content/21073
// Support:	geteamdev@gmail.com
//
// Please direct any bugs/comments/suggestions to support e-mail.

#region Namespaces

using UnityEngine;
using System.Collections;

#endregion // Namespaces

// ######################################################################
// FXQ_RotateShapeParticle class
// Rotates gameObject around its own pivot.
// ######################################################################

public class FXQ_RotateShapeParticle : MonoBehaviour
{

	// ########################################
	// Variables
	// ########################################

	#region Variables

	// Start Rotation
	public Vector3 m_StartRotation;         // Euler angles when this script starts.

	// Start Rotation overtime
	public Vector3 m_RotationOvertime;          // Rotation to rotate this object overtime.

	#endregion // Variables

	// ########################################
	// MonoBehaviour Functions
	// http://docs.unity3d.com/ScriptReference/MonoBehaviour.html
	// ########################################

	#region MonoBehaviour

	// Start is called on the frame when a script is enabled just before any of the Update methods is called the first time.
	// http://docs.unity3d.com/ScriptReference/MonoBehaviour.Start.html
	void Start()
	{
		// Set start local rotation
		transform.localEulerAngles = m_StartRotation;
	}

	// Update is called every frame, if the MonoBehaviour is enabled.
	// http://docs.unity3d.com/ScriptReference/MonoBehaviour.Update.html
	void Update()
	{
		// Rotate around local pivot overtime.
		transform.localEulerAngles = transform.localEulerAngles + (m_RotationOvertime * Time.deltaTime);
	}

	#endregion // MonoBehaviour
}
