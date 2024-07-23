using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class HideInObject : MonoBehaviour, InteractableObject
{
    [SerializeField] GameObject objCamera;

    //GameObject _player;
    private bool _objCamBool;


	void Start()
    {
        _objCamBool = false; 
        //_player = GameObject.FindGameObjectWithTag("Player");
    }

	//private void Update()
	//{
	//	if(Input.GetKeyDown(PlayerInteract.interactingKey) && _objCamBool)
 //       {
 //           InitiateInteractingSequence();
	//	}
	//}

	void SetCamera(bool objectCameraBool)
    {
        
        objCamera.SetActive(objectCameraBool);
        if (objectCameraBool)
            PlayerManager.Instance.UpdatePlayerState(PlayerState.Hidden);
        else
            PlayerManager.Instance.UpdatePlayerState(PlayerState.Default);
    }
	public void ToggleCamera()
	{
		_objCamBool = !_objCamBool;
		SetCamera(_objCamBool);
	}

	public void Interacting()
	{
		ToggleCamera();
	}

	public void NotInteracting()
	{
		//Debug.Log("InteractingCalled");
		ToggleCamera();
	}
}
