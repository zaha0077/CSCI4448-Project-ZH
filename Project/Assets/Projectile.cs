using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Projectile : Entity {

	private int ticks = 8;

	public override void Move(){
		Vector3 temp = this.transform.position;
		temp.x += getSpeed (); 
		this.transform.position = temp;
	}

	// Use this for initialization
	void Start () {
		setSpeed (0.06f * Player.dir_);
	}
	
	// Update is called once per frame
	void Update () {
		Move ();
		if (ticks == 0) {
			Destroy (this.gameObject);
		}
		ticks--;
	}
		
	//Collision detection.
	void OnCollisionEnter2D(Collision2D collision){

		if (collision.gameObject.GetComponent<Player> () != null) {
			Physics2D.IgnoreCollision (this.gameObject.GetComponent<Collider2D>(), collision.gameObject.GetComponent<Collider2D>(), true);
		}

		//Solid collisions
		if (collision.gameObject.GetComponents<Solid>() != null) {
			Destroy (this.gameObject);
		}

		//Enemy collision.
		if (collision.gameObject.GetComponent<Enemy>() != null) {
			Destroy (this.gameObject);
			if (!collision.gameObject.GetComponent<Enemy>().getInvincible ()) { //If enemy can be hurt, do the following.
				collision.gameObject.GetComponent<Enemy>().takeDamage(20);
			}
		}
	}
		
}
