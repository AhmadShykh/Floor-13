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
	[SerializeField] GameObject[] CanvasObjects;

	[Header("Canvas Childs")]

	[Tooltip("Collectable Canvas Children")]
	[SerializeField] GameObject collectableDescription;
	[SerializeField] GameObject collectableBtnsPanel;

	[Header("Prefabs")]

	[SerializeField] GameObject collectableBtn;
	[SerializeField] Sprite[] collectableSprites;


    private GameObject _player;
	bool canvasShowing = false;


	private void Awake()
	{
		// Initializing Collectables Manager && Canvas Manager 
		CollectablesManager manager = CollectablesManager.Instance;
		CanvasManager canvasManager = CanvasManager.Instance;
		
		CanvasManager.Instance.OnCanvasChanged += CanvasChanged;

		_player = GameObject.FindGameObjectWithTag("Player");
		
	}

	private void CanvasChanged(CanvasTypes type)
	{


		if(type == CanvasTypes.None)
		{
			foreach(GameObject canvas in  CanvasObjects)
			{
				canvas.SetActive(false);
			}
		}
	}

	private void Start()
	{
		InitializeCollectableCanvas();
	}

	private void Update()
	{
		HandleInputs();
	}

	private void HandleInputs()
	{
		if (Input.GetKeyDown(KeyCode.C) )
		{
			if(CanvasManager.Instance.currentCanvas == CanvasTypes.CollectablesCanvas)
				CanvasManager.Instance.UpdateGameCanvas(CanvasTypes.None);
			else
				CanvasManager.Instance.UpdateGameCanvas(CanvasTypes.CollectablesCanvas);
		}
	}

	void AddCollectable(Collectable collectable)
	{
		GameObject button = Instantiate(collectableBtn, collectableBtnsPanel.transform);
		button.GetComponentInChildren<TextMeshProUGUI>().text = collectable.collectableName;
		button.GetComponentInChildren<Image>().sprite = collectableSprites[(int)collectable.type];

		// Assign a listener to handle the click event
		button.GetComponent<Button>().onClick.AddListener(() => ShowDescription(collectable));


		textCanvas.transform.Find("TextPanel/TextArea").GetComponent<TextMeshProUGUI>().text = collectable.collectableText;

		ToggleTextCanvas(true);

	}


	void InitializeCollectableCanvas()
	{
		// Generate buttons for each collectable
		foreach (Collectable collectable in CollectablesManager.Instance.collectables)
		{
			GameObject button = Instantiate(collectableBtn, collectableBtnsPanel.transform);
			button.GetComponentInChildren<TextMeshProUGUI>().text = collectable.collectableName;
			button.GetComponent<Image>().sprite = collectableSprites[(int)collectable.type];

			// Assign a listener to handle the click event
			button.GetComponent<Button>().onClick.AddListener(() => ShowDescription(collectable));
		}
	}

	void ShowDescription(Collectable collectable)
	{
		collectableDescription.SetActive(true);
		collectableDescription.GetComponentInChildren<TextMeshProUGUI>().text = collectable.collectableText;
	}

	public void ToggleCollectableCanvas(bool active)
	{
		if (active)
		{
			collectableDescription.SetActive(!active);
		}
		canvasShowing = active;

		collectablesCanvas.SetActive(active);
		DefaultObjsValue(active);
	}

	public void ToggleTextCanvas(bool active)
	{
		canvasShowing = active;
		textCanvas.SetActive(active);
		DefaultObjsValue(active);
	}


	private void DefaultObjsValue(bool active)
	{
		
			_player.SetActive(!active);
			if(active)
				Cursor.lockState = CursorLockMode.None;
			else
				Cursor.lockState = CursorLockMode.Locked;

	}

}
