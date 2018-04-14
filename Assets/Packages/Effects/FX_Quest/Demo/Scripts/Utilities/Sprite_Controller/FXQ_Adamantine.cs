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
// FXQ_Adamantine class
// Controls Adamantine sprite animation.
// ######################################################################

public class FXQ_Adamantine : MonoBehaviour
{

	// ########################################
	// Variables
	// ########################################

	#region Variables

	Animator anim = null;

	#endregion // Variables

	// ########################################
	// MonoBehaviour Functions
	// http://docs.unity3d.com/ScriptReference/MonoBehaviour.html
	// ########################################

	#region MonoBehaviour

	// Awake is called when the script instance is being loaded.
	// http://docs.unity3d.com/ScriptReference/MonoBehaviour.Awake.html
	void Awake()
	{
		anim = GetComponent<Animator>();
	}

	// Start is called on the frame when a script is enabled just before any of the Update methods is called the first time.
	// http://docs.unity3d.com/ScriptReference/MonoBehaviour.Start.html
	void Start()
	{
		anim.SetBool("UnderAttack", false);
	}

	// Update is called every frame, if the MonoBehaviour is enabled.
	// http://docs.unity3d.com/ScriptReference/MonoBehaviour.Update.html
	void Update()
	{
		// Reset "UnderAttack" condition to false
		//if(anim.GetBool("UnderAttack")==true)
		//{ 
		//	AnimatorStateInfo pAnimatorStateInfo = anim.GetCurrentAnimatorStateInfo(0);
		//	if(pAnimatorStateInfo.IsName("Hurt"))
		//	{
		//		anim.SetBool("UnderAttack", false);
		//	}
		//}
	}

	#endregion // MonoBehaviour

	// ########################################
	// Animation functions
	// ########################################

	#region Animation

	public void UnderAttack()
	{
		// Play Hurt animaiton
		if (anim == null)
			return;

		AnimatorStateInfo pAnimatorStateInfo = anim.GetCurrentAnimatorStateInfo(0);
		if (pAnimatorStateInfo.IsName("Idle"))
		{
			// Delay play Hurt animation.
			//anim.SetBool("UnderAttack", true);

			// Immediately play Hurt animation
			anim.Play("Hurt");
		}

	}

	#endregion // Animation
}
