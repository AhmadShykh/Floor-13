using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Locker : MonoBehaviour, InteractableObject
{

	[SerializeField] GameObject objCamera;



	public bool lockerOpen = false;
	
	//GameObject _player;
	private bool _objCamBool;


	// Start is called before the first frame update
	void Start()
    {
		_objCamBool = false;
		objCamera.SetActive(_objCamBool);
    }

    // Update is called once per frame
 //   void Update()
 //   {
	//	if (Input.GetKeyDown(PlayerInteract.interactingKey) && _objCamBool)
	//	{
	//		ToggleCamera();
	//	}
	//}


	void SetCamera(bool objectCameraBool)
	{

		objCamera.SetActive(objectCameraBool);
		if (objectCameraBool)
		{
			Cursor.lockState = CursorLockMode.None;
			PlayerManager.Instance.UpdatePlayerState(PlayerState.Interacting);
		}
		else
		{
			PlayerManager.Instance.UpdatePlayerState(PlayerState.Default);
		}
	}
	public void ToggleCamera()
	{
		Debug.Log($"{lockerOpen} {_objCamBool}");
		if(!lockerOpen )
		{
			_objCamBool = !_objCamBool;
			SetCamera(_objCamBool);
		}
		else
		{
			Debug.Log("Locker Opened");
			GetComponent<Animator>().Play("SafeOpen");
		}
	}



	public void Interacting()
	{
		//Debug.Log("Locker");
		ToggleCamera();
	}

	public void NotInteracting()
	{
		//Debug.Log("Locker");
		ToggleCamera();
	}
}
