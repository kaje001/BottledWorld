using UnityEngine;
 using System.Collections;
 using UnityEngine.UI;
 
 public class Fade : MonoBehaviour {
    public float FadeRate;
    Image image;
    float targetAlpha = 0f;
     // Use this for initialization
     void Start () {
         image = GetComponent<Image>();
         if(this.image==null)
         {
             Debug.LogError("Error: No image on "+name);
         }
         //targetAlpha = image.color.a;
     }
     
     // Update is called once per frame
     void Update () {
         Color curColor = image.color;
         float alphaDiff = Mathf.Abs(curColor.a-targetAlpha);
         if (alphaDiff>0.0001f)
         {
             curColor.a = Mathf.Lerp(curColor.a,targetAlpha,FadeRate*Time.deltaTime);
             image.color = curColor;
         }
		 
		// Debug.Log("Alpha " + curColor.a+ ", " + alphaDiff + ", " + targetAlpha);
		 
     }
 
     public void FadeOut()
     {
         targetAlpha = 0.0f;
     }
 
     public void FadeIn()
     {
         targetAlpha = 1.0f;
     }
 }