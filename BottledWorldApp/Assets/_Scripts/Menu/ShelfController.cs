using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class ShelfController : MonoBehaviour, IPointerDownHandler, IDragHandler, IPointerUpHandler{

	[SerializeField] float lerpSpeedDrag = 15f*20;

	[SerializeField] MenuController menuControll;

	Vector3 resetPos;
	Vector3 targetPos;
	float lerpSpeed;
	public bool isDraged = false;
	GameObject overObject = null;

	float timeNextPosition = 0;

	float touchDif = 0f;
	float lastTouchY;

	// Use this for initialization
	void Start () {
		targetPos = transform.localPosition;
        if(LastGameData.Instance.biom == 1)
        {
            Debug.Log("biom1");
            targetPos = new Vector3(targetPos.x, 0.89f, targetPos.z);
        }
        else if (LastGameData.Instance.biom == 2)
        {
            targetPos = new Vector3(targetPos.x, 1.98f, targetPos.z);
        }
        lerpSpeed = lerpSpeedDrag;
        //transform.localPosition = targetPos;
    }
	
	// Update is called once per frame
	void Update () {
		targetPos -= new Vector3(0f,touchDif,0f);
		//Debug.Log(touchDif);
		if (targetPos.y > -0.21f && targetPos.y < 1.98f) {
			
		} else if (targetPos.y < -0.21f) {
			targetPos = new Vector3(targetPos.x,-0.21f,targetPos.z);
		} else if (targetPos.y > 1.98f) {
			targetPos = new Vector3(targetPos.x,1.98f,targetPos.z);
		}


		transform.localPosition = Vector3.Lerp (transform.localPosition, targetPos, lerpSpeed * Time.deltaTime);
	}

	public void OnPointerDown(PointerEventData data){
		if (menuControll.custom) {
			return;
		}
		//Debug.Log ("Start Drag");
		targetPos = transform.localPosition;
		isDraged = true;
		lerpSpeed = lerpSpeedDrag;

		lastTouchY = data.position.y;
	}



	public void OnDrag(PointerEventData data){
		if (menuControll.custom) {
			return;
		}

		//Vector3 touchPointinWorld = Camera.main.ScreenToWorldPoint (new Vector3 (data.position.x, data.position.y, 0f));
		touchDif = (lastTouchY - data.position.y)/100f;
		lastTouchY = data.position.y;

		//transform.position = Vector3.Lerp (transform.position, touchPointinWorld, Time.deltaTime);
		//targetPos = touchPointinWorld;


	}


	public void OnPointerUp(PointerEventData data){
		if (menuControll.custom) {
			return;
		}
		touchDif = 0f;
		isDraged = false;

	}


}
