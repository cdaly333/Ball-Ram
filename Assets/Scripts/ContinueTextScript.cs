﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using UnityEngine;

public class ContinueTextScript : MonoBehaviour {
	public static ContinueTextScript instance;
	public Text ContinueText;
	private float moveDist;

	// Use this for initialization
	void Start () {
		if (instance == null) {
			instance = this;
		} else if (instance != this) {
			Destroy (gameObject);
		}
		moveDist = (float)ContinuePanelScript.instance.gameObject.GetComponent<RectTransform>().rect.height/5f + 50f;

	}
	public IEnumerator MoveIn(){
		ContinueText = GetComponent<Text> ();
		if (ContinuePanelScript.instance.continueType == 0) {
			ContinueText.text = "Free continue...";
		} else if (ContinuePanelScript.instance.continueType == 1) {
			ContinueText.text = "100 coins to continue...";
		} else {
			ContinueText.text = "Watch ad to continue...";
		}
		float speed = -25f;
		float accel = ((speed*speed - (9))/(2f*moveDist));
		while (speed < 3f) {
			gameObject.transform.localPosition = new Vector2 (gameObject.transform.localPosition.x, gameObject.transform.localPosition.y + speed);
			speed += accel;
			yield return new WaitForSeconds (0.005f);
		}
		for (int i = 5; i > 0; i--) {
			if (ContinuePanelScript.instance.continueType == 0) {
				ContinueText.text = "Free continue..." + i;
			} else if (ContinuePanelScript.instance.continueType == 1) {
				ContinueText.text = "100 coins to continue..." + i;
			} else {
				ContinueText.text = "Watch ad to continue..." + i;
			}

			yield return new WaitForSeconds (1f);
		}

	}
	public IEnumerator MoveOut(){
		float speed = 25f;
		float accel = -((speed*speed - (9))/(2f*moveDist));
		while (speed > -3f) {
			gameObject.transform.localPosition = new Vector2 (gameObject.transform.localPosition.x, gameObject.transform.localPosition.y + speed);
			speed += accel;
			yield return new WaitForSeconds (0.005f);
		}
	}

}
