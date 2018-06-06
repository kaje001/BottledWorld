using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PhysicsController : MonoBehaviour {

	public Slider camHeight;
	public Slider camDist;
	public PhysicMaterial playermat;

	public Text a;
	public Text b;


	// Use this for initialization
	void Start () {
		camHeight.onValueChanged.AddListener(delegate {
			ChangeHeight();
		});
		camDist.onValueChanged.AddListener(delegate {
			ChangeDis();
		});
		a.text = camHeight.value.ToString ();
		b.text = camDist.value.ToString ();
	}
	
	// Update is called once per frame
	void Update () {
		
	}

	public void ChangeHeight(){
		playermat.dynamicFriction = camHeight.value;
		a.text = camHeight.value.ToString ();
	}

	public void ChangeDis(){
		playermat.staticFriction = camDist.value;
		b.text = camDist.value.ToString ();
	}


}
