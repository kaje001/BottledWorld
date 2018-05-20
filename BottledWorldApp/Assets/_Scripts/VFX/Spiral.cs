using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Spiral : MonoBehaviour {

    public float Radius_1;
    public float Radius_2;
    public float MaxHeight;
    public GameObject Emitter;
    public int Duration;
    public float RotationPerSecond;

    float xPos, yPos, zPos;
    float progress;
    float timePassed = 5;

	float radiusEnd=0.0001f;
	float radiusStart_1;
    float radiusStart_2;

	// Use this for initialization
	void Start () {
        
	}
	
	// Update is called once per frame
	void Update () {
        if (Duration > timePassed)
        {
            timePassed += Time.deltaTime;
            changeRadius();
            calculateXZ();
            calculateY();
            move();
        }
    }

    // reduces radius every frame
	public void changeRadius (){
		float progressRadius;
		progressRadius = Mathf.InverseLerp (0,Duration,timePassed);
		Radius_1 = Mathf.Lerp (radiusStart_1,radiusEnd,progressRadius);
        Radius_2 = Mathf.Lerp(radiusStart_2, radiusEnd, progressRadius);
    }

    // calculates next (next Frame) x and z position of Emitter
    private void calculateXZ()
    {
        progress += RotationPerSecond * Time.deltaTime * 2 * Mathf.PI;
        xPos = Radius_1 * Mathf.Cos(progress);
        zPos = Radius_2 * Mathf.Sin(progress);
    }

    // calculates next (next Frame) z position of Emitter
    private void calculateY()
    {
        yPos = timePassed/Duration * MaxHeight;
    }

    // moves Emitter to calculated position
    private void move()
    {
        Emitter.transform.localPosition = new Vector3(xPos,yPos,zPos);
    }


	//TODO: Play()
	public void PlayParticles(){
		calculateXZ();
		Emitter.transform.localPosition = new Vector3(xPos,0,zPos);
		timePassed = 0;
		radiusStart_1 = Radius_1;
		radiusStart_2 = Radius_2;
	}
}
