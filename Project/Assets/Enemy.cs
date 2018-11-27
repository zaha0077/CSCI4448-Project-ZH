using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : Entity {
	protected int hp_;
	public GameObject shard;

	public void takeDamage(int value){
		hp_ -= value;
	}

	//Functions from Unity's MonoBehavior class.
	// Initialization
	void Start () {
		hp_ = 20;
	}
	
	// Code executed every frame.
	void Update () {
		if (hp_ <= 0) {
			Destroy (this.gameObject);
		}
	}

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
