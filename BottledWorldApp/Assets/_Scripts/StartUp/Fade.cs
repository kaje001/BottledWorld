using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class Fade : MonoBehaviour
{
	public float fadeRate;
	Image image;
	float targetAlpha = 0f;
	// Use this for initialization
	void Start ()
	{
		image = GetComponent<Image> ();
		//Color curColor = image.color;
		//curColor.a = 1.0f;
		//image.color = curColor;
	}
     
	// Update is called once per frame
	void Update ()
	{
		Color curColor = image.color;
		float alphaDiff = Mathf.Abs (curColor.a - targetAlpha);
		if (alphaDiff > 0.0001f) {
			curColor.a = Mathf.Lerp (curColor.a, targetAlpha, fadeRate * Time.deltaTime);
			image.color = curColor;
		}
		 
		// Debug.Log("Alpha " + curColor.a+ ", " + alphaDiff + ", " + targetAlpha);
		 
	}

	public void FadeOut (float time)
	{
		fadeRate = time;
		targetAlpha = 0.0f;
	}

	public void FadeIn (float time)
	{
		fadeRate = time;
		targetAlpha = 1.0f;
	}
}