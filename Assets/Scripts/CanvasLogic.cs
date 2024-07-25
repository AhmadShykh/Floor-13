using System;
using System.Collections;
using System.Collections.Generic;
using System.Net.Sockets;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

public class CanvasLogic : MonoBehaviour
{
	[Header("Canvas Settings")]

	// 0: description, 1: colletable
	[SerializeField] GameObject[] CanvasObjects;

	[Header("Canvas Childs")]

	[Tooltip("Collectable Canvas Children")]
	[SerializeField] GameObject collectableDescription;
	[SerializeField] GameObject collectableBtnsPanel;

	[Header("Prefabs")]

	[SerializeField] GameObject collectableBtn;
	[SerializeField] Sprite[] collectableSprites;


	//private GameObject _player;
	//bool canvasShowing = false;
	int currentCanvas = -1;

	private void Awake()
	{
		// Initializing Collectables Manager && Canvas Manager 
		CollectablesManager collectablesManager = CollectablesManager.Instance;
		//CanvasManager canvasManager = CanvasManager.Instance;

		//CanvasManager.Instance.OnCanvasChanged += CanvasChanged;
		CollectablesManager.Instance.AddCollectablesAction += AddCollectable;

		//_player = GameObject.FindGameObjectWithTag("Player");

	}


	//private void CanvasChanged(CanvasTypes type)
	//{

	//	// If the user was reading then set the player state to default 
	//	// but if the playing was checking out collectables than don't because 
	//	// collectables are displayed in overlay thus could cause multiple player states at once

	//	if (CanvasManager.Instance.prevCanvas != CanvasTypes.None)
	//		ToggleCanvas(false, (int)CanvasManager.Instance.prevCanvas - 1);
	//	else if (CanvasManager.Instance.prevCanvas == CanvasTypes.CollectableDescription)
	//		PlayerManager.Instance.UpdatePlayerState(PlayerState.Default);

	//	if(type == CanvasTypes.None)
	//	{
	//		GameManager.Instance.ResumeGame(); 
	//		PlayerManager.Instance.UpdatePlayerState(PlayerManager.Instance.playerCurrentState);
	//	}
	//	else
	//	{
	//		GameManager.Instance.PauseGame();
	//		Cursor.lockState = CursorLockMode.None;
	//		if (type == CanvasTypes.CollectablesCanvas)
	//		{
	//			ToggleCanvas(true, 1);
	//		}
	//		else if (type == CanvasTypes.CollectableDescription)
	//		{
	//			ToggleCanvas(true, 0);
	//			//PlayerManager.Instance.UpdatePlayerState(PlayerState.Reading);
	//		}

	//	}
		

	//}


	private void ToggleCanvas(bool active, int i)
	{
		CanvasObjects[currentCanvas].SetActive(!active);
		if(i != -1)
			CanvasObjects[i].SetActive(active);
	}

	private void Start()
	{
		// Setting Canvas and Objects Default Value

		collectableDescription.SetActive(false);

		foreach (GameObject canvas in CanvasObjects)
		{
			canvas.SetActive(false);
		}

		InitializeCollectableCanvas();
	}

	private void Update()
	{
		HandleInputs();
	}

	//public void UpdateCanvas(int type)
	//{
	//	CanvasManager.Instance.UpdateGameCanvas((CanvasTypes)type);
	//}

	private void HandleInputs()
	{
		if (Input.GetKeyDown(KeyCode.C) )
		{
			if (currentCanvas == -1)
				ToggleCanvas(true, 1);
			else
				TurnOffCanvas();
		}
	}

	public void TurnOffCanvas()
	{
		ToggleCanvas(true, -1);
		PlayerManager.Instance.UpdatePlayerState(PlayerManager.Instance.playerPreviousState);
	}

	void AddCollectable(Collectable collectable)
	{
		ToggleCanvas(true, 0);

		AddBtnToCollectablesArea(collectable);

			
		CanvasObjects[1].transform.Find("TextPanel/TextArea").GetComponent<TextMeshProUGUI>().text = collectable.collectableText;

		//ToggleTextCanvas(true);

	}

	private void AddBtnToCollectablesArea(Collectable collectable)
	{
		GameObject button = Instantiate(collectableBtn, collectableBtnsPanel.transform);
		button.GetComponentInChildren<TextMeshProUGUI>().text = collectable.collectableName;
		button.GetComponentInChildren<Image>().sprite = collectableSprites[(int)collectable.type];

		// Assign a listener to handle the click event
		button.GetComponent<Button>().onClick.AddListener(() => ShowDescription(collectable));
	}

	void InitializeCollectableCanvas()
	{
		// Generate buttons for each collectable
		foreach (Collectable collectable in CollectablesManager.Instance.collectables)
		{
			AddBtnToCollectablesArea(collectable);
		}
	}

	void ShowDescription(Collectable collectable)
	{
		collectableDescription.SetActive(true);
		collectableDescription.GetComponentInChildren<TextMeshProUGUI>().text = collectable.collectableText;
	}

	//public void ToggleCollectableCanvas(bool active)
	//{
	//	if (active)
	//	{
	//		collectableDescription.SetActive(!active);
	//	}
	//	canvasShowing = active;

	//	collectablesCanvas.SetActive(active);
	//	DefaultObjsValue(active);
	//}

	//public void ToggleTextCanvas(bool active)
	//{
	//	canvasShowing = active;
	//	textCanvas.SetActive(active);
	//	DefaultObjsValue(active);
	//}


	//private void DefaultObjsValue(bool active)
	//{
		
	//	_player.GetComponent<PlayerController>().enabled = active;

	//	if(!active)
	//		Cursor.lockState = CursorLockMode.None;
	//	else
	//		Cursor.lockState = CursorLockMode.Locked;

	//}

}

