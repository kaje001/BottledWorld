using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class TouchCon : MonoBehaviour, IPointerDownHandler, IDragHandler, IPointerUpHandler {

	public PlayerController playCon;
	float lastY;
	int screenY;
	float lastX;
	int screenX;
	

	void Start () {
		screenX = Screen.width;
		screenY = Screen.height;
		
	}
	
	
	public void OnPointerDown(PointerEventData data)
	{
		lastX = data.position.x;
		lastY = data.position.y;
    }
	
	 public void OnPointerUp(PointerEventData data)
    {
    }
	
	
	public void OnDrag(PointerEventData data)
	{
		float x = data.position.x;
		float y = data.position.y;

		Vector2 drag = new Vector2 ((x - lastX)/screenX, (y - lastY)/screenY);
		float length = drag.magnitude;
		if (Mathf.Abs(drag.y) > Mathf.Abs(drag.x)) {
			if (x > screenX / 2f) {
				if (drag.y > 0) {
					length = -length;
				}
			} else {
				if (drag.y < 0) {
					length = -length;
				}
			}
		} else {
			if (drag.x > 0) {
				length = -length;
			}
		}
		//playCon.RotateWorld(length);
		

		lastX = x;
		lastY = y;
    }
	
}
