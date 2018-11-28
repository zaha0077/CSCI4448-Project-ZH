using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//"abstract" calss for entities
public class Entity : MonoBehaviour {
	private float speed_; //Movement velocity, some someclasses use this for horizontal movement only.
	private bool invincible_ = false; //Can damageable objects be harmed?
	protected int hurt_ticks_ = 0; //Used by subclasses that can be damaged to prevent constant contact damage.
	protected int hurt_max_ = 5; //How many frames until damageable objects can be hurt again.
	protected Color hurtcolor_ = new Color (1f, 0f, 0f); //Color to flash when hurt
	protected Color normalcolor_ = new Color (1f, 1f, 1f); //Default color

	public static bool quitting_ = false; //Controls when it's appropriate to spawn fragments on object destruction

	public void setSpeed(float value){ 
		speed_ = value;
	}

	public float getSpeed(){
		return speed_;
	}

	public void setInvincible(bool val){
		invincible_ = val;
	}

	public bool getInvincible(){
		return invincible_;
	}

	//Template for movement behavior
	public virtual void Move(){
	}

	//Built-in unity functions used in almost all Unity classes:
	// initialization
	void Start () {
	}
	
	// Behavior of the object at every frame
	void Update () {
	}

	//Override of OnApplicationQuit from MonoBehavior.
	void OnApplicationQuit(){
		quitting_ = true;
	}
}
