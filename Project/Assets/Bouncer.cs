using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/**A tricky Enemy that bounces diagonally off of Solid instances.*/
public class Bouncer : Enemy {
	private bool cooldown_ = false; //Used to prevent colliding with the same wall more than once.
	private int bounceticks_ = -1;
	private float direction_ = 45f * (Mathf.PI / 180f);
	private float hspeed_;
	private float vspeed_;

	/**
	* Unique movement behavior for this Enemy.
	*/
	public override void Move(){
		Vector3 temp = this.transform.position;
		temp.x += hspeed_;
		temp.y += vspeed_;
		this.transform.position = temp;
	}

	/**
	 * Called during a collision with a Solid. Adjusts the enemy's current movement direction based on what direction the collision occurred from.
	*/
	public void AdjustDirection(){
		Vector3 temp = this.transform.position;
		//Horizontal bounce
		if (Physics2D.Raycast (temp, Vector2.left, 0.16f, LayerMask.GetMask ("Solids")) || Physics2D.Raycast (temp, Vector2.right, 0.16f, LayerMask.GetMask ("Solids")) ) { //if the collision was horizontal, flip horizontal direction
			hspeed_ = -hspeed_;
		}
		//Vertical bounce
		if (Physics2D.Raycast (temp, Vector2.up, 0.16f, LayerMask.GetMask ("Solids")) || Physics2D.Raycast (temp, Vector2.down, 0.16f, LayerMask.GetMask ("Solids")) ) { //if the collision was vertical, flip vertical direction.
			vspeed_ = -vspeed_;
		}
	}

	/**
	 * Defined in Unity's MonoBehavior class. 
	 * 
	 * MonoBehavior derived classes use this function for instantiation rather than the constructor.
	*/
	void Start () {
		direction_ = direction_ + ((Mathf.PI/2) * Random.Range (1, 4));
		hspeed_ = 0.02f * Mathf.Cos (direction_);
		vspeed_ = 0.02f * Mathf.Sin (direction_);
		hp_ = 60;
	}

	/**
	 * Defined in Unity's MonoBehavior class. 
	 * 
	 * This function is called every frame inside the game.
	*/
	void Update () {
		Move ();
		this.transform.Rotate (0,0,22.5f);
		ManageHealth ();
		if (bounceticks_ > -1) {
			if (bounceticks_ == 0) {
				cooldown_ = false;
			}
			bounceticks_--;
		}
	}

	/**
	 * Defined in Unity's MonoBehavior class. 
	 * 
	 * This function is used in collision detection.
	*/
	void OnCollisionEnter2D(Collision2D col){
		if (col.gameObject.GetComponent<Solid>() != null && !cooldown_) {
			AdjustDirection ();
			bounceticks_ = hurt_max_;
			cooldown_ = true;
		}
	}
}
