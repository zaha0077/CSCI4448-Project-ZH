using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/**
 * The most complex Entity in the game. This class handles the playable character and its behaviors.
*/
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



	/**
	 * Returns true if the position immediately ahead of the player's movement does not contain a Solid. False otherwise.
	 */
	bool CheckPosition(float speed, Vector3 pos){
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

	/**
	 * Sets the direction of the player's sprite. Returns the modified direction.
	 */
	float ChangeSpriteDirection(float xscale){
		if (hspeed_ != 0.0f) {
			xscale = hspeed_ / Mathf.Abs (hspeed_);
			dir_ = (int)xscale;
		}
		return xscale;
	}

	/**
	* Jump behavior.
	*/
	public void Jump(){
		in_air_ = true;
		gravity_ = -0.14f;
		jumps_++;
	}

	/**
	* Returns true if the player can jump, false otherwise.
	*/
	public bool CanJump (){
		return Input.GetButtonDown ("Jump") && jumps_ < Controller.jumpcap_;
	}

	/**
	* Unique movement behavior for the Player class.
	*/
	public override void Move(){
		Vector3 temp = this.transform.position;
		Vector3 scale = this.transform.localScale;

		if (!GetInvincible ()) {
			hspeed_ = Input.GetAxis ("Horizontal") * GetSpeed ();
			scale.x = ChangeSpriteDirection (scale.x);
		}
	
		if (CheckPosition(hspeed_, temp)){
			temp.x += hspeed_;
			}

		if (CanJump()) {
			Jump ();
		}

		if (in_air_) {
			temp.y -= gravity_;
		}

		this.transform.position = temp;
		this.transform.localScale = scale;
	}

	/**
	* Has the player take damage.
	*/
	public void TriggerHurt(){
		Controller.health_ -= 10;
		SetInvincible (true);
		hurt_ticks_ = hurt_max_;
		hspeed_ = -(hspeed_*2f); //knockback
		in_air_ = true;
		gravity_ = -0.10f;
		GetComponent<SpriteRenderer> ().color = hurtcolor_;
	}

	/**
	* Shooting behavior.
	*/
	private void Shoot (){
		if (Input.GetAxis ("Fire1") > 0 && can_shoot_) {
			Instantiate (shot, this.transform.position, Quaternion.identity);
			can_shoot_ = false;
			cooldown_ = Controller.fire_rate_;
		}
	}

	/**
	* Checks if the player has walked off the edge of a platform and applies gravity if they have.
	*/
	private void CheckFloor(Vector3 pos){
		
		Vector3 side_a = new Vector3 (pos.x + 0.05f, pos.y, pos.z);
		Vector3 side_b = new Vector3 (pos.x - 0.05f, pos.y, pos.z);

		if ((!in_air_) && ((!Physics2D.Raycast(side_a, Vector2.down, 0.18f, LayerMask.GetMask("Solids"))) && (!Physics2D.Raycast (side_b, Vector2.down, 0.18f, LayerMask.GetMask("Solids"))))) {
			in_air_ = true;
			jumps_++;
		}
	}

	/**
	* Manages the player's brief invulnerability period after taking damage.
	*/
	private void ResolveHurtCounter(){
		//Reset mercy invincibility
		if (GetInvincible () && hurt_ticks_ == 0) {
			GetComponent<SpriteRenderer> ().color = normalcolor_;
			SetInvincible (false);
		}

		//Decrement hurt ticks
		if (hurt_ticks_ > 0) {
			hurt_ticks_--;
		}
	}

	/**
	* Manages the cooldown period between shots.
	*/
	private void ResolveShotCooldown(){
		//Decrement shot cooldown
		if (cooldown_ > 0) {
			cooldown_--;
		} else if (!can_shoot_) { //Make us able to shoot again.
			can_shoot_ = true;
		}
	}

	/**
	* Applies acceleration due to gravity.
	*/
	private void DoFallAccelaration(){
		if (in_air_ && gravity_ < terminal_vel_) {
			gravity_ += 0.01f;
		}
	}

	/** 
	 * Takes a collision and returns true if the collided object is below the player.
	*/
	private bool GetVerticalRelative(Collision2D col){
		return col.contacts[0].normal.y == 1.0f;
	}

	/**
	 * Defined in Unity's MonoBehavior class. 
	 * 
	 * MonoBehavior derived classes use this function for instantiation rather than the constructor.
	*/
	void Start () {
		in_air_ = true;
		gravity_ = 0.0f;
		SetSpeed (0.03f);
	}
	
	/**
	 * Defined in Unity's MonoBehavior class. 
	 * 
	 * This function is called every frame inside the game.
	*/
	void Update () {
		//Shoot if able
		Shoot();
		DoFallAccelaration ();
		CheckFloor(this.transform.position);
		ResolveHurtCounter ();
		ResolveShotCooldown ();

		//Apply movement behavior
		if (!Controller.endflag_) {
			Move ();
		}

	}

	/**
	 * Defined in Unity's MonoBehavior class. 
	 * 
	 * This function is used in collision detection.
	*/
	void OnCollisionEnter2D(Collision2D col){
		//Solid collisions
		if (col.gameObject.GetComponents<Solid>() != null) {
			Vector3 temp = this.transform.position;

			if (GetVerticalRelative(col)) { //if there is a solid object below, stand on it.
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
			if (!GetInvincible ()) { //If we can be hurt, do the following.
				TriggerHurt();
			}
		}

		//Exit Collision
		if (col.gameObject.GetComponent<Exit>() != null) {
			if (!Controller.endflag_) { //If we can be hurt, do the following.
				Controller.endflag_ = true;
			}
		}
	}

	/**
	 * Defined in Unity's MonoBehavior class. 
	 * 
	 * This function is used in collision detection.
	*/
	void OnCollisionStay2D(Collision2D col){
		if (col.gameObject.GetComponents<Solid> () != null) {
			Vector3 temp = this.transform.position;
			temp.y = col.transform.position.y + 0.16f;
		} else if (col.gameObject.GetComponent<Enemy> () != null) {
			if (!GetInvincible ()) { //If we can be hurt, do the following.
				TriggerHurt();
			}
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
