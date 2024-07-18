using System;
using System.Collections;
using System.Collections.Generic;
using Unity.PlasticSCM.Editor.WebApi;
using Unity.VisualScripting;
using UnityEditor.MPE;
using UnityEngine;
using UnityEngine.Rendering.UI;
using UnityEngine.Rendering.Universal;

public class PlayerInteract : MonoBehaviour
{
    public static KeyCode interactingKey = KeyCode.F;
	[SerializeField] float _raycastLength;

	// Those Objects which might be effected by a certain raycast hit
	//[Header("Interacted Object")]
	//[SerializeField] GameObject torch;

	string[] interactingObjs = { "HidingObject", "Battery","Collectables" };

	string[] highlightObjs = { "Battery", "Collectables" };

	GameObject previousObj = null;

	private void Update()
	{
		RayCastCheck();
	}

	private void RayCastCheck()
	{
		Ray torchRay = new Ray(transform.position, transform.forward);

		// Draw the ray in the Scene view for debugging purposes
		Debug.DrawRay(torchRay.origin, torchRay.direction * _raycastLength, Color.red);

		if (Physics.Raycast(torchRay, out RaycastHit objHit, _raycastLength))
		{
			GameObject hitGameobject = objHit.collider.gameObject;
			if(Array.Exists(interactingObjs,objTag => objTag == hitGameobject.tag))
			{
				//Debug.Log($"Object Collided with: {hitGameobject.tag}");
				if (Array.Exists(highlightObjs, objTag => objTag == hitGameobject.tag))
				{
					hitGameobject.GetComponent<Renderer>().material.color = Color.yellow;
				}
				if (Input.GetKeyDown(interactingKey))
					hitGameobject.GetComponent<InteractableObject>().InitiateInteractingSequence();
			}



			// Changing back battery color if its not look at by player
			if (previousObj != null && Array.Exists(highlightObjs, objTag => objTag == previousObj.tag) && previousObj != hitGameobject.gameObject)
			{
				previousObj.GetComponent<Renderer>().material.color = Color.white;
			}
			
			previousObj = hitGameobject.gameObject;
		}
		else if (previousObj != null && Array.Exists(highlightObjs, objTag => objTag == previousObj.tag))
			previousObj.GetComponent<Renderer>().material.color = Color.white;
	}

}
