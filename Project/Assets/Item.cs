using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//Concrete behaviors for items to have.
public abstract class Effect {
	public abstract void Execute ();
}

//Extends player projectile range by the amount specified.
public class Rangeup : Effect {
	private int amount_;

	public Rangeup(int value){
		amount_ = value;
	}

	public override void Execute(){
		Controller.shot_range_ += amount_;
	}
}

//Increases the player's fire rate
public class Rateup : Effect {
	private int amount_;

	public Rateup(int value){
		amount_ = value;
	}

	public override void Execute(){
		Controller.fire_rate_ -= amount_;
	}
}

//Increases the player's damage output.
public class Damageup : Effect {
	private int amount_;

	public Damageup(int value){
		amount_ = value;
	}

	public override void Execute(){
		Controller.dmg_ += amount_;
	}
}

//Sets unique text to display upon item collection.
public class ShowText : Effect {
	
	public ShowText(string str){
		Controller.txtstr_ = str;
	}

	public override void Execute(){
		Controller.txtflag_ = true;
	}
}

//Increases the jump cap.
public class JumpChange : Effect {
	public override void Execute(){
		Controller.jumpcap_++;
	}
}

//"Abstract" parent class for items, which use the Factory design pattern for their generation.
public class Item : MonoBehaviour {
	//An item has a list of effects that are executed 
	protected List<Effect> effects_ = new List<Effect>();

	//Factory Method
	public virtual void SetEffects(){}

	//Overrides of Monobehavior functions
	// initialization
	void Start () {
		SetEffects ();
	}
	
	// Code to be executed every frame
	void Update () {
		
	}

	//Collision Behavior.
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
