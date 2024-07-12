// - ElectricTorchOnOff - Script by Marcelli Michele

// This script is attached in primary model (default) of the Electric Torch.
// You can On/Off the light and choose any letter on the keyboard to control it
// Use the "battery" or no and the duration time
// Change the intensity of the light

using CodeMonkey.HealthSystemCM;
using System;
using Unity.VisualScripting;
using UnityEditor.PackageManager;
using UnityEngine;
using UnityEngine.UIElements;

public class ElectricTorchOnOff : MonoBehaviour
{
	EmissionMaterialGlassTorchFadeOut _emissionMaterialFade;
	BatteryPowerPickup _batteryPower;
	HealthSystemComponent _batteryHealthComponent;
	//

	public enum LightChoose
    {
		noBattery,
		withBattery
    }

	public LightChoose modoLightChoose;
	[Space]
	[Space]
	public string onOffLightKey = "F";
	public KeyCode batteryPickupKey = KeyCode.F;
	private KeyCode _kCode;
	[Space]
	[Space]
	public bool _PowerPickUp = false;
	[Space]
	public float intensityLight = 2.5F;
	public float _raycastLenght;

	private bool _flashLightOn = false;
	[SerializeField] float _lightTime = 0.05f;
	public static event Action<float> BatterPickedUp;

	public EventHandler PickedUpBattery;

	private void Awake()
    {
		_batteryPower = FindObjectOfType<BatteryPowerPickup>();
		_batteryHealthComponent = GetComponent<HealthSystemComponent>();
	}
    void Start()
	{
		GameObject _scriptControllerEmissionFade = GameObject.Find("default");

		if (_scriptControllerEmissionFade != null)
		{
			_emissionMaterialFade = _scriptControllerEmissionFade.GetComponent<EmissionMaterialGlassTorchFadeOut>();
		}
		if (_scriptControllerEmissionFade  == null) {Debug.Log("Cannot find 'EmissionMaterialGlassTorchFadeOut' script");}

		_kCode = (KeyCode)System.Enum.Parse(typeof(KeyCode), onOffLightKey);
	}

	void Update()
	{
		// detecting parse error keyboard type
		if (System.Enum.TryParse(onOffLightKey, out _kCode))
		{
			_kCode = (KeyCode)System.Enum.Parse(typeof(KeyCode), onOffLightKey);
		}
        //

        switch (modoLightChoose)
        {
            case LightChoose.noBattery:
				NoBatteryLight();
				break;
            case LightChoose.withBattery:
				WithBatteryLight();
				break;
        }

		// Check for battery pickup

		//CheckBatterPickup();
	}

	//private void CheckBatterPickup()
	//{
	//	Ray torchRay = new Ray(transform.position, transform.forward);

	//	// Draw the ray in the Scene view for debugging purposes
	//	Debug.DrawRay(torchRay.origin, torchRay.direction* _raycastLenght,Color.red);

	//	if (Physics.Raycast(torchRay,out RaycastHit objHit, _raycastLenght))
	//	{
	//		Collider hitGameobject = objHit.collider;
	//		if(hitGameobject.tag == "Battery" )
	//		{
	//			// Example: Change the color of the hit object
	//			Renderer renderer = hitGameobject.GetComponent<Renderer>();
	//			if (renderer != null)
	//			{
	//				renderer.material.color = Color.yellow;
	//			}
	//			if (Input.GetKeyDown(batteryPickupKey) && intensityLight <=0 )
	//			{
	//				_batteryPower = hitGameobject.GetComponent<BatteryPowerPickup>();
	//				_PowerPickUp = true;
	//			}
	//		}
	//	}
	//}

	public void SetBatteryPicked(BatteryPowerPickup battery)
	{
		_batteryPower = battery;
		_PowerPickUp = true;
	}

	void InputKey()
    {
		if (Input.GetKeyDown(_kCode) && _flashLightOn == true)
		{
			_flashLightOn = false;

		}
		else if (Input.GetKeyDown(_kCode) && _flashLightOn == false)
		{
			_flashLightOn = true;

		}
	}

	void NoBatteryLight()
    {
		if (_flashLightOn)
		{
			GetComponent<Light>().intensity = intensityLight;
			_emissionMaterialFade.OnEmission();
		}
		else
		{
			GetComponent<Light>().intensity = 0.0f;
			_emissionMaterialFade.OffEmission();
		}
		InputKey();
	}

	void WithBatteryLight()
    {

		if (_flashLightOn)
		{
			GetComponent<Light>().intensity = intensityLight;
			intensityLight -= Time.deltaTime * _lightTime;
			_emissionMaterialFade.TimeEmission(_lightTime);
            
			if (intensityLight < 0)
            {
				intensityLight = 0;
			}
			//if (_PowerPickUp == true)
			//{
			//	if(_batteryPower )
			//		intensityLight = _batteryPower.PowerIntensityLight;
			//}
		}
		else
		{
			GetComponent<Light>().intensity = 0.0f;
			_emissionMaterialFade.OffEmission();

			//if (_PowerPickUp == true)
			//{
			//	intensityLight = _batteryPower.PowerIntensityLight;
			//}
		}

		if(_batteryPower && _PowerPickUp )
		{
			intensityLight = _batteryPower.PowerIntensityLight;
			BatterPickedUp?.Invoke(intensityLight);
			Destroy(_batteryPower.gameObject);
			_PowerPickUp = false;
		}


		InputKey();
	}
}
