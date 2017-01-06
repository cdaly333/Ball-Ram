﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ProjectileScript : MonoBehaviour {

	private Rigidbody2D rb2d;
	private CircleCollider2D projCol;
	private int minSpeed, maxSpeed;
	public int health;

	// Use this for initialization
	void Start () {
		health = 1;
		rb2d = GetComponent<Rigidbody2D> ();
		projCol = GetComponent<CircleCollider2D> ();
		minSpeed = 1;
		maxSpeed = 10;
		rb2d.velocity = new Vector2(Random.Range(minSpeed, maxSpeed), Random.Range(minSpeed, maxSpeed));
	}
	
	// Update is called once per frame
	void FixedUpdate () {
	}
	void OnCollisionEnter2D(Collision2D node){
		if (node.gameObject.tag == "Shield") {
			health--;
			Debug.Log("Ball HP: " + health);
			if (health < 1) {
				GameObject.Destroy (gameObject);
				GameControl.instance.score++;
				Debug.Log ("Score: " + GameControl.instance.score);
			}
		}
		else if (node.gameObject.name == "Player") {
			GameObject.Destroy (gameObject);
			GameControl.instance.score++;
			Debug.Log ("Score: " + GameControl.instance.score);
		}
	}
}
