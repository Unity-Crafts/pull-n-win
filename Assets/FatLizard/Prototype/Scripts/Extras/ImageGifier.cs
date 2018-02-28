using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using System.Collections.Generic;

[RequireComponent(typeof(Image))]
public class ImageGifier : MonoBehaviour
{
	[Header("Settings")]
	public float speed = 1f;

	[Header("Animation Clips")]
	public List<Sprite> sprites;

	[Header("Required!")]
	public Image image = null;

	private int curFrame = 0;
	private float timer = 0f;
	void Update()
	{
		if(sprites != null)
		{
			timer += Time.deltaTime * speed;

			if(timer > 1f/System.Convert.ToSingle(sprites.Count))
			{
				if(curFrame < sprites.Count - 1)
				{
					curFrame += 1;
				}

				else
				{
					curFrame = 0;
				}

				image.sprite = sprites [curFrame];
				timer = 0f;
			}
		}
	}
}