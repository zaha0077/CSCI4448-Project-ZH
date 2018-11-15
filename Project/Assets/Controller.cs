using System.Collections;
using System.Collections.Generic;
using UnityEngine;

enum RoomObject{Wall, Player, Empty};

public class Controller : MonoBehaviour {
	public static int health_;
	public Transform wall;
	bool[,] layout = new bool[16,12] {
		{true,true,true,true,true,true,true,true,true,true,true,true},
		{true,false,false,false,false,true,false,false,false,false,false,false},
		{true,false,false,false,false,true,false,false,false,false,false,false},
		{true,false,false,false,false,true,false,false,false,false,false,false},
		{true,false,false,false,false,true,false,false,false,false,false,false},
		{true,false,false,false,false,true,false,false,false,false,false,false},
		{true,false,false,false,false,false,false,false,false,false,false,false},
		{true,false,false,false,false,false,false,false,false,false,false,false},
		{true,false,false,false,false,false,false,false,false,false,false,false},
		{true,false,false,false,false,false,false,false,false,false,false,false},
		{true,false,false,false,false,true,false,false,false,false,false,false},
		{true,false,false,false,false,true,false,false,false,false,false,false},
		{true,false,false,false,false,true,false,false,false,false,false,false},
		{true,false,false,false,false,false,false,false,false,false,false,false},
		{true,false,false,false,false,false,false,false,false,false,false,false},
		{true,false,false,false,false,false,false,false,false,false,false,false}};
	// Use this for initialization
	void Start () {
		health_ = 100;
		float x0 = -1.28f;
		float y0 = -0.96f;
		float step = 0.16f;

		for (int i = 0; i < 16; i++){
			for (int j = 0; j < 12; j++) {
				if (layout[i,j]) {
					Instantiate (wall, new Vector3 (x0 + (step * i), y0 + (step * j), 0.0f), Quaternion.identity);
				}
			}
		}

		//Instantiate (wall, new Vector3 (0.16f, -1.0f, 0.0f), Quaternion.identity);
	}
	
	// Update is called once per frame
	void Update () {
		
	}
}
