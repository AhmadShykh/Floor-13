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
	[SerializeField] GameObject _mainCamera;

	// Those Objects which might be effected by a certain raycast hit
	//[Header("Interacted Object")]
	//[SerializeField] GameObject torch;

	string[] interactingObjs = { "HidingObject", "Battery","Collectables" ,"Safe"};

	string[] highlightObjs = { "Battery", "Collectables" };

	GameObject previousObj = null;
	InteractableObject _interactedWith;

	private void Update()
	{
		RayCastCheck();
	}

	private void RayCastCheck()
	{
		Ray torchRay = new Ray(_mainCamera.transform.position, _mainCamera.transform.forward);

		bool interacted = Input.GetKeyDown(interactingKey);

		if(interacted && _interactedWith != null)
		{
			_interactedWith.NotInteracting();
			_interactedWith = null;
		}

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
				if (interacted)
				{
					_interactedWith = hitGameobject.GetComponent<InteractableObject>();
					_interactedWith.Interacting();

				}
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
