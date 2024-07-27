using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using System;
using UnityEngine.SubsystemsImplementation;

public class Collectable : CollectableBase,InteractableObject
{
    
    //private GameObject _textShownCanvas;
    //private GameObject _player;

	private void Awake()
	{
        //_textShownCanvas = GameObject.FindGameObjectWithTag("TextCanvas");
        //_player = GameObject.FindGameObjectWithTag("Player");
	}
	private void Start()
	{

	}

	public void Interacting()
	{
		PlayerManager.Instance.UpdatePlayerState(PlayerState.Reading);
		CollectablesManager.Instance.AddCollectable(this);

		if (Array.Exists(destroyable, obj => obj == type))
		{
			Destroy(gameObject);
		}
		
		//CanvasManager.Instance.UpdateGameCanvas(CanvasTypes.CollectableDescription);
		//PlayerManager.Instance.UpdatePlayerState(PlayerState.Reading);
	}

	public void NotInteracting()
	{
		Debug.Log("Collectables dont uninteract");
		// Colletables Will do nothing if not interacting
		return;
	}

}

public enum CollectableTypes
{
	Paper = 0,
	Trophy = 1,
	Key = 2
}