using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public enum EyeView
{
	RenderTexture, CameraFly
}

public class TopViewCam : MonoBehaviour 
{
	public EyeView eyeView = EyeView.RenderTexture;
	public RawImage topView = null;
	public Animator camAnim = null;


	public void ShowTopView(Toggle toggle)
	{
		if(eyeView == EyeView.RenderTexture)
		{
			topView.enabled = toggle.isOn;
		}

		else
		{
			if(toggle.isOn)
			{
				camAnim.SetTrigger ("CamShow");
			}

			else
			{
				camAnim.SetTrigger ("CamHide");
			}
		}
	}
}