﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MoveGame : MonoBehaviour {
	private Rigidbody2D rb2d;
	public float moveSpeed;
	Matrix4x4 calibrationMatrix;
	public bool onPhone = false;

	void calibrateAccelerometer(){
		Vector3 wantedDeadZone = Input.acceleration;;
		Quaternion rotateQuaternion = Quaternion.FromToRotation(new Vector3(0f, 0f, -1f), wantedDeadZone);
		//create identity matrix ... rotate our matrix to match up with down vec
		Matrix4x4 matrix = Matrix4x4.TRS(Vector3.zero, rotateQuaternion, new Vector3(1f, 1f, 1f));
		//get the inverse of the matrix
		this.calibrationMatrix = matrix.inverse;
	}

	Vector3 getAccelerometer(Vector3 accelerator){
		Vector3 accel = this.calibrationMatrix.MultiplyVector(accelerator);
		return accel;
	}

	// Use this for initialization
	void Start () {
		rb2d = GetComponent<Rigidbody2D> ();
		moveSpeed = 12f;
		calibrateAccelerometer ();
	}
	
	void FixedUpdate () {
		if (onPhone) {
			/* 
			 * MOVING WITH ACCELEROMETER
			Vector3 dir = getAccelerometer (Input.acceleration);
			if (dir.sqrMagnitude > 1)
				dir.Normalize ();
			rb2d.velocity = new Vector2 (45 * dir.x, 45 * dir.y);
			*/
			rb2d.velocity = Vector2.Scale(JoystickScript.instance.movePos,(new Vector2(moveSpeed, moveSpeed)));
		} else {
			float v = Input.GetAxisRaw("Vertical");
			float h = Input.GetAxisRaw("Horizontal");
			rb2d.velocity = new Vector2 (moveSpeed * h, moveSpeed * v);
		}

    }
}
