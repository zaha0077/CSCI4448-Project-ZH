using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/**
 * Parent class used for item behavior.
 */
public abstract class Effect {
	/**
	 * Executes the effect's behavior as defined by derived classes.
	 */
	public abstract void Execute ();
}

/**
* Implementation of Effect that increases the player's shot range by a specified value upon execution.
*/
public class Rangeup : Effect {
	private int amount_;

	public Rangeup(int value){
		amount_ = value;
	}

	public override void Execute(){
		Controller.shot_range_ += amount_;
	}
}

/**
* Implementation of Effect that increases the player's fire rate by a specified value upon execution.
*/
public class Rateup : Effect {
	private int amount_;

	public Rateup(int value){
		amount_ = value;
	}

	public override void Execute(){
		Controller.fire_rate_ -= amount_;
	}
}

/**
* Implementation of Effect that increases the player's shot damage by a specified value upon execution.
*/
public class Damageup : Effect {
	private int amount_;

	public Damageup(int value){
		amount_ = value;
	}

	public override void Execute(){
		Controller.dmg_ += amount_;
	}
}

/**
* Implementation of Effect that changes the text to be displayed by the Controller class and sets the text update flag to true.
*/
public class ShowText : Effect {

	private string str_;

	public ShowText(string str){
		str_ = str;
	}

	public override void Execute(){
		Controller.txtstr_ = str_;
		Controller.txtflag_ = true;
	}
}

/**
* Implementation of Effect that sets the amount of times the player can jump to the specified value.
*/
public class JumpChange : Effect {
	private int val_;

	public JumpChange(int value){
		val_ = value;
	}

	public override void Execute(){
		Controller.jumpcap_ = val_;
	}
}

/**
* Parent class of items, which use the Factory design pattern to define how they change game parameters upon being collected.
*/
public class Item : MonoBehaviour {
	//An item has a list of effects that are executed 
	protected List<Effect> effects_ = new List<Effect>();

	/**Factory method used to assign item behaviors upon instantiation*/
	public virtual void SetEffects(){}

	/**
	 * Defined in Unity's MonoBehavior class. 
	 * 
	 * MonoBehavior derived classes use this function for instantiation rather than the constructor.
	*/
	void Start () {
		SetEffects ();
	}

	/**
	 * Defined in Unity's MonoBehavior class. 
	 * 
	 * This function is used in collision detection.
	*/
	void OnCollisionEnter2D(Collision2D col){
		//Upon touching the player, execute every behavior this item has.
		if (col.gameObject.GetComponent<Player> () != null) {
			foreach (Effect thing in effects_) {
				thing.Execute ();
			}
			Destroy (this.gameObject);
		}
	}
}
