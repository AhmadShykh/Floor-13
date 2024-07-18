using System;
using System.Collections;
using System.Collections.Generic;
using System.Xml.Serialization;
using TMPro;
using Unity.VisualScripting;
using UnityEditor;
using UnityEditor.Compilation;
using UnityEditor.EditorTools;
using UnityEditor.Rendering;
using UnityEditorInternal;
using UnityEngine;
using UnityEngine.Animations;
using UnityEngine.EventSystems;
using UnityEngine.Rendering;
using UnityEngine.UIElements;

public class PlayerController : MonoBehaviour
{
    [Header("Camera Rotation Settings")]
    [SerializeField] float mouseSensitivity;
    [SerializeField] float xDegree;
    [SerializeField] float yDegree;
    [SerializeField] GameObject head;
    [SerializeField] GameObject body;


	[Header("Movement Settings")]
	[SerializeField] float accelDeaccel;
	[SerializeField] float speed;

	[Header("Body Rotation Settings")]
	[SerializeField] float degreePerSec;
	[SerializeField] float rotSpeed;

    [Header("Animation Settings")]
    [SerializeField] Animator characterAnimator;

	float _yRotation = 0;
    float _xRotation = 0;
	float _yVal = 0;
	float _xVal = 0;

	int direction = 0;
	float rotateVal = 0;

	float _timer = 0;
	float _rotationSec = 0;

	PlayerDirection _currentXDirection = PlayerDirection.Zero;
	PlayerDirection _currentYDirection = PlayerDirection.Zero;

    Quaternion _tarStrafeRot = Quaternion.identity;


    // Start is called before the first frame update
    void Start()
    {
		UnityEngine.Cursor.lockState = CursorLockMode.Locked;
    }

    // Update is called once per frame
    void Update()
    {
		PlayerMovement();		
	}


	private void PlayerMovement()
	{

		HeadRotation();

		// Movement
		float xAxis = Input.GetAxisRaw("Horizontal");
		float yAxis = Input.GetAxisRaw("Vertical");

		Vector3 movement = BodyMovement(xAxis, yAxis);

		// Rotating Movement direction wrt Head
		Vector3 strafeMov = Quaternion.Euler(0, head.transform.eulerAngles.y, 0) * new Vector3(xAxis, 0f, yAxis == 0 && xAxis == 0 ? 1 : yAxis);

		// Angle for body while moving
		float strafeAngle = Mathf.Rad2Deg * Mathf.Atan2(strafeMov.x, strafeMov.z);


		if (strafeAngle < 0)
			strafeAngle += 360;

		float strafeHeadAngleDiff = Mathf.Abs(head.transform.rotation.eulerAngles.y - strafeAngle);

		float forceValue = movement.magnitude;

		if (strafeHeadAngleDiff > 100 && strafeHeadAngleDiff < 260)
		{
			forceValue *= -1;
			strafeAngle = Mathf.Rad2Deg * Mathf.Atan2(-strafeMov.x, -strafeMov.z);
		}

		//Debug.Log(forceValue);
		
		characterAnimator.SetFloat("Blend", forceValue);


		// Rotating body in backwards direction if the body and head direction does not match


		_currentYDirection = (PlayerDirection)yAxis;
		_currentXDirection = (PlayerDirection)xAxis;

		float hBYAngleDiff = head.transform.localEulerAngles.y - body.transform.localEulerAngles.y;

		hBYAngleDiff = WrapAngle(hBYAngleDiff);

		//float multiplier = 1;

		if (xAxis != 0 || yAxis != 0)
		{
			_tarStrafeRot = Quaternion.Euler(0, strafeAngle, 0);
		}

		if (_currentXDirection != PlayerManager.Instance.playerXDirection || _currentYDirection != PlayerManager.Instance.playerYDirection || hBYAngleDiff > yDegree || hBYAngleDiff < -yDegree)
		{
			PlayerManager.Instance.playerXDirection = _currentXDirection;
			PlayerManager.Instance.playerYDirection = _currentYDirection;

			PlayerManager.Instance.prevRot = body.transform.rotation;

			if (xAxis == 0 && yAxis == 0)
			{
				_tarStrafeRot = Quaternion.Euler(0, head.transform.eulerAngles.y, 0);
			}

			_timer = 0;
		}

		_rotationSec = (Mathf.Abs(Quaternion.Angle(_tarStrafeRot, PlayerManager.Instance.prevRot))) / degreePerSec;

		_timer += Time.deltaTime;

		//float currentInterpoleVal = multiplier * Mathf.Clamp01(_timer / _rotationSec);

		body.transform.rotation = Quaternion.Slerp(body.transform.rotation, _tarStrafeRot, Time.deltaTime * rotSpeed);

	}

	private Vector3 BodyMovement(float xAxis, float yAxis)
	{
		_xVal = Mathf.Lerp(_xVal, xAxis, Time.deltaTime * accelDeaccel);
		_yVal = Mathf.Lerp(_yVal, yAxis, Time.deltaTime * accelDeaccel);

		// Movement direction
		Vector3 movement = new Vector3(_xVal, 0f, _yVal);

		// Rotating Movement direction wrt Head
		movement = Quaternion.Euler(0, head.transform.eulerAngles.y, 0) * movement;

		//Rotating Object in movement direction
		transform.Translate(movement * speed * Time.deltaTime);
		return movement;
	}

	private bool IsForward()
	{
		if (_currentXDirection == PlayerDirection.Zero && _currentYDirection == PlayerDirection.Zero)
			return true;
		else 
			return false; 
	}

	private void HeadRotation()
	{
		// Head Camera Movement
		// Get mouse input
		float mouseX = Input.GetAxis("Mouse X") * mouseSensitivity * Time.deltaTime;
		float mouseY = Input.GetAxis("Mouse Y") * mouseSensitivity * Time.deltaTime;

		_xRotation -= mouseY;
		_yRotation += mouseX;

		// Head Movement
		_yRotation = WrapAngle(_yRotation);
		_xRotation = Mathf.Clamp(_xRotation, -xDegree, 50);

		head.transform.localRotation = Quaternion.Euler(_xRotation, _yRotation, 0);

	}

	//private (float, float) ReceiveInput()
	//{
	//	// Head Camera Movement
	//	// Get mouse input
	//	float mouseX = Input.GetAxis("Mouse X") * mouseSensitivity * Time.deltaTime;
	//	float mouseY = Input.GetAxis("Mouse Y") * mouseSensitivity * Time.deltaTime;

	//	// Movement
	//	float xAxis = Input.GetAxis("Horizontal");
	//	float yAxis = Input.GetAxis("Vertical");

	//	_xRotation -= mouseY;
	//	_yRotation += mouseX;


	//}

	float WrapAngle(float angle)
	{
		angle = angle % 360;

		if (angle > 180)
			angle -= 360;
		else if (angle < -180)
			angle += 360;
		return angle;
	}
}
