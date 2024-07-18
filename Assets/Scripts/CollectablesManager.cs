using JetBrains.Annotations;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CollectablesManager 
{
	private static CollectablesManager instance;

	public List<Collectable> collectables = new List<Collectable>();
	public Action<Collectable> AddCollectablesAction;

	public static CollectablesManager Instance
	{
		get
		{
			if (instance == null)
			{
				instance = new CollectablesManager();
			}
			return instance;
		}
	}

	private CollectablesManager()
	{
		// Add Collectables through player pref if loading
	}
	
	public bool AddCollectable(Collectable obj)
	{
		if (collectables.Exists(element => element == obj))
			return false;
		else
		{
			collectables.Add(obj);
			AddCollectablesAction?.Invoke(obj);
			return true;
		}
	}
}
