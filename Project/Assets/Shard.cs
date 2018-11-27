using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Shard : Entity {
	private float direction_; //Direction of travel, in radians.
	private int lifetime_ = 30; //Ticks this object stays alive.

	public void setDirection(float dir){ //takes a direction in degrees and sets the direction field to the radian equivalent
		direction_ = dir * (Mathf.PI/180.0f);
		Debug.Log ("foo");
	}

	public override void Move(){
		Vector3 temp = this.transform.position;

		//Move in the angle specified by direction_.
		temp.x += getSpeed () * Mathf.Cos (direction_);
		temp.y += getSpeed () * Mathf.Sin (direction_);

		this.transform.position = temp;
	}

	// Overrides of functions from Unity's MonoBehavior class.
	// Initialization
	void Start () {
		setSpeed(0.01f);
	}
	
	// Code executed every frame.
	void Update () {
		Move ();
		this.transform.Rotate (0,0,22.5f);
		if (lifetime_ == 0) {
			Destroy (this.gameObject);
		}
		lifetime_--;
	}
}
