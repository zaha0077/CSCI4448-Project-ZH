using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//Breakable Walls
public class Breakable : Solid {
	private bool hit_ = false; //Used to limit object destruction only to collisions with bullets.

	public GameObject shard;

	public override void Explode(){
		hit_ = true;
		Destroy (this.gameObject);
	}
		
	//Override of method inherited from MonoBehavior
	void OnDestroy(){
		if (hit_) {
			for (int i = 1; i < 5; i++) {
				GameObject tmp = Instantiate (shard, this.transform.position, Quaternion.identity);
				tmp.GetComponent<Shard> ().setDirection (45.0f + (90.0f * i));
			}
		}
	}
}
