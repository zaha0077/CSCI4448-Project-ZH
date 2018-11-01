using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : Entity {

	//Vertical movement parameters.
	private float gravity_;
	public bool in_air_;

	//Movement behavior
	public override void Move(){
		Vector3 temp = this.transform.position;
		float hspeed = Input.GetAxis ("Horizontal") * getSpeed ();
		temp.x += hspeed;

		if (Input.GetButtonDown ("Jump")) {
			in_air_ = true;
			gravity_ = -0.15f;
		}

		if (in_air_) {
			temp.y -= gravity_;
		}

		this.transform.position = temp;
	}
		
	// Use this for initialization
	void Start () {
		in_air_ = false;
		gravity_ = 0.1f;
		setSpeed (0.05f);
	}
	
	// Update is called once per frame
	void Update () {
		if (in_air_ && gravity_ < 0.1f) {
			gravity_ += 0.01f;
		}
		Move ();
	}

	//Collision detection.
	void OnCollisionEnter2D(Collision2D col){
		if (col.gameObject.GetComponents<Solid>() != null) {
			Debug.Log ("Ping!");
			gravity_ = 0.0f;
		}
	}
}
