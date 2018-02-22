using UnityEngine;
using System.Collections;

public class CustomURL : MonoBehaviour
{
	[Header("GMAIL EMAIL INFO")]

	public string email = "";
	public string subject = "";

	[TextArea(1, 3)]
	public string message = "";

	void Awake()
	{
		//Debug.LogError ();
	}

	public void OpenThisURL(string url)
	{
		Application.OpenURL (url);
		//CustomAdmob.Access.Admob_ShowIntertitial ();
	}

	public void EmailOnGmail()
	{
		string[] initial = subject.Split (new char[] { ' ' });
		string customSubject = initial [0];

		for(int index = 1; index < initial.Length; index++)
		{
			customSubject += "+" + initial [index];
		}

		string shareURL = "https://mail.google.com/mail/u/0/?view=cm&su=" + customSubject + "&to=" + email + "&body=" + message + "&fs=1&tf=1";

		Application.OpenURL (shareURL);
		//CustomAdmob.Access.Admob_ShowIntertitial ();
	}

	public void ShareOnGooglePlus()
	{
		string shareURL = "https://plus.google.com/share?url=https://play.google.com/store/apps/details?id=" 
			+ Application.identifier;

		Application.OpenURL (shareURL);
		//CustomAdmob.Access.Admob_ShowIntertitial ();
	}

	[Header("FACEBOOK SHARE INFO")]
	public string title = "";

	public void ShareOnFacebook()
	{
		string[] initial = title.Split (new char[] { ' ' });
		string customTitle = "&title=" + initial [0];

		for(int index = 1; index < initial.Length; index++)
		{
			customTitle += "+" + initial [index];
		}
		
		string shareURL = "http://www.facebook.com/share.php?u=https://play.google.com/store/apps/details?id=" 
			+ Application.identifier + customTitle;

		Application.OpenURL (shareURL);
		//CustomAdmob.Access.Admob_ShowIntertitial ();
	}

	[Header("TWEETER SHARE INFO")]
	public string content = "";
	public bool useDefault = true;

	public void ShareOnTweeter()
	{
		string[] initial = content.Split (new char[] { ' ' });
		string customContent = initial [0];

		for(int index = 1; index < initial.Length; index++)
		{
			customContent += "%20" + initial [index];
		}

		if(useDefault)
		{
			customContent = "Now available at Google Play! Download Now for FREE. Please share!";
		}

		string shareURL = "https://twitter.com/intent/tweet?text=" + customContent 
			+ "&source=sharethiscom&related=BytesCrafter&via=BytesCrafter&url=" 
			+ "https://play.google.com/store/apps/details?id=" + Application.identifier;

		Application.OpenURL (shareURL);
		//CustomAdmob.Access.Admob_ShowIntertitial ();
	}

	public void RateThisGame()
	{
		Application.OpenURL (Application.identifier);
	}
}