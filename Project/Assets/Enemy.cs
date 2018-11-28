using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : Entity {
	protected int hp_;
	public GameObject shard;

	public void takeDamage(int value){
		hp_ -= value;
		GetComponent<SpriteRenderer> ().color = hurtcolor_;
		hurt_ticks_ = hurt_max_;
	}

	public void commonBehavior(){
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

	//Functions from Unity's MonoBehavior class.

	//Destruction behavior
	void OnDestroy(){
		if (!quitting_) {
			for (int i = 1; i < 5; i++) {
				GameObject tmp = Instantiate (shard, this.transform.position, Quaternion.identity);
				tmp.GetComponent<Shard> ().setDirection (45.0f + (90.0f * i));
			}
		}
	}
}
