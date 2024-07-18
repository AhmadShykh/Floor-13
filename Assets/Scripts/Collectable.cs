using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class Collectable : MonoBehaviour,InteractableObject
{
    

	public string collectableName = "";
	public CollectableTypes type;
    public string collectableText;


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

	public void InitiateInteractingSequence()
	{
		CollectablesManager.Instance.AddCollectable(this);
		CanvasManager.Instance.UpdateGameCanvas(CanvasTypes.CollectableDescription);
	}

}

public enum CollectableTypes
{
	Paper = 0,
	Trophy = 1,
}