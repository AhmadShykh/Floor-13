using CodeMonkey.HealthSystemCM;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BatteryHealthSystemComponent : MonoBehaviour, IGetHealthSystem
{

	public float batteryAmountMax = 100f;
	public bool batterOn = false;

	private HealthSystem healthSystem;
	
	private ElectricTorchOnOff _torchComponent;


	private void Awake()
	{
		_torchComponent = GetComponentInChildren<ElectricTorchOnOff>();
		// Create Health System
		float batteryAmountMax = _torchComponent.intensityLight;
		healthSystem = new HealthSystem(batteryAmountMax);


		// Setting Batter Health To Player
		PlayerManager.Instance.InitializeBatteryHealth(this);
	}

	void SetBatterMaxHealth(float value)
	{
		batteryAmountMax = value;
		healthSystem.SetHealthMax(batteryAmountMax, true);
	}


	private void Update()
	{
		if (batterOn)
		{
			healthSystem.SetHealth(_torchComponent.intensityLight);
		}
		
	}

	public void BatterPowerPickup(BatteryPowerPickup battery)
	{
		if(_torchComponent.intensityLight <= 0)
		{
			SetBatterMaxHealth(battery.PowerIntensityLight);
			Debug.Log(_torchComponent);
			_torchComponent.SetBatteryPicked(battery);

		}
	}



	/// <summary>
	/// Get the Health System created by this Component
	/// </summary>
	public HealthSystem GetHealthSystem()
	{
		return healthSystem;
	}


}

