using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/**
* This class controls the short lived fragments created when either the player, an enemy, or breakable wall is killed/destroyed.
*/
public class Shard : Entity {
	private float direction_; //Direction of travel, in radians.
	private int lifetime_ = 30; //Ticks this object stays alive.

	/**
	 * Takes a direction in degrees and sets the direction_ field to the radian equivalent
	*/
	public void SetDirection(float dir){
		direction_ = dir * (Mathf.PI/180.0f);
	}

	/**
	 * Unique movement behavior for the Shard class.
	*/
	public override void Move(){
		Vector3 temp = this.transform.position;

		//Move in the angle specified by direction_.
		temp.x += GetSpeed () * Mathf.Cos (direction_);
		temp.y += GetSpeed () * Mathf.Sin (direction_);

		this.transform.position = temp;
	}

	/**
	 * Defined in Unity's MonoBehavior class. 
	 * 
	 * MonoBehavior derived classes use this function for instantiation rather than the constructor.
	*/
	void Start () {
		SetSpeed(0.01f);
	}
	
	/**
	 * Defined in Unity's MonoBehavior class. 
	 * 
	 * This function is called every frame inside the game.
	*/
	void Update () {
		Move ();
		this.transform.Rotate (0,0,22.5f);
		if (lifetime_ == 0) {
			Destroy (this.gameObject);
		}
		lifetime_--;
	}
}
