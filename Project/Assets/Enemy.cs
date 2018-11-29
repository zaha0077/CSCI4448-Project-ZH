using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/**
 * Parent class of all enemies in the game.
*/
public class Enemy : Entity {
	protected int hp_;
	public GameObject shard;

	/**
	 * Subtracts a specified value from the enemy's hp.
	*/
	public void TakeDamage(int value){
		hp_ -= value;
		GetComponent<SpriteRenderer> ().color = hurtcolor_;
		hurt_ticks_ = hurt_max_;
	}

	/**
	 * Manages behavior relating to enemies being damaged, including enemy death.
	*/
	public void ManageHealth(){
		//Reset color
		if (hurt_ticks_ == 0) {
			GetComponent<SpriteRenderer> ().color = normalcolor_;
			hurt_ticks_--;
		}

		//Decrement hurt ticks
		if (hurt_ticks_ > 0) {
			hurt_ticks_--;
		}

		//Death
		if (hp_ <= 0) {
			Destroy (this.gameObject);
		}
	}

	/**
	 * Defined in Unity's MonoBehavior class. 
	 * 
	 * This function is called when an instance of a MonoBehavior derived class is destroyed.
	*/
	void OnDestroy(){
		if (!quitting_) {
			for (int i = 1; i < 5; i++) {
				GameObject tmp = Instantiate (shard, this.transform.position, Quaternion.identity);
				tmp.GetComponent<Shard> ().SetDirection (45.0f + (90.0f * i));
			}
		}
	}
}
