using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/**A derived class of Solid that is destroyable by the player.*/
public class Breakable : Solid {
	private bool hit_ = false; //Used to limit object destruction only to collisions with bullets.

	public GameObject shard;

	/**
	 * Destroys the Breakable instance when called.
	 */
	public override void Explode(){
		hit_ = true;
		Destroy (this.gameObject);
	}
		
	/**
	 * Defined in Unity's MonoBehavior class. 
	 * 
	 * This function is called when an instance of a MonoBehavior derived class is destroyed.
	*/
	void OnDestroy(){
		if (hit_) {
			for (int i = 1; i < 5; i++) {
				GameObject tmp = Instantiate (shard, this.transform.position, Quaternion.identity);
				tmp.GetComponent<Shard> ().SetDirection (45.0f + (90.0f * i));
			}
		}
	}
}
