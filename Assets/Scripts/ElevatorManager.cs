using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ElevatorManager : Singleton<ElevatorManager>
{
	public bool isLocked;
	public int[] correctPassword ; 
	public int[] currentDigits ;
	public int currentFloor;
	public int indx = 0;

	private void Start()
	{
		currentDigits = new int[4];
		isLocked = false;
	}

	public override void Awake()
	{
	
		if (instance == null)
		{
			instance = this ;
		}
		else
		{
			Destroy(gameObject);
		}
	}

	public void SetCurrentDigit(int digit)
	{
		if (isLocked)
		{
			currentDigits[indx] = digit;
			indx++;
			if(indx >= 4)
			{
				indx = 4;
				currentDigits = new int[4];
			}
		}
		else
		{
			FindObjectOfType<GF_GameController>().CallElevatorSequence();
		}
	}
}
