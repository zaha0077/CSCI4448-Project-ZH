using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : Entity {

	//Vertical movement parameters.
	private float gravity_;
	public bool in_air_;

	//Test for free space for horizontal movement.
	bool checkPosition(float speed){
		Vector3 direction;
		if (speed < 0.0f) {
			direction = Vector2.left;
		} else {
			direction = Vector2.right;
		}
		Debug.DrawRay (this.transform.position, direction * 0.08f);
		return !Physics2D.Raycast (this.transform.position, direction, 0.14f);
	}

	//Movement behavior
	public override void Move(){
		Vector3 temp = this.transform.position;
		float hspeed = Input.GetAxis ("Horizontal") * getSpeed ();

		if (checkPosition(hspeed)){
			temp.x += hspeed;
			}

		if (Input.GetButtonDown ("Jump") && !in_air_) {
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
		in_air_ = true;
		gravity_ = 0.0f;
		setSpeed (0.04f);
	}
	
	// Update is called once per frame
	void Update () {
		if (in_air_ && gravity_ < 0.08f) {
			gravity_ += 0.01f;
		}

		//Apply gravity if we walk off a platform.
		if ((!in_air_) && (!Physics2D.Raycast (this.transform.position, Vector2.down, 0.18f))) {
			in_air_ = true;
		}

		Move ();

	}

	//returns true if the opposing object is below the specified coordinates.
	bool getVerticalRelative(Collision2D col){
		return col.contacts[0].normal.y == 1.0f;
	}

	//Collision detection.
	void OnCollisionEnter2D(Collision2D col){
		//Solid collisions
		if (col.gameObject.GetComponents<Solid>() != null) {
			Vector3 temp = this.transform.position;

			if (getVerticalRelative(col)) { //if there is a solid object below, stand on it.
				in_air_ = false;
				gravity_ = 0.0f;
				temp.y = col.transform.position.y + 0.16f;

			}
			else if (in_air_){ //if there is a solid object above, bump against it.
				gravity_ = 0.0f;
				temp.y = col.transform.position.y - 0.08f;
			}
			this.transform.position = temp;
		}

		//Enemy collision.
		if (col.gameObject.GetComponent("Enemy") != null) {
			Destroy (this.gameObject);
		}
	}

	//Reduces clipping.
	void OnCollisionStay2D(Collision2D col){
		Vector3 temp = this.transform.position;
		temp.y = col.transform.position.y + 0.16f;
	}
}
