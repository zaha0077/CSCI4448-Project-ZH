using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//This class handles player behavior.
public class Player : Entity {

	private float gravity_; //Vertical movement speed.
	private float hspeed_; //Horizontal speed.
	private float terminal_vel_ = 0.05f; //Terminal velocity, or max falling speed. 
	private bool in_air_; //Checks if the player is already in the air.
	private bool can_shoot_ = true; //Can we shoot?
	private int cooldown_; //Frames until we can shoot again.
	private int jumps_ = 0; //how many times we have jumped.

	public static int dir_ = 1; //Horizontal direction, 1 for right, -1 for left. Used by projectiles to determine what direction to travel in.

	public GameObject shard; //Used to hold the prefab that the player instantiates upon destruction.
	public GameObject shot; //Used to hold the prefab containing bullet object information.



	//Test for free space for horizontal movement.
	bool checkPosition(float speed, Vector3 pos){
		Vector3 direction;
		if (speed < 0.0f) {
			direction = Vector2.left;
		} else {
			direction = Vector2.right;
		}

		Vector3 side_a = new Vector3 (pos.x,pos.y+0.03f,pos.z);
		Vector3 side_b = new Vector3 (pos.x,pos.y-0.03f,pos.z);
		return !(Physics2D.Raycast (side_a, direction, 0.14f, LayerMask.GetMask("Solids")) && Physics2D.Raycast (side_b, direction, 0.14f, LayerMask.GetMask("Solids")));
	}

	//Sets the direction of the player's sprite.
	float changeSpriteDirection(float value){
		if (hspeed_ != 0.0f) {
			value = hspeed_ / Mathf.Abs (hspeed_);
			dir_ = (int)value;
		}
		return value;
	}

	public void Jump(){
		in_air_ = true;
		gravity_ = -0.14f;
		jumps_++;
	}

	public bool canJump (){
		return Input.GetButtonDown ("Jump") && jumps_ < Controller.jumpcap_;
	}

	//Movement behavior
	public override void Move(){
		Vector3 temp = this.transform.position;
		Vector3 scale = this.transform.localScale;

		if (!getInvincible ()) {
			hspeed_ = Input.GetAxis ("Horizontal") * getSpeed ();
			scale.x = changeSpriteDirection (scale.x);
		}
	
		if (checkPosition(hspeed_, temp)){
			temp.x += hspeed_;
			}

		if (canJump()) {
			Jump ();
		}

		if (in_air_) {
			temp.y -= gravity_;
		}

		this.transform.position = temp;
		this.transform.localScale = scale;
	}

	//Take damage.
	public void triggerHurt(){
		Controller.health_ -= 10;
		setInvincible (true);
		hurt_ticks_ = hurt_max_;
		hspeed_ = -(hspeed_*2f); //knockback
		in_air_ = true;
		gravity_ = -0.10f;
		GetComponent<SpriteRenderer> ().color = hurtcolor_;
	}

	//Shooting
	private void Shoot (){
		if (Input.GetAxis ("Fire1") > 0 && can_shoot_) {
			Instantiate (shot, this.transform.position, Quaternion.identity);
			can_shoot_ = false;
			cooldown_ = Controller.fire_rate_;
		}
	}

	//Check if we walk off a platform.
	private void checkFloor(Vector3 pos){
		
		Vector3 side_a = new Vector3 (pos.x + 0.05f, pos.y, pos.z);
		Vector3 side_b = new Vector3 (pos.x - 0.05f, pos.y, pos.z);

		if ((!in_air_) && ((!Physics2D.Raycast(side_a, Vector2.down, 0.18f)) && (!Physics2D.Raycast (side_b, Vector2.down, 0.18f)))) {
			in_air_ = true;
			jumps_++;
		}
	}

	//Manage the timer for mercy invincibility.
	private void resolveHurtCounter(){
		//Reset mercy invincibility
		if (getInvincible () && hurt_ticks_ == 0) {
			GetComponent<SpriteRenderer> ().color = normalcolor_;
			setInvincible (false);
		}

		//Decrement hurt ticks
		if (hurt_ticks_ > 0) {
			hurt_ticks_--;
		}
	}

	//Resolves shot cooldown
	private void resolveShotCooldown(){
		//Decrement shot cooldown
		if (cooldown_ > 0) {
			cooldown_--;
		} else if (!can_shoot_) { //Make us able to shoot again.
			can_shoot_ = true;
		}
	}

	//Apply gravity effects.
	private void doFallAccelaration(){
		if (in_air_ && gravity_ < terminal_vel_) {
			gravity_ += 0.01f;
		}
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
		//Shoot if able
		Shoot();
		doFallAccelaration ();
		checkFloor(this.transform.position);
		resolveHurtCounter ();
		resolveShotCooldown ();

		//Apply movement behavior
		if (!Controller.endflag_) {
			Move ();
		}

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
				jumps_ = 0;
				gravity_ = 0.0f;
				temp.y = col.transform.position.y + 0.16f;

			}
			else if (in_air_ && Physics2D.Raycast (temp, Vector2.up, 0.16f, LayerMask.GetMask ("Solids"))){ //if there is a solid object above, bump against it.
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

		//Exit Collision
		if (col.gameObject.GetComponent<Exit>() != null) {
			if (!Controller.endflag_) { //If we can be hurt, do the following.
				Controller.endflag_ = true;
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
