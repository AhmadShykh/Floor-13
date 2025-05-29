using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class EnemyManager1
{

	private static EnemyManager1 instance;

	public static EnemyManager1 Instance
	{
		get
		{
			if (instance == null)
			{
				instance = new EnemyManager1();
			}

			return instance;
		}
	}


	private EnemyManager1()
	{

	}


	public EnemyStates1 enemyCurrentState;
	public EnemyStates1 enemyPreivousState;
    public Action<EnemyStates1> EnemyStateChange;
    
    public EnemyController1 enemyController1 { get; set; }

   


    public void UpdateEenemyState(EnemyStates1 state)
    {
	    enemyPreivousState = enemyCurrentState;
	    enemyCurrentState = state;
	    EnemyStateChange?.Invoke(state);
    }

}


public enum EnemyStates1
{
	Inactive,
	Investigate, 
	Idle,
	Waiting,
	Patrol,
	Hunt,
	Search,
	Kill

    
}