﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HealthEnemyScript : MonoBehaviour {

    private Rigidbody2D rb;
    private ProjectileScript pScript;
    private SpriteRenderer spRend;
    public int maxHealth = 4;

    // Use this for initialization
    void Start () {
        rb = GetComponent<Rigidbody2D>();
        pScript = GetComponent<ProjectileScript>();
        spRend = GetComponent<SpriteRenderer>();
        maxHealth = 6;
        pScript.health = maxHealth;
    }
	
	// Update is called once per frame
	void Update () {
        
        switch (pScript.health)
        {
            case 6:
                spRend.color = Color.magenta;
                break;
            case 5:
                spRend.color = Color.red;
                break;
            case 4:
                spRend.color = Color.blue;
                break;
            case 3:
                spRend.color = Color.green;
                break;
            case 2:
                spRend.color = Color.yellow;
                break;
            case 1:
                spRend.color = Color.white;
                break;
        }
	}
}