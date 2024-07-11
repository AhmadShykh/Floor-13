using System.Collections;
using System.Collections.Generic;
using System.Xml.Serialization;
using Unity.VisualScripting;
using UnityEditor.Compilation;
using UnityEditor.EditorTools;
using UnityEditor.Rendering;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.Rendering;

public class Movement : MonoBehaviour
{
    [Header("Camera Rotation Settings")]
    [SerializeField] float mouseSensitivity;
    [SerializeField] float xDegree;
    [SerializeField] float yDegree;
    [SerializeField] float rotateBodyDegree;
    [SerializeField] GameObject head;
    [SerializeField] GameObject body;
    [SerializeField] float rotationSpeed;


	[Header("Movement Settings")]
	[SerializeField] float speed;
    [SerializeField] CharacterController characterController;
    [SerializeField] float y;

    [Header("Animation Settings")]
    [SerializeField] Animator characterAnimator;

	float _yRotation = 0;
    float _xRotation = 0;
	int direction = 0;
	float rotateVal = 0;

    Quaternion _strafeRotation = Quaternion.identity;
    Quaternion _roundRotation = Quaternion.identity;
    // Start is called before the first frame update
    void Start()
    {
        Cursor.lockState = CursorLockMode.Locked;
    }

    // Update is called once per frame
    void Update()
    {
		// Head Camera Movement
		// Get mouse input
		float mouseX = Input.GetAxis("Mouse X") * mouseSensitivity * Time.deltaTime;
		float mouseY = Input.GetAxis("Mouse Y") * mouseSensitivity * Time.deltaTime;
        
		// Movement
		float xAxis = Input.GetAxis("Horizontal") * Time.deltaTime;
		float yAxis= Input.GetAxis("Vertical") * Time.deltaTime;

        _xRotation -= mouseY;
        _yRotation += mouseX;

        _yRotation = WrapAngle(_yRotation);
        _xRotation = Mathf.Clamp(_xRotation,-xDegree,70);

        // Head Movement
		head.transform.localRotation = Quaternion.Euler(_xRotation, _yRotation, 0);

		// Movement direction
		Vector3 movement = new Vector3(xAxis, 0f, yAxis);
		
		// Rotating Movement direction wrt Head
		movement = Quaternion.Euler(0,head.transform.eulerAngles.y,0) * movement;

		// Angle for body while moving
		float strafeAngle = Mathf.Rad2Deg * Mathf.Atan2(movement.x, movement.z);


		if (strafeAngle < 0)
			strafeAngle += 360;

		// Rotating body in backwards direction if the body and head direction does not match
		
		float strafeHeadAngleDiff = Mathf.Abs(head.transform.rotation.eulerAngles.y - strafeAngle );

		if(strafeHeadAngleDiff > 100 && strafeHeadAngleDiff < 260)
		{
			strafeAngle = Mathf.Rad2Deg * Mathf.Atan2(-movement.x,- movement.z);
		}


		if (xAxis != 0 || yAxis != 0)
			_strafeRotation = Quaternion.Euler(0, strafeAngle, 0);
		else
			_strafeRotation = Quaternion.identity;	
		

        //Rotating Object in movement direction
		transform.Translate(movement * speed * Time.deltaTime);

        float hBYAngleDiff = head.transform.localEulerAngles.y - body.transform.localEulerAngles.y;

        hBYAngleDiff = WrapAngle(hBYAngleDiff);


        if(hBYAngleDiff < -yDegree || hBYAngleDiff > yDegree)
        {
			direction = (int)(hBYAngleDiff / Mathf.Abs(hBYAngleDiff));
			rotateVal = body.transform.localEulerAngles.y + direction * rotateBodyDegree;
			
			_roundRotation = Quaternion.Euler(0,rotateVal, 0);
		}

		// Rotate the object around if not strafing 

		Quaternion targetRotation = Quaternion.identity;

		if (_strafeRotation != Quaternion.identity)
			targetRotation = _strafeRotation;
		else
			targetRotation = _roundRotation;	
		

		body.transform.rotation = Quaternion.Slerp(body.transform.rotation, targetRotation, Time.deltaTime * rotationSpeed);

	}



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
