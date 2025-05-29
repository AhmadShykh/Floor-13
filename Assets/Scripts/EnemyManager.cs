using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class EnemyManager
{

	private static EnemyManager instance;

	public static EnemyManager Instance
	{
		get
		{
			if (instance == null)
			{
				instance = new EnemyManager();
			}

			return instance;
		}
	}


	private EnemyManager()
	{

	}


	public EnemyStates enemyCurrentState;
    public Action<EnemyStates> EnemyStateChange;

   


    public void UpdateEenemyState(EnemyStates state)
    {
        enemyCurrentState = state;



        EnemyStateChange?.Invoke(enemyCurrentState);   
    }

}


public enum EnemyStates
{
	Waiting,
	Patroling,
    Chasing,
    Hiding
    
}