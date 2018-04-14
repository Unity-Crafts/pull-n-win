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
// FXQ_TargetAnimatorEvent class
// Stores behavior information when particle played
// 
// Note this class is used in FXQ_2D_Demo.UpdateTargetAnimator() function.
// ######################################################################
public class FXQ_TargetAnimatorEvent : MonoBehaviour
{

	// ########################################
	// Variables
	// ########################################

	#region Variables

	// Type of particle
	public enum ParticleEvent
	{
		None,
		Attack,
		UI
	}

	public Animator m_TargetAnimator = null;
	public ParticleEvent m_ParticleEvent = ParticleEvent.None;

	#endregion // Variables

}
