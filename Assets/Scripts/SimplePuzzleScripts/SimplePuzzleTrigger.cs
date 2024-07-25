using PuzzleSystem;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SimplePuzzleTrigger : CorePuzzleTrigger, InteractableObject
{
	public void Interacting()
	{
		TriggerImpl();
		gameObject.SetActive(false);
	}

	public void NotInteracting()
	{
		throw new System.NotImplementedException();
	}

	//// Start is called before the first frame update
	//void Start()
 //   {
        
 //   }

 //   // Update is called once per frame
 //   void Update()
 //   {
        
 //   }
}
