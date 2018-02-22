using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class LedOn
{
	public float duration = 0.25F;
	public Color[] onColors;
}

[System.Serializable]
public class LedOff
{
	public bool canTurnOff = true;
	public float duration = 0.25F;
	public Color offColor = Color.grey;
}

public enum LedActtion
{
	ChooseAction, GetRenderer
}

public class LedLight : MonoBehaviour
{
	public bool activated = true;
	public LedActtion action = LedActtion.ChooseAction;

	[Header("SETTINGS")]
	public LedOn ledOn = new LedOn();
	public LedOff ledOff = new LedOff();

	[HideInInspector]
	public Renderer render = null;
	private float timer = 0f;
	private bool isOn = false;
	private int current = 0;

	void OnValidate()
	{
		if(action == LedActtion.GetRenderer)
		{
			render = GetComponent<Renderer>();
			action = LedActtion.ChooseAction;
		}
	}

	void Update()
	{
		if (!activated)
			return;

		if(ledOn.onColors.Length > 0)
		{
			if(timer > 0f)
			{
				timer -= Time.deltaTime;
			}

			else
			{
				if(current >= (ledOn.onColors.Length - 1))
				{
					if(!ledOff.canTurnOff)
					{
						render.material.color = ledOn.onColors [current];
						timer = ledOn.duration; current = 0;
						return;
					}

					if(isOn)
					{
						render.material.color = ledOff.offColor;
						timer = ledOff.duration;
					}

					else
					{
						render.material.color = ledOn.onColors [current];
						timer = ledOn.duration; current = 0;
					}
				}

				else
				{
					if(!ledOff.canTurnOff)
					{
						render.material.color = ledOn.onColors [current];
						timer = ledOn.duration; current += 1;
						return;
					}

					if(isOn)
					{
						render.material.color = ledOff.offColor;
						timer = ledOff.duration;
					}

					else
					{
						render.material.color = ledOn.onColors [current];
						timer = ledOn.duration; current += 1;
					}
				}

				isOn = !isOn;
			}
		}

		//float lerp = Mathf.PingPong(Time.time, duration) / duration;
		//renderer.material.color = Color.Lerp(colorStart, colorEnd, lerp);
	}
}