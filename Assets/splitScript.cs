﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class splitScript : MonoBehaviour {
	private Rigidbody2D rb2d;
	private ProjectileScript proj;

	// Use this for initialization
	void Start () {
		rb2d = GetComponent<Rigidbody2D> ();
		proj = GetComponent<ProjectileScript> ();
		proj.health = 3;
	}
	
	// Update is called once per frame
	void Update () {
		
	}

	void OnCollisionEnter2D(Collision2D node){
		if (node.gameObject.tag == "Shield") {
			proj.health--;
			if (proj.health <= 2) {
				for (int i = 0; i < 4; i++) {
					GameObject tempShield = Instantiate(Resources.Load<GameObject>("Prefabs/Proj3"));
					tempShield.transform.position = gameObject.transform.position;
				}
				GameObject.Destroy (gameObject);
				GameControl.instance.score++;
			}
		}

	}
}