using UnityEngine;

public class PlayerAndCameraController : MonoBehaviour
{
    [Header("Player Movement")]
    public float moveSpeed = 5f;
    public Transform player;

    [Header("Camera Orbit Settings")]
    public Transform cameraPivot; // Usually empty GameObject at player center (head)
    public float distance = 5.0f;
    public float xSpeed = 120.0f;
    public float ySpeed = 120.0f;
    public float yMinLimit = -20f;
    public float yMaxLimit = 80f;
    public float zoomSpeed = 2f;
    public float minDistance = 2f;
    public float maxDistance = 10f;

    private float x = 0.0f;
    private float y = 0.0f;

    private Transform cam;

    public bool playerHid = false;

    void Start()
    {
        cam = Camera.main.transform;

        Vector3 angles = cam.eulerAngles;
        x = angles.y;
        y = angles.x;

        // Optional cursor setup
        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;
    }

    void Update()
    {
        MovePlayer();
        OrbitCamera();
        CustomerTriggers();
    }

    private void CustomerTriggers()
    {
        if(Input.GetKeyDown(KeyCode.U))
            EnemyManager1.Instance.enemyController1.CheckSoundHeardPosition(player.transform.position,false);
        else if(Input.GetKeyDown(KeyCode.I))
            EnemyManager1.Instance.enemyController1.CheckSoundHeardPosition(player.transform.position,true);
        else if (Input.GetKeyDown(KeyCode.H))
        {
            playerHid = !playerHid;
            Debug.Log($"Player Hiding: {playerHid}");
            if(EnemyManager1.Instance.enemyCurrentState == EnemyStates1.Hunt)
                EnemyManager1.Instance.UpdateEenemyState(EnemyStates1.Search);
        }
        else if (Input.GetKeyDown(KeyCode.O))
        {
            EnemyManager1.Instance.enemyController1.SetNavMeshAgentDestination(player.transform.position);
        }
            
    }

    void MovePlayer()
    {
        float h = Input.GetAxis("Horizontal");
        float v = Input.GetAxis("Vertical");

        // Get the camera's forward and right vectors
        Vector3 camForward = cam.forward;
        Vector3 camRight = cam.right;

        // Flatten the vectors (ignore Y axis)
        camForward.y = 0f;
        camRight.y = 0f;
        camForward.Normalize();
        camRight.Normalize();

        // Combine input with camera directions
        Vector3 moveDir = camForward * v + camRight * h;

        player.position += moveDir * moveSpeed * Time.deltaTime;

        // Optional: Rotate the player to face movement direction
        if (moveDir != Vector3.zero)
        {
            Quaternion toRotation = Quaternion.LookRotation(moveDir, Vector3.up);
            player.rotation = Quaternion.Slerp(player.rotation, toRotation, 10f * Time.deltaTime);
        }
    }


    void OrbitCamera()
    {
        if (Input.GetMouseButton(1)) // Right-click to rotate
        {
            x += Input.GetAxis("Mouse X") * xSpeed * Time.deltaTime;
            y -= Input.GetAxis("Mouse Y") * ySpeed * Time.deltaTime;
            y = Mathf.Clamp(y, yMinLimit, yMaxLimit);
        }

        distance -= Input.GetAxis("Mouse ScrollWheel") * zoomSpeed;
        distance = Mathf.Clamp(distance, minDistance, maxDistance);

        Quaternion rotation = Quaternion.Euler(y, x, 0);
        Vector3 negDistance = new Vector3(0.0f, 0.0f, -distance);
        Vector3 position = rotation * negDistance + cameraPivot.position;

        cam.rotation = rotation;
        cam.position = position;
    }
}
