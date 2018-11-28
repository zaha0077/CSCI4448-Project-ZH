using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Controller : MonoBehaviour {
	//game parameters
	private bool critical_ = false; //is health critical?
	private bool gameover_ = false; //is the game over?
	public static int health_; //Player health
	public static int shot_range_; //Player projectile range
	public static int fire_rate_; //Player fire rate when the button is held down.
	public static int dmg_; //Player bullet damage
	public static int jumpcap_; //How many times the player can jump consecutively.
	public static string txtstr_ = ""; //text to display 
	public static int txt_timer_ = -1; //Counter for how long to display a particular text.
	public static bool txtflag_ = false; //Used to tell when to update displayed text.

	//Parameters that allow communication with the UI
	public Text itemtxt;
	public Text healthtxt;

	//Prefabs for instantiation
	public Transform wall;
	public Transform breakable;
	public Object trap;
	public Object enemy1;

	//personal params;
	private Color red_ = new Color (1f, 0f, 0f);
	private Color white_ = new Color (1f, 1f, 1f);

	//The game world
	int[,] layout = new int[20,24] {
		{1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1},
		{1,0,0,0,0,1,1,1,1,1,1,1,0,0,0,0,0,0,1,0,0,0,0,1},
		{1,0,0,0,0,1,1,1,1,1,1,1,0,0,0,0,0,0,1,0,0,0,0,1},
		{1,0,0,0,0,1,0,0,0,0,0,0,0,0,0,0,0,0,1,0,0,0,0,1},
		{1,0,0,0,0,1,4,0,0,0,0,4,0,0,0,0,0,0,1,0,0,0,0,1},
		{1,0,0,0,0,1,0,0,0,0,0,0,0,0,0,0,0,0,1,0,0,0,0,1},
		{1,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,1,0,0,0,0,1},
		{1,0,0,0,0,0,0,0,0,0,0,1,0,0,0,0,0,0,1,0,0,0,0,1},
		{1,0,0,0,0,0,0,0,0,0,0,1,0,0,0,0,0,0,1,0,0,0,0,1},
		{1,0,0,0,0,0,0,0,0,0,0,1,0,0,0,0,0,0,1,0,0,0,0,1},
		{1,2,1,1,1,1,0,0,0,0,0,1,0,0,0,0,0,0,1,0,0,0,0,1},
		{1,2,1,1,1,1,0,0,0,0,0,1,0,0,0,0,0,0,1,0,0,0,0,1},
		{1,2,1,1,1,1,0,0,0,0,0,1,0,0,0,0,0,0,1,0,0,0,0,1},
		{1,2,0,0,0,0,0,0,0,0,0,1,0,0,0,0,0,0,0,0,0,0,0,1},
		{1,2,0,0,0,0,0,0,0,0,0,1,0,0,0,0,0,0,0,0,0,0,0,1},
		{1,2,0,0,0,0,0,0,0,0,0,1,0,0,0,0,0,0,0,0,0,0,0,1},
		{1,3,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,1,0,0,0,0,0,1},
		{1,3,0,4,0,0,0,0,0,0,0,0,0,0,0,0,0,1,0,0,0,0,0,1},
		{1,3,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,1,0,0,0,0,0,1},
		{1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,0,0,0,1,1,1}};

	//{0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0}

	public void SetText(){
		itemtxt.text = txtstr_;
		txt_timer_ = 90;
		txtflag_ = false;
	}

	//Overrides of functions from Unity's MonoBehavior class.
	// Initialization
	void Start () {

		Camera.main.transform.parent = GameObject.Find("player").transform; //Set the camera to follow the player.
		health_ = 100;
		shot_range_ = 8;
		fire_rate_ = 28;
		jumpcap_ = 1;
		dmg_ = 10;
		float x0 = -1.28f;
		float y0 = -0.96f;
		float step = 0.16f;
		itemtxt.text = "";

		//generate the level from the array.
		for (int i = 0; i < layout.GetLength(0); i++){
			for (int j = 0; j < layout.GetLength(1); j++) {
				switch (layout[i,j]) {
				case 1: //Walls
					Instantiate (wall, new Vector3 (x0 + (step * i), y0 + (step * j), 0.0f), Quaternion.identity);
					break;
				case 2: //Breakable walls
					Instantiate (breakable, new Vector3 (x0 + (step * i), y0 + (step * j), 0.0f), Quaternion.identity);
					break;
				case 3: //Spikes of doom
					Instantiate (trap, new Vector3 (x0 + (step * i), y0 + (step * j), 0.0f), Quaternion.identity);
					break;
				case 4: //Enemy type 1
					Instantiate (enemy1, new Vector3 (x0 + (step * i), y0 + (step * j), 0.0f), Quaternion.identity);
					break;
				default:
					break;					
				}
			}
		}
	}
	
	// Code executed every frame.
	void Update () {

		healthtxt.text = "HP: " + health_.ToString ();

		if (txtflag_){
			SetText ();
		}

		if (health_ <= 30 && !critical_) {
			healthtxt.color = red_;
			critical_ = true;
		} else if (health_ > 30 && critical_) {
			healthtxt.color = white_;
			critical_ = false;
		}

		if (health_ <= 0 && !gameover_) {
			health_ = 0;
			Camera.main.transform.parent = null; //Tell the camera to stop following the player
			Destroy (GameObject.Find("player")); //kill the player
			itemtxt.text = "";
			itemtxt.fontSize = 48;
			txt_timer_ = 90;
			gameover_ = true; //Prevent this section of code from being executed more than once.
		}

		if (txt_timer_ >= 0) {
			if (txt_timer_ == 0 ) {
				if (gameover_) {
					itemtxt.text = "Game Over";
				} else {
					itemtxt.text = "";
				}
			}
			txt_timer_--;
		}
	}
}
