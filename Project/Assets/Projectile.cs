using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/**
 * Projectiles fired by the player.
 */
public class Projectile : Entity {

	private int ticks_;

	/**
	* Unique movement behavior for the Projectile class.
	*/
	public override void Move(){
		Vector3 temp = this.transform.position;
		temp.x += GetSpeed (); 
		this.transform.position = temp;
	}

	/**
	 * Defined in Unity's MonoBehavior class. 
	 * 
	 * MonoBehavior derived classes use this function for instantiation rather than the constructor.
	*/
	void Start () {
		ticks_ = Controller.shot_range_;
		SetSpeed (0.06f * Player.dir_);
	}
	
	/**
	 * Defined in Unity's MonoBehavior class. 
	 * 
	 * This function is called every frame inside the game.
	*/
	void Update () {
		Move ();
		if (ticks_ == 0) {
			Destroy (this.gameObject);
		}
		ticks_--;
	}
		
	/**
	 * Defined in Unity's MonoBehavior class. 
	 * 
	 * This function is used in collision detection.
	*/
	void OnCollisionEnter2D(Collision2D collision){

		if (collision.gameObject.GetComponent<Player> () != null) {
			Physics2D.IgnoreCollision (this.gameObject.GetComponent<Collider2D>(), collision.gameObject.GetComponent<Collider2D>(), true);
		}

		//Solid collisions
		if (collision.gameObject.GetComponent<Solid>() != null) {
			collision.gameObject.GetComponent<Solid>().Explode();
			Destroy (this.gameObject);
		}

		//Enemy collision.
		if (collision.gameObject.GetComponent<Enemy>() != null) {
			Destroy (this.gameObject);
			if (!collision.gameObject.GetComponent<Enemy>().GetInvincible ()) { //If enemy can be hurt, do the following.
				collision.gameObject.GetComponent<Enemy>().TakeDamage(Controller.dmg_);
			}
		}
	}
		
}
