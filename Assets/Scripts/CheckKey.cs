using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class CheckKey : MonoBehaviour, InteractableObject
{
    public string keyName;
	[SerializeField] GameObject textPrefab;
 
	public void Interacting()
	{
		Collectable found = null;
		foreach (Collectable collectable in CollectablesManager.Instance.collectables)
		{
			if (collectable.name == keyName)
			{
				found = collectable;
			}
		}

		if (found != null)
		{
			CollectablesManager.Instance.collectables.Remove(found);
			Debug.Log("Key Found Open Elevator");
		}
		else
		{
			Debug.Log("Key Could Not be found");
		}

	}

	public void NotInteracting()
	{
		throw new System.NotImplementedException();
	}
}
