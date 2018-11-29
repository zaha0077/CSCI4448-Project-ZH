using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/**An Enemy that lazily floats horizontally, bouncing off of walls in its path.*/
public class Floater : Enemy {

	private bool cooldown_ = false; //Used to prevent colliding with the same wall more than once.
	private int bounceticks_ = -1;

	/**
	* Unique movement behavior for this Enemy.
	*/
	public override void Move(){
		Vector3 temp = this.transform.position;
		temp.x += GetSpeed (); 
		this.transform.position = temp;
	}

	/**
	 * Defined in Unity's MonoBehavior class. 
	 * 
	 * MonoBehavior derived classes use this function for instantiation rather than the constructor.
	*/
	void Start () {
		SetSpeed (0.01f);
		hp_ = 40;
	}

	/**
	 * Defined in Unity's MonoBehavior class. 
	 * 
	 * This function is called every frame inside the game.
	*/
	void Update () {
		Move ();
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
			bounceticks_ = hurt_max_;
			cooldown_ = true;
			SetSpeed (-GetSpeed());
		}
	}

}
