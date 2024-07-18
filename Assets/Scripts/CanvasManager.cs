using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CanvasManager 
{
	private static CanvasManager instance;
	
	public static CanvasManager Instance
	{
		get
		{
			if (instance == null)
			{
				instance = new CanvasManager();
				instance.UpdateGameCanvas(CanvasTypes.None);

			}
			return instance;
		}
	}

	public CanvasTypes currentCanvas;
	public Action<CanvasTypes> OnCanvasChanged;

	public void UpdateGameCanvas(CanvasTypes newCanvas)
	{

		currentCanvas = newCanvas;

		OnCanvasChanged?.Invoke(currentCanvas);

	}
	
	public GameObject activeCanvas;
	private CanvasManager()
	{
		
	}




}

public enum CanvasTypes
{
	None = 0,
	CollectableDescription = 1,
	CollectablesCanvas = 2,
}
