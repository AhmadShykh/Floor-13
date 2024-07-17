using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerManager 
{
    
	
	private static PlayerManager instance;
    public static PlayerManager Instance 
    { 
        get 
        {
			if (instance == null)
			{
				instance = new PlayerManager();
			}
			return instance; 
        } 
    }
    private PlayerManager() 
    {
        playerXDirection = PlayerDirection.Zero;
        playerYDirection = PlayerDirection.Zero;
    }

    public PlayerDirection playerXDirection;
    public PlayerDirection playerYDirection;
    
    public Quaternion prevRot;
    public void InitialzeManager()
    {
        
    }


    public void SetDirection(PlayerDirection x, PlayerDirection y)
    {
        playerYDirection = y;
        playerXDirection = x;
        prevRot = Quaternion.identity;
    }


    
}

public enum PlayerDirection
{
	Positive = 1,
	Negative = -1,
	Zero = 0
}