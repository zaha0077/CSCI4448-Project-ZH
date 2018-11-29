using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/**
* An invincible, immobile entity that inherits from the Enemy class for the purpose of damaging the player.
*/
public class Hazard : Enemy {

	/**
	 * Defined in Unity's MonoBehavior class. 
	 * 
	 * MonoBehavior derived classes use this function for instantiation rather than the constructor.
	*/
	void Start () {
		hp_ = 9999;
		SetInvincible (true);
	}
}
