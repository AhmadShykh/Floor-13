using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ElevatorButtonPress : MonoBehaviour
{
	public int number;
	private void OnMouseDown()
	{
		Debug.Log("MouseDonw");
		ElevatorManager.Instance.SetCurrentDigit(number);	
	}
}
