using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class TouchCon : MonoBehaviour, IPointerDownHandler, IDragHandler, IPointerUpHandler {

	public PlayerController playCon;
	int lastX;
	int screenX;
	

	void Start () {
		screenX = Screen.width;
		
	}
	
	
	public void OnPointerDown(PointerEventData data)
    {
		lastX = (int) data.position.x;
    }
	
	 public void OnPointerUp(PointerEventData data)
    {
    }
	
	
	public void OnDrag(PointerEventData data)
    {
		int x = (int) data.position.x;
		float rot = x - lastX;
		playCon.RotateWorld(rot/screenX);
		
		
		lastX = x;
    }
	
}
