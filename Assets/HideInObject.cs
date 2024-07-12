using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class HideInObject : MonoBehaviour, InteractableObject
{
    [SerializeField] GameObject objCamera;

    GameObject _player;
    private bool _objCamBool;


	void Start()
    {
        _objCamBool = false; 
        _player = GameObject.FindGameObjectWithTag("Player");
    }

	private void Update()
	{
		if(Input.GetKeyDown(PlayerInteract.interactingKey) && _objCamBool)
        {
            InitiateInteractingSequence();
		}
	}

	void ToggleCamera(bool objectCameraBool)
    {
        
        objCamera.SetActive(objectCameraBool);
        _player.SetActive(!objectCameraBool);
    }
	public void InitiateInteractingSequence()
	{
        _objCamBool = !_objCamBool;
        ToggleCamera(_objCamBool);
	}
}
