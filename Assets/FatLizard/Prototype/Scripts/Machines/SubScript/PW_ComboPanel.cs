
using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using System.Collections.Generic;

[System.Serializable]
public class SpinnerValue
{
	[Range(1, 7)]
	public int oneColor = 1;

	[Range(1, 7)]
	public int twoColor = 1;

	[Range(1, 7)]
	public int threeColor = 1;
}

public class PW_ComboPanel : MonoBehaviour
{
	[Header("REFERENCES")]
	public Text oneColor = null;
	public Text twoColor = null;
	public Text threeColor = null;

	//Put spinner mechanism here.
	void OnValidate()
	{
		oneColor.text = "x" + PW_References.Access.userInterfaces.spinValue.oneColor;

		twoColor.text = "x" + PW_References.Access.userInterfaces.spinValue.twoColor;

		threeColor.text = "x" + PW_References.Access.userInterfaces.spinValue.threeColor;
	}

	void Update()
	{
		if (!oneColor.text.Equals("x" + PW_References.Access.userInterfaces.spinValue.oneColor))
		{
			oneColor.text = "x" + PW_References.Access.userInterfaces.spinValue.oneColor;
		}

		if (!oneColor.text.Equals("x" + PW_References.Access.userInterfaces.spinValue.twoColor))
		{
			twoColor.text = "x" + PW_References.Access.userInterfaces.spinValue.twoColor;
		}

		if (!oneColor.text.Equals("x" + PW_References.Access.userInterfaces.spinValue.threeColor))
		{
			threeColor.text = "x" + PW_References.Access.userInterfaces.spinValue.threeColor;
		}
	}
}