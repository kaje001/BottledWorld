﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class AntController : MonoBehaviour, IPointerDownHandler, IDragHandler, IPointerUpHandler{

	[SerializeField] float lerpSpeedDrag = 15f;
	[SerializeField] float lerpSpeedMove = 0.2f;
	public MenuController menuControll;
	public LayerMask layMask;

	Vector3 resetPos;
	Vector3 targetPos;
	float lerpSpeed;
	public bool isDraged = false;
	public bool isDrop = false;

	float timeNextPosition = 0;

	// Use this for initialization
	void Start () {
		targetPos = transform.position;
		lerpSpeed = lerpSpeedMove;
	}
	
	// Update is called once per frame
	void Update () {
		if (!isDraged && Time.time > timeNextPosition) {
			targetPos = GetRandomPos ();
			timeNextPosition = Time.time + Random.Range (5, 50) / 10f;
			lerpSpeed = lerpSpeedMove;

			isDrop = false;
		}


		if (isDrop) {
			isDraged = false;
		}


		if (isDraged || isDrop) {

			transform.position = Vector3.Lerp (transform.position, targetPos, lerpSpeed * Time.deltaTime);
		} else {
			transform.position = Vector3.MoveTowards (transform.position, targetPos, lerpSpeed * Time.deltaTime);
		}

	}

	public void OnPointerDown(PointerEventData data){
		//Debug.Log ("Start Drag");
		resetPos = transform.position;
		targetPos = resetPos;
		isDraged = true;
		isDrop = false;
		lerpSpeed = lerpSpeedDrag;
	}



	public void OnDrag(PointerEventData data){
		Vector3 touchPointinWorld = Camera.main.ScreenToWorldPoint (new Vector3 (data.position.x, data.position.y, 1.2f));

		//transform.position = Vector3.Lerp (transform.position, touchPointinWorld, Time.deltaTime);
		targetPos = touchPointinWorld;
	}


	public void OnPointerUp(PointerEventData data){
		targetPos = resetPos;
		isDrop = true;
		timeNextPosition = Time.time + 2f;
		Ray ray = Camera.main.ScreenPointToRay(data.position);
		RaycastHit hitInfo;
		if (Physics.Raycast (ray, out hitInfo, 10f, layMask)) {

			menuControll.CheckActivatedObject (hitInfo.collider.gameObject);
		}
	}

	Vector3 GetRandomPos(){
		Vector3 newPos = new Vector3(Random.Range(0,90)/100f-2f,1.1f,Random.Range(0,140)/100f-0.7f);

		return newPos;
	}

}
