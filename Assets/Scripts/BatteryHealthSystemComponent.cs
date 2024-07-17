using CodeMonkey.HealthSystemCM;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BatteryHealthSystemComponent : MonoBehaviour, IGetHealthSystem
{

	public float batteryAmountMax = 100f;
	private HealthSystem healthSystem;
	private ElectricTorchOnOff _torchComponent;


	private void Awake()
	{
		// Create Health System
		float batteryAmountMax = GetComponent<ElectricTorchOnOff>().intensityLight;
		healthSystem = new HealthSystem(batteryAmountMax);

		ElectricTorchOnOff.BatterPickedUp += SetBatterMaxHealth;

		_torchComponent = GetComponent<ElectricTorchOnOff>();
	}

	void SetBatterMaxHealth(float value)
	{
		batteryAmountMax = value;
		healthSystem.SetHealthMax(batteryAmountMax, true);
	}


	private void Update()
	{
		healthSystem.SetHealth(_torchComponent.intensityLight);
		
	}



	/// <summary>
	/// Get the Health System created by this Component
	/// </summary>
	public HealthSystem GetHealthSystem()
	{
		return healthSystem;
	}


}

