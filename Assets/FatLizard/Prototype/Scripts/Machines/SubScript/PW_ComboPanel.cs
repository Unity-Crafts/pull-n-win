
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
	[Header("SPINNERS")]
	public SpinnerValue spinValue = new SpinnerValue();

	[Header("REFERENCES")]
	public Text oneColor = null;
	public Text twoColor = null;
	public Text threeColor = null;

	//Put spinner mechanism here.
	void OnValidate()
	{
		oneColor.text = "x" + spinValue.oneColor;

		twoColor.text = "x" + spinValue.twoColor;

		threeColor.text = "x" + spinValue.threeColor;
	}
}