﻿using System.Collections;
using System.Collections.Generic;
using System;
using System.Runtime.Serialization.Formatters.Binary;
using System.IO;
using UnityEngine;
using UnityEngine.Advertisements;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class GameControl : MonoBehaviour {
	public static GameControl instance;
	public bool isContinue = false;
	public bool hasContinued = false;
	public bool newHighscore = false;
	public int score;
	public int numProj;
	public int numPow;
	public List<GameObject> projArr;
	public int level;
	public int numDead = 0;
	public int numDeadInRow = 0;
	public int coinTot = 0;
	public int coinSpawnOdds = 3; // 1 in coinSpawnOdds
	public int initShields;
	public float spawnSpeed = 1f;
    
	public float rotateSpeed;

    public Text scoreText;
	public Text coinText;
    public Text levelText;
        public int maxfont = 72;
        public int minFont = 10;
	GameObject []interfaces;

    public List<GameObject> powArr;
	private float minX = -25, maxX = 25, minY = -15, maxY = 15;
	public int nmbrOfPowerUps;

	public PlayerData saveData;
	// Use this for initialization
	void Awake () {
		if (instance == null) {
			instance = this;
		} else if (instance != this) {
			Destroy (gameObject);
		}


		score = 0;
		numProj = 0;
		level = 1;
		Screen.orientation = ScreenOrientation.LandscapeLeft;
		Screen.sleepTimeout = SleepTimeout.NeverSleep;
		//Application.targetFrameRate = 60;
        levelText.text = "";
        levelText.fontSize = minFont;
        setScoreText();
		coinText = GameObject.Find ("coinText").GetComponent<Text> ();

		//save file stuff
		//File.Delete (Application.persistentDataPath + "/playerInfo.dat");
		Load();
		saveData.character = "default";
		if (saveData.character == "default") {
			initShields = 3;
		}
		else if (saveData.character == "weak12") {
			initShields = 12;
		}
		if (saveData.leftHanded) {
		}
	}
	void Start(){
	}
	// Update is called once per frame
	void Update () {
		if (numDead / (level * 3) > 0) {
			//StartCoroutine (tempSpawnSpeedup ());
            level++;
            StartCoroutine(levelPopup());
            numDead /= 2;
			spawnPowerUp (0);
			
		}

        
	}
	public void restart(){
		SceneManager.LoadScene ("Scenes/Main");
	}
	public void quitToMenu(){
		SceneManager.LoadScene ("menu");
	}
	public void SwitchChar(){
		if (saveData.character == "default") {
			saveData.character = "weak12";
		} else {
			saveData.character = "default";
		}
	}
	public void Save(){
		BinaryFormatter bf = new BinaryFormatter ();
		FileStream file = File.Create (Application.persistentDataPath + "/playerInfo.dat");

		saveData.coinBank += coinTot;
		if (score > saveData.highscore) {
			saveData.highscore = score;
			newHighscore = true;
		}
		//Debug.Log (saveData.highscore);
		bf.Serialize (file, saveData);
		file.Close ();
	}

	public void Load(){
		if (File.Exists (Application.persistentDataPath + "/playerInfo.dat")) {
			BinaryFormatter bf = new BinaryFormatter ();
			FileStream file = File.Open (Application.persistentDataPath + "/playerInfo.dat", FileMode.Open);
			saveData = (PlayerData)bf.Deserialize (file);
			file.Close ();
		} else {
			saveData.coinBank = 0;
			saveData.character = "default";

			saveData.leftHanded = false;
			saveData.highscore = 0;
		}
		//Debug.Log ("coinBank: " + saveData.coinBank.ToString());
	}

	[Serializable]
	public class PlayerData{
		public long coinBank;
		public string character;
		public bool leftHanded;
		public int highscore;
	}
	IEnumerator tempSpawnSpeedup(){
		float temp = spawnSpeed;
		spawnSpeed = 0.2f;
		yield return new WaitForSeconds(3f);
		spawnSpeed = temp;

	}
	public void Continue(){
		StartCoroutine (PlayerScript.instance.nuke ());
		StartCoroutine (ContinuePanelScript.instance.TurnOffPanel ());
		isContinue = true;
	}
	public void PlayerDied(){

		//SwitchChar ();
		StartCoroutine(waitForRestart ());
		InterfaceOff ();
	}
	IEnumerator waitForRestart(){
		yield return new WaitForSeconds(2f);
		if (hasContinued == false && score > 100) {
			hasContinued = true;
			int rand = UnityEngine.Random.Range (0, 20);
			if (rand == 0) {
				StartCoroutine (ContinuePanelScript.instance.TurnOnPanel (0));
				yield return new WaitForSeconds (5.5f);
			} else if (1 <= rand && rand <= 3) {
				if (saveData.coinBank > 100) {
					StartCoroutine (ContinuePanelScript.instance.TurnOnPanel (1));
					yield return new WaitForSeconds (5.5f);
				}
			} else if (4 <= rand && rand <= 8) {
				StartCoroutine (ContinuePanelScript.instance.TurnOnPanel (2));
				yield return new WaitForSeconds (5.5f);
			}
		}

		if (isContinue == false) {
			Save ();
			StartCoroutine (ContinuePanelScript.instance.TurnOffPanel());
			StartCoroutine (GameOverPanelScript.instance.TurnOnPanel ());
			//SceneManager.LoadScene ("menu");
		} else {
			isContinue = false;
		}
	}

	public void spawnProj(int projType, Vector3 pos){
		GameObject tempProj = Instantiate(Resources.Load<GameObject>("Prefabs/Proj" + projType.ToString()), pos, Quaternion.Euler(0, 0, 0));
		tempProj.name = "proj" + projType.ToString () + "_" + numProj.ToString ();
		projArr.Add (tempProj);
		numProj++;
	}

	public void deleteProj(GameObject other){
		numDead++;
		numDeadInRow++;
		score += numDeadInRow;
        setScoreText();
		numProj--;
		if(UnityEngine.Random.Range(0,coinSpawnOdds) == 0){
			for (int i = 0; i < UnityEngine.Random.Range (1, 3); i++) {
				GameObject tempCoin = Instantiate(Resources.Load<GameObject>("Prefabs/Coin"));
				tempCoin.transform.position = other.transform.position;
				tempCoin.GetComponent<Rigidbody2D> ().velocity = new Vector2 (UnityEngine.Random.Range(-5, 5), UnityEngine.Random.Range(-5, 5));
			}
		}
		projArr.Remove (other);
		GameControl.Destroy (other);
	}

	public void spawnPowerUp(int type)
	{
		GameObject tempPwrUp = Instantiate(Resources.Load<GameObject>("Prefabs/PowerUp"));
		tempPwrUp.transform.position = new Vector3(UnityEngine.Random.Range(minX, maxX), UnityEngine.Random.Range(minY, maxY), 0);
		tempPwrUp.GetComponent<powerUp>().PowerUpNumber = UnityEngine.Random.Range(0, nmbrOfPowerUps-1);
		powArr.Add(tempPwrUp);
		numPow++;

	}

    public void setScoreText()
    {
        scoreText.text = "Score: " + score.ToString();
    }
	public void setCoinText()
	{
		
		coinText.text = "Coins: " + coinTot.ToString();
	}

    IEnumerator levelPopup()
    {
        levelText.text = "LEVEL " + level.ToString();
		float alphaIncrement = 1f/(maxfont - minFont);

        for(int i =0; i < maxfont-minFont; i++)
        {
            levelText.fontSize++;
			levelText.color = new Color(1f,1f,1f,1f - alphaIncrement*i);
            yield return new WaitForSeconds(0.01f);
        }
        levelText.text = "";
        levelText.fontSize = minFont;
    }

	public void ShowRewardedAd()
	{
		if (Advertisement.IsReady("rewardedVideo"))
		{
			Time.timeScale = 0;

			var options = new ShowOptions { resultCallback = HandleShowResult };
			Advertisement.Show("rewardedVideo", options);
		}
	}

	private void HandleShowResult(ShowResult result)
	{
		Time.timeScale = 1;
		switch (result)
		{
		case ShowResult.Finished:
			Debug.Log ("The ad was successfully shown.");
			StartCoroutine (waitAndUnkill());
			break;
		case ShowResult.Skipped:
			Debug.Log("The ad was skipped before reaching the end.");
			break;
		case ShowResult.Failed:
			Debug.LogError("The ad failed to be shown.");
			break;
		}
	}

	public IEnumerator waitAndUnkill(){
		yield return new WaitForSeconds (0.5f);
		StartCoroutine(ContinuePanelScript.instance.TurnOffPanel ());
		yield return new WaitForSeconds (0.5f);
		PlayerScript.instance.Unkill ();
	}
	public void InterfaceOff(){
		interfaces = GameObject.FindGameObjectsWithTag("Interface");
		foreach (GameObject i in interfaces) {
			i.SetActive(false);
		}
	}
	public void InterfaceOn(){
		foreach (GameObject i in interfaces) {
			i.SetActive(true);
		}
	}
}