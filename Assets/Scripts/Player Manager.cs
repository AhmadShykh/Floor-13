using CodeMonkey.HealthSystemCM;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerManager 
{
    
	
	private static PlayerManager instance;
    private PlayerHealthSystemComponent playerHealth;
    private BatteryHealthSystemComponent batteryHealth;

    public Action<PlayerState> playerStateChangeEvent;
    public PlayerState playerCurrentState;

    //public bool isHidden;
    public PlayerDirection playerXDirection;
    public PlayerDirection playerYDirection;
    
    public Quaternion prevRot;

    public bool isMoving = true;

	public static PlayerManager Instance 
    { 
        get 
        {
			if (instance == null)
			{
				instance = new PlayerManager();
                instance.UpdatePlayerState(PlayerState.Default);
			}
			return instance; 
        } 
    }
    private PlayerManager() 
    {
        playerXDirection = PlayerDirection.Zero;
        playerYDirection = PlayerDirection.Zero;

        //camera = GameObject.FindGameObjectWithTag("MainCamera");


        prevRot = Quaternion.identity;
    }


    public void UpdatePlayerState(PlayerState playerState)
    {
        playerCurrentState = playerState;

        playerStateChangeEvent?.Invoke(playerState);

    }

    public void InitializeHealth(PlayerHealthSystemComponent health)
    {
        playerHealth = health;
    }

	public void InitializeBatteryHealth(BatteryHealthSystemComponent _batteryHealth)
	{
		batteryHealth  = _batteryHealth;
	}

	public void PlayerReceiveDamage(float damage)
    {
        playerHealth.GetHealthSystem().Damage(damage);
    }

    public void TorchPowerInput(bool value)
    {
        Debug.Log(value);  
        batteryHealth.batterOn = value;
    }

    public void BatterPowerPickedup(BatteryPowerPickup battery)
    {
        batteryHealth.BatterPowerPickup(battery);
    }

    public void SetDirection(PlayerDirection x, PlayerDirection y)
    {
        playerYDirection = y;
        playerXDirection = x;
        prevRot = Quaternion.identity;
    }


    
}

public enum PlayerState
{
    Default,
    //Reading,
    Hidden,
    Interacting,
}

public enum PlayerDirection
{
	Positive = 1,
	Negative = -1,
	Zero = 0
}