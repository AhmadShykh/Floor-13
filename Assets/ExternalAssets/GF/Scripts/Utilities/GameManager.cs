﻿using UnityEngine;
using System.Collections;

public class GameManager {

	private static GameManager instance;
	
	private GameManager() { }
	
	public static GameManager Instance
	{
		get
		{
			if (instance == null)
			{
				instance = new GameManager();
			}
			return instance;
		}
	}

    public bool Initialized = false;
    public int CurrentLevel = 1;
    public int CurrentPlayer = 1;
	public string GameStatus;
	public int Objectives;
	public int SessionStatus = 0;
	public bool SessionAd = false;

    public void TaskComplete() {
		if(Objectives > 0)
       		Objectives--;
        GameObject.FindGameObjectWithTag("GameController").GetComponent<GF_GameController>().OnLevelCheck(0);
    }

	public void GameLoose(int reasonIndex=0){
		if (GameStatus != "Loose"){
			//Cursor.lockState = CursorLockMode.None;

			GameStatus = "Loose";
			GameObject.FindGameObjectWithTag ("GameController").GetComponent<GF_GameController> ().OnLevelCheck (reasonIndex);
		} else{
			Debug.LogWarning ("Game loose being called multiple times !");
		}
	}

	public void switchplayer(int index, bool active)
	{
		GameObject.FindGameObjectWithTag("gamecontroller").GetComponent<GF_GameController>().SwitchPlayer(index, active);
	}

	public void PauseTimer(){
		GameObject.FindGameObjectWithTag ("GameController").GetComponent<GF_GameController> ().TimerPaused = true;
	}



	public void ResumeTimer(){
		GameObject.FindGameObjectWithTag ("GameController").GetComponent<GF_GameController> ().TimerPaused = false;
	}

    public void UpdateInventory() {
        //Give items to player here
    }

	public void PauseGame()
	{
		GameObject.FindGameObjectWithTag("GameController").GetComponent<GF_GameController>().PauseGame();
	}

	public void ResumeGame()
	{
		GameObject.FindGameObjectWithTag("GameController").GetComponent<GF_GameController>().ResumeGame();
	}


}
