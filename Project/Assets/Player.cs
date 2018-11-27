using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//This class handles player behavior.
public class Player : Entity {

	private float gravity_; //Vertical movement speed.
	private float hspeed_; //Horizontal speed.
	private bool in_air_; //Checks if the player is already in the air.
	private bool can_shoot_ = true; //Can we shoot?
	private int cooldown_; //Frames until we can shoot again.

	public static int dir_ = 1; //Horizontal direction, 1 for right, -1 for left
	public GameObject shard; //Used to hold the prefab that the player instantiates upon destruction.
	public GameObject shot; //Used to hold the prefab containing bullet object information.



	//Test for free space for horizontal movement.
	bool checkPosition(float speed){
		Vector3 direction;
		if (speed < 0.0f) {
			direction = Vector2.left;
		} else {
			direction = Vector2.right;
		}
		//Debug.DrawRay (this.transform.position, direction * 0.08f);
		return !Physics2D.Raycast (this.transform.position, direction, 0.14f, LayerMask.GetMask("Solids"));
	}

	//Movement behavior
	public override void Move(){
		Vector3 temp = this.transform.position;
		Vector3 scale = this.transform.localScale;

		if (!getInvincible ()) {
			hspeed_ = Input.GetAxis ("Horizontal") * getSpeed ();

			if (hspeed_ != 0.0f) {
				scale.x = hspeed_ / Mathf.Abs (hspeed_);
				dir_ = (int)scale.x;
			}
		}

		Debug.Log (dir_);

		if (checkPosition(hspeed_)){
			temp.x += hspeed_;
			}

		if (Input.GetButtonDown ("Jump") && !in_air_) {
			in_air_ = true;
			gravity_ = -0.14f;
		}

		if (in_air_) {
			temp.y -= gravity_;
		}

		this.transform.position = temp;
		this.transform.localScale = scale;
	}

	//Take damage.
	void triggerHurt(){
		Controller.health_ -= 10;
		setInvincible (true);
		hurt_ticks_ = hurt_max_;
		hspeed_ = -(hspeed_*2f); //knockback
		in_air_ = true;
		gravity_ = -0.10f;
		GetComponent<SpriteRenderer> ().color = new Color (1f, 0f, 0f);
		Debug.Log (Controller.health_);
	}

	//Overrides of functions from Unity's MonoBehavior class.
	// Initialization
	void Start () {
		in_air_ = true;
		gravity_ = 0.0f;
		setSpeed (0.03f);
	}
	
	// Code executed every frame
	void Update () {
		if (Input.GetAxis ("Fire1") > 0 && can_shoot_) {
			Instantiate (shot, this.transform.position, Quaternion.identity);
			can_shoot_ = false;
			cooldown_ = 7;
		}
			

		if (in_air_ && gravity_ < 0.06f) {
			gravity_ += 0.01f;
		}

		//Apply gravity if we walk off a platform.
		if ((!in_air_) && (!Physics2D.Raycast (this.transform.position, Vector2.down, 0.18f))) {
			in_air_ = true;
		}

		if (getInvincible () && hurt_ticks_ == 0) {
			GetComponent<SpriteRenderer> ().color = new Color (1f, 1f, 1f);
			setInvincible (false);
		}

		if (hurt_ticks_ > 0) {
			hurt_ticks_--;
		}

		if (cooldown_ > 0) {
			cooldown_--;
		} else if (!can_shoot_) {
			can_shoot_ = true;
		}

		Move ();

	}

	//returns true if the opposing object is below the specified coordinates.
	bool getVerticalRelative(Collision2D col){
		return col.contacts[0].normal.y == 1.0f;
	}

	//Collision detection.
	void OnCollisionEnter2D(Collision2D col){
		//Solid collisions
		if (col.gameObject.GetComponents<Solid>() != null) {
			Vector3 temp = this.transform.position;

			if (getVerticalRelative(col)) { //if there is a solid object below, stand on it.
				in_air_ = false;
				gravity_ = 0.0f;
				temp.y = col.transform.position.y + 0.16f;

			}
			else if (in_air_){ //if there is a solid object above, bump against it.
				gravity_ = 0.0f;
				temp.y = col.transform.position.y - 0.08f;
			}
			this.transform.position = temp;
		}

		//Enemy collision.
		if (col.gameObject.GetComponent<Enemy>() != null) {
			if (!getInvincible ()) { //If we can be hurt, do the following.
				triggerHurt();
			}
		}
	}

	//Reduces chance of clipping through objects.
	void OnCollisionStay2D(Collision2D col){
		if (col.gameObject.GetComponents<Solid> () != null) {
			Vector3 temp = this.transform.position;
			temp.y = col.transform.position.y + 0.16f;
		} else if (col.gameObject.GetComponent<Enemy> () != null) {
			if (!getInvincible ()) { //If we can be hurt, do the following.
				triggerHurt();
			}
		}
	}

	void OnDestroy(){
		if (!quitting_) {
			for (int i = 1; i < 5; i++) {
				GameObject tmp = Instantiate (shard, this.transform.position, Quaternion.identity);
				tmp.GetComponent<Shard> ().setDirection (45.0f + (90.0f * i));
			}
		}
	}
}
