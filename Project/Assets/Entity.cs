using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Entity : MonoBehaviour {
	private float speed_;

	public void setSpeed(float value){
		speed_ = value;
	}
	public float getSpeed(){
		return speed_;
	}
	public virtual void Move(){
	}

	// Use this for initialization
	void Start () {
	}
	
	// Update is called once per frame
	void Update () {
	}
}
