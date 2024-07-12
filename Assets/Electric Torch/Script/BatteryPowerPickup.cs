// - BatteryPowerPickup - Script by Marcelli Michele
// Attach this script on a GameObject (battery pickup) with collider component


using UnityEditor.Rendering;
using UnityEngine;

public class BatteryPowerPickup : MonoBehaviour, InteractableObject
{
    ElectricTorchOnOff _torchOnOff;
    //
    public float PowerIntensityLight;

    private void Awake()
    {
        _torchOnOff = FindObjectOfType<ElectricTorchOnOff>();
    }

	public void HighlightObj()
	{
		GetComponent<Renderer>().material.color = Color.yellow;
	}

	public void InitiateInteractingSequence()
	{

		if (_torchOnOff.intensityLight <= 0)
			_torchOnOff.SetBatteryPicked(this);
	}

}
