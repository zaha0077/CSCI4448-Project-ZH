using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Floater : Enemy {

	private bool cooldown_ = false; //Used to prevent colliding with the same wall more than once.
	private int bounceticks_ = -1;

	public override void Move(){
		Vector3 temp = this.transform.position;
		temp.x += getSpeed (); 
		this.transform.position = temp;
	}

	//Functions from Unity's MonoBehavior class.
	// Initialization
	void Start () {
		setSpeed (0.01f);
		hp_ = 40;
	}

	// Code executed every frame.
	void Update () {
		Move ();
		commonBehavior ();
		if (bounceticks_ > -1) {
			if (bounceticks_ == 0) {
				cooldown_ = false;
			}
			bounceticks_--;
		}
	}

	//Collision Detection
	void OnCollisionEnter2D(Collision2D col){
		//Vector3 temp = this.transform.position;
		if (col.gameObject.GetComponent<Solid>() != null && !cooldown_) {
			bounceticks_ = hurt_max_;
			cooldown_ = true;
			setSpeed (-getSpeed());
			Debug.Log ("foo " + getSpeed().ToString());
			//temp.x = col.transform.position.x - (0.16f * direction_);
		}
		//this.transform.position = temp;
	}

}
