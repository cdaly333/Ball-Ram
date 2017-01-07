﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerScript : MonoBehaviour {
	private Rigidbody2D rb2d;
	private CircleCollider2D playerCol;
	public int health;
	private int numShield;
	public List<GameObject> shieldArr;
	private Vector3 zAxis = new Vector3 (0,0,1);

	// Use this for initialization
	void Start () {
		rb2d = GetComponent<Rigidbody2D> ();
		playerCol = GetComponent<CircleCollider2D> ();

		health = 3;
		numShield = 3;
		float shieldDist = 360f / numShield;

		for (int i = 0; i < numShield; i++) {
			GameObject tempShield = Instantiate(Resources.Load<GameObject>("Prefabs/ShieldMain"));
			tempShield.transform.parent = gameObject.transform;
			tempShield.name = "ShieldMain" + i.ToString ();

			tempShield.transform.RotateAround ((Vector3) gameObject.transform.position, zAxis, shieldDist*i);
			Debug.Log (shieldDist * i);

			shieldArr.Add (tempShield);
		}
	}
	
	// Update is called once per frame
	void Update () {
		if(health < 1){
			GameControl.instance.PlayerDied ();
		}
	}
	void OnCollisionEnter2D(Collision2D node){
		if (node.gameObject.tag == "Projectile") {
			health--;
			Debug.Log("Health: " + health);
		}
	}
}
