using System;
using Unity.VisualScripting;
using UnityEngine;

public class PlayerController : MonoBehaviour
{

	//[Header("General Settings")]
	//[SerializeField] GameObject player;


	[Header("Camera Rotation Settings")]
    [SerializeField] float mouseSensitivity;
    [SerializeField] float xDegree;
    [SerializeField] float yDegree;
    [SerializeField] GameObject head;
    [SerializeField] GameObject body;
	public GameObject camera;


	[Header("Movement Settings")]
	[SerializeField] float accelDeaccel;
	[SerializeField] float speed;
	[SerializeField] GameObject[] bodyMesh;

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


	private void Awake()
	{
		// Initializing Player Manager
		PlayerManager instance = PlayerManager.Instance;

		//PlayerManager.Instance.playerStateChangeEvent += PlayerChangeState;
	}

	// Start is called before the first frame update
	void Start()
    {
		UnityEngine.Cursor.lockState = CursorLockMode.Locked;

		//camera = GameObject.FindGameObjectWithTag("MainCamera");
    }

	//void PlayerChangeState(PlayerState state)
	//{
	//	//Debug.Log("State Change: " +state);

	//	ResetAnimation();
		
	//	if (state == PlayerState.Default)
	//	{
	//		SetPlayerDefault(true);
	//		camera.gameObject.SetActive(true);


	//		//PlayerManager.Instance.currentPlayerObject.SetActive(true);

	//	}
	//	else if (state == PlayerState.Hidden)
	//	{
	//		SetPlayerDefault(false);
	//	}
	//	else if(state == PlayerState.Interacting )
	//	{
	//		camera.gameObject.SetActive(false);
	//		PlayerManager.Instance.isMoving = false;
	//		PlayerManager.Instance.isRaycasting = false;
	//		Cursor.lockState = CursorLockMode.None;
	//	}
	//	//else if(state == PlayerState.Reading)
	//	//{
	//	//	isMoving = false;
	//	//}
	//	//Debug.Log(PlayerManager.Instance.isMoving);

	//}

	private void ResetAnimation()
	{
		_xVal = 0;
		_yVal = 0;
		characterAnimator.SetFloat("Blend", 0);

	}


	// Switching between player complete default to opposite 
	void SetPlayerDefault(bool val)
	{
		PlayerManager.Instance.isMoving = val;
		PlayerManager.Instance.isRaycasting = val;
		PlayerManager.Instance.currentPlayerObject.SetActive(val);
		Cursor.lockState = CursorLockMode.Locked;
	}

	//public void SetPlayerInactive(bool val)
	//{
	//	gameObject.SetActive(val);
	//}

	public void UpdatePlayerState(PlayerState state)
	{
		PlayerManager.Instance.UpdatePlayerState(state);
	}


	private void HidePlayer(bool val)
	{
		foreach (GameObject obj in bodyMesh)
			obj.SetActive(!val);
	}


	// Update is called once per frame
	void Update()
    {
		if(PlayerManager.Instance.isMoving)
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

		//Moving Object 
		PlayerManager.Instance.currentPlayerObject.transform.Translate(movement * speed * Time.deltaTime);


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
