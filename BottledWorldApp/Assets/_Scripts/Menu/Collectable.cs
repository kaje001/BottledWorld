using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Collectable {

	int level;
	bool collected = false;

	public Collectable(int level){
		this.level = level;
	}

	public void SetCollected(bool b){
		collected = b;
	}

	public bool GetCollected(){
		return collected;
	}

	public int GetLevel() {
		return level;
	}

}
