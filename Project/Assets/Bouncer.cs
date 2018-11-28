using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//This enemy bounces diagonally off walls.
public class Bouncer : Enemy {
	private bool cooldown_ = false; //Used to prevent colliding with the same wall more than once.
	private int bounceticks_ = -1;
	private float direction_ = 45f * (Mathf.PI / 180f);
	private float hspeed_;
	private float vspeed_;

	public override void Move(){
		Vector3 temp = this.transform.position;
		temp.x += hspeed_;
		temp.y += vspeed_;
		this.transform.position = temp;
	}

	//Change direction upon collision.
	public void adjustDirection(){
		Vector3 temp = this.transform.position;
		if (Physics2D.Raycast (temp, Vector2.left, 0.16f, LayerMask.GetMask ("Solids")) || Physics2D.Raycast (temp, Vector2.right, 0.16f, LayerMask.GetMask ("Solids")) ) { //if the collision was horizontal, flip horizontal direction
			hspeed_ = -hspeed_;
		}
		if (Physics2D.Raycast (temp, Vector2.up, 0.16f, LayerMask.GetMask ("Solids")) || Physics2D.Raycast (temp, Vector2.down, 0.16f, LayerMask.GetMask ("Solids")) ) { //if the collision was vertical, flip vertical direction.
			vspeed_ = -vspeed_;
		}
	}

	//Functions from Unity's MonoBehavior class.
	// Initialization
	void Start () {
		direction_ = direction_ + ((Mathf.PI/2) * Random.Range (1, 4));
		hspeed_ = 0.02f * Mathf.Cos (direction_);
		vspeed_ = 0.02f * Mathf.Sin (direction_);
		hp_ = 60;
	}

	// Code executed every frame.
	void Update () {
		Move ();
		this.transform.Rotate (0,0,22.5f);
		manageHealth ();
		if (bounceticks_ > -1) {
			if (bounceticks_ == 0) {
				cooldown_ = false;
			}
			bounceticks_--;
		}
	}

	//Collision Detection
	void OnCollisionEnter2D(Collision2D col){
		if (col.gameObject.GetComponent<Solid>() != null && !cooldown_) {
			adjustDirection ();
			bounceticks_ = hurt_max_;
			cooldown_ = true;
		}
	}
}
