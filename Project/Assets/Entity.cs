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
		Vector3 temp = this.transform.position;
		temp.x += speed_;
		this.transform.position = temp;
	}

	// Use this for initialization
	void Start () {
		setSpeed (0.5f);
	}
	
	// Update is called once per frame
	void Update () {
		Move();
	}
}
