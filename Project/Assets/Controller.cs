using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Controller : MonoBehaviour {
	private bool gameover_ = false;
	public static int health_;
	public Transform wall;
	public Object trap;
	public int width = 24;
	public int height = 16;
	int[,] layout = new int[20,24] {
		{1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1},
		{1,0,0,0,0,1,1,1,1,1,1,1,0,0,0,0,0,0,1,0,0,0,0,1},
		{1,0,0,0,0,1,1,1,1,1,1,1,0,0,0,0,0,0,1,0,0,0,0,1},
		{1,0,0,0,0,1,0,0,0,0,0,0,0,0,0,0,0,0,1,0,0,0,0,1},
		{1,0,0,0,0,1,0,0,0,0,0,0,0,0,0,0,0,0,1,0,0,0,0,1},
		{1,0,0,0,0,1,0,0,0,0,0,0,0,0,0,0,0,0,1,0,0,0,0,1},
		{1,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,1,0,0,0,0,1},
		{1,0,0,0,0,0,0,0,0,0,0,1,0,0,0,0,0,0,1,0,0,0,0,1},
		{1,0,0,0,0,0,0,0,0,0,0,1,0,0,0,0,0,0,1,0,0,0,0,1},
		{1,0,0,0,0,0,0,0,0,0,0,1,0,0,0,0,0,0,1,0,0,0,0,1},
		{1,1,1,1,1,1,0,0,0,0,0,1,0,0,0,0,0,0,1,0,0,0,0,1},
		{1,1,1,1,1,1,0,0,0,0,0,1,0,0,0,0,0,0,1,0,0,0,0,1},
		{1,1,1,1,1,1,0,0,0,0,0,1,0,0,0,0,0,0,1,0,0,0,0,1},
		{1,1,0,0,0,0,0,0,0,0,0,1,0,0,0,0,0,0,0,0,0,0,0,1},
		{1,1,0,0,0,0,0,0,0,0,0,1,0,0,0,0,0,0,0,0,0,0,0,1},
		{1,1,0,0,0,0,0,0,0,0,0,1,0,0,0,0,0,0,0,0,0,0,0,1},
		{1,2,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,1,0,0,0,0,0,1},
		{1,2,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,1,0,0,0,0,0,1},
		{1,2,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,1,0,0,0,0,0,1},
		{1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,0,0,0,1,1,1}};

	//Overrides of functions from Unity's MonoBehavior class.
	// Initialization
	void Start () {

		Camera.main.transform.parent = GameObject.Find("player").transform; //Set the camera to follow the player.
		health_ = 100;
		float x0 = -1.28f;
		float y0 = -0.96f;
		float step = 0.16f;

		for (int i = 0; i < layout.GetLength(0); i++){
			for (int j = 0; j < layout.GetLength(1); j++) {
				switch (layout[i,j]) {
				case 1:
					Instantiate (wall, new Vector3 (x0 + (step * i), y0 + (step * j), 0.0f), Quaternion.identity);
					break;
				case 2:
					Instantiate (trap, new Vector3 (x0 + (step * i), y0 + (step * j), 0.0f), Quaternion.identity);
					break;
				default:
					break;					
				}
			}
		}

		//Instantiate (wall, new Vector3 (0.16f, -1.0f, 0.0f), Quaternion.identity);
	}
	
	// Code executed every frame.
	void Update () {
		if (health_ <= 0 && !gameover_) {
			Camera.main.transform.parent = null; //Tell the camera to stop following the player
			Destroy (GameObject.Find("player")); //kill the player
			gameover_ = true; //Prevent this section of code from being executed more than once.
		}

	}
}
