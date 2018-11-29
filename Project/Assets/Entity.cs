using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/**The parent class for mobile game entities, which holds common values related to damage and movement.
*/
public class Entity : MonoBehaviour {
	private float speed_; //Movement velocity, some someclasses use this for horizontal movement only.
	private bool invincible_ = false; //Can damageable objects be harmed?
	protected int hurt_ticks_ = 0; //Used by subclasses that can be damaged to prevent constant contact damage.
	protected int hurt_max_ = 5; //How many frames until damageable objects can be hurt again.
	protected Color hurtcolor_ = new Color (1f, 0f, 0f); //Color to flash when hurt
	protected Color normalcolor_ = new Color (1f, 1f, 1f); //Default color

	public static bool quitting_ = false; //Controls when it's appropriate to spawn fragments on object destruction

	/**
	 * Sets the movement speed of the entity to a new value.
	*/
	public void SetSpeed(float value){ 
		speed_ = value;
	}

	/**
	 * Returns the current movement speed of the entity.
	*/
	public float GetSpeed(){
		return speed_;
	}

	/**
	 * Sets the entity's invulnerability flag to the specified value.
	*/
	public void SetInvincible(bool val){
		invincible_ = val;
	}

	/**
	 * Returns the value of the entity's invulnerability flag.
	*/
	public bool GetInvincible(){
		return invincible_;
	}

	/**Function responsible for movement behavior.
	 * 
	 */
	public virtual void Move(){
	}

	//Built-in unity functions used in almost all Unity classes:

	/**
	 * Defined in Unity's MonoBehavior class. 
	 * 
	 * This function defines behavior to be performed upon the game closing.
	*/
	void OnApplicationQuit(){
		quitting_ = true;
	}
}
