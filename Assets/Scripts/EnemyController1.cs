using System.Collections;
using UnityEngine;
using UnityEngine.AI;

public class EnemyController1 : MonoBehaviour
{

    [SerializeField] NavMeshAgent enemy;
	[SerializeField] Animator animator;
	

	[Header("Enemy Properties")]
	public float angle;
	public float chaseDistance;
	public float audioChaseDistance;
	[SerializeField, Tooltip("Variable that is divided by chase and audio distance when Hunting")]  float decreaseDistanceBy;
	public float patrolRange;
	[SerializeField, Tooltip("Distance After which the enemy catches the player")] float playerReachThreshold;
	[SerializeField] float idleToWalkSpeedValue;
	[SerializeField] float crawlSpeed;
	[SerializeField] float fastCrawlSpeed;
	[SerializeField] LayerMask obstructionMask;
	
	[Header("State Change Timer Settings")] 
	[SerializeField] float searchToIdleTime;
	[SerializeField] float idleToPatrolTime;
	[Header("Customer Read Only")]
	[ReadOnly] public bool playerInView = false;
	
	//Private Variables 
	
	private Vector3 investigatePosition;
	private float speed;
	private Vector3 previousPosition;
	
	// Timer based states variable to check if they are already called or not 
	private Coroutine delayedIdleCoroutine = null;
	private Coroutine delayedPatrolCoroutine = null;

	// For calling locker killing animation if hid when within sight
	private bool killedFromHidingSpot = false;

	// Player Object
	private GameObject playerObject;
	
	

	private void Awake()
	{
		playerObject = GameObject.FindGameObjectWithTag("Player");
	}


    void EnemyStateChanged(EnemyStates1 enemyStates)
    {
		Debug.Log(enemyStates);

		switch (enemyStates)
		{
			case EnemyStates1.Inactive:
				// The player does nothign here. 
				break;
			case EnemyStates1.Investigate:
				enemy.speed = fastCrawlSpeed;
				break;
			case EnemyStates1.Hunt:
				enemy.speed = fastCrawlSpeed;
				break;
			// Switch to Search yourself only when the enemy was hunting and make sure to keep your playerhiding bool to true 
			case EnemyStates1.Search:
				enemy.speed = crawlSpeed;
				break;
			case EnemyStates1.Idle:
				enemy.speed = crawlSpeed;
				break;
			case EnemyStates1.Patrol:
				enemy.speed = crawlSpeed;
				break;
			// Switch to waiting by using the function given below and change based on lose and win as you want
			case EnemyStates1.Waiting:
				enemy.speed = crawlSpeed;
				break;
		}

	}

    

	// Start is called before the first frame update
	void Start()
    {
		EnemyManager1.Instance.EnemyStateChange += EnemyStateChanged;
		EnemyManager1.Instance.enemyController1 = this;

		EnemyManager1.Instance.enemyCurrentState = EnemyStates1.Inactive;
		EnemyManager1.Instance.UpdateEenemyState(EnemyStates1.Inactive);
		previousPosition = transform.position;
		
    }

    // Update is called once per frame
    void Update()
	{
		EnemyStateMovement();
		SetAnimation();
	}
    
	/// <summary>
	/// Handles enemy behavior based on its current state, including movement, detection,
	/// and transitions between states such as Investigate, Hunt, Search, Idle, Patrol, Waiting, and Kill.
	/// 
	/// - Investigate: Checks if the player is in sight and transitions to Idle if the destination is reached.
	/// - Hunt: Chases the player; if too far, switches to Search. If within attack range, switches to Kill.
	/// - Search: Looks for the player; if the destination is reached without spotting the player, returns to Idle.
	/// - Idle: Checks if the player is in sight and transitions to Patrol after a delay.
	/// - Patrol: Randomly moves to a nearby point, while checking for player visibility.
	/// - Waiting: Placeholder for actions when enemy reaches a waiting point.
	/// - Kill: Triggers the kill animation when the enemy catches the player.
	/// 
	/// Uses raycasting and distance checks to determine player visibility and proximity.
	/// </summary>
	private void EnemyStateMovement()
	{
		// Get the player position here 
		Transform player = playerObject.transform;

		Ray ray = new Ray(transform.position,transform.forward);
		Debug.DrawRay(ray.origin,ray.direction*chaseDistance);

		Vector3 playerPos = player.position, enemyPos = transform.position;
		float distanceToPlayer = Vector3.Distance(enemyPos, playerPos);
		
		
		if (EnemyManager1.Instance.enemyCurrentState == EnemyStates1.Investigate)
		{
			CheckToChasePlayer(playerPos, enemyPos, distanceToPlayer);
			if (!enemy.pathPending && enemy.remainingDistance <= enemy.stoppingDistance)
			{
				EnemyManager1.Instance.UpdateEenemyState(EnemyStates1.Idle);
			}
			
		}
		else if (EnemyManager1.Instance.enemyCurrentState == EnemyStates1.Hunt)
		{
			playerInView = false;
			// Was using the less chase distance here but was causing issues
			if (distanceToPlayer > chaseDistance)
			{
				// Player Got out of site and went for searching 
				EnemyManager1.Instance.UpdateEenemyState(EnemyStates1.Search);
			}
			else if (distanceToPlayer <= playerReachThreshold)
			{
				// Here the enemy reaches the player and looses
				EnemyManager1.Instance.UpdateEenemyState(EnemyStates1.Kill);
			}
			else
			{
				playerInView = true;
				enemy.SetDestination(playerObject.transform.position);
			}
		}
		else if (EnemyManager1.Instance.enemyCurrentState == EnemyStates1.Search)
		{
			CheckToChasePlayer(playerPos, enemyPos, distanceToPlayer);
			if (!enemy.pathPending && enemy.remainingDistance <= enemy.stoppingDistance)
			{
				if (playerInView)
				{
					playerInView = false;
					EnemyManager1.Instance.UpdateEenemyState(EnemyStates1.Kill);
				}
				else
				{
					// Call a searching animation here if you want 
					if(delayedIdleCoroutine == null)
						delayedIdleCoroutine = StartCoroutine(DelayedReturnToIdle());
				}
			}
		}
		else if (EnemyManager1.Instance.enemyCurrentState == EnemyStates1.Idle)
		{
			CheckToChasePlayer(playerPos, enemyPos, distanceToPlayer);
			if(delayedPatrolCoroutine == null)
				StartCoroutine(DelayedReturnToPatrol());
		}
		else if (EnemyManager1.Instance.enemyCurrentState == EnemyStates1.Patrol)
		{
			CheckToChasePlayer(playerPos, enemyPos, distanceToPlayer);
			if (enemy.remainingDistance <= enemy.stoppingDistance) //done with path
			{
				Vector3 point;
				if (RandomPoint(transform.position, patrolRange, out point)) //pass in our centre point and radius of area
				{
					Debug.DrawRay(point, Vector3.up, Color.blue, 1.0f); //so you can see with gizmos
					enemy.SetDestination(point);
				}
			}
		}
		else if (EnemyManager1.Instance.enemyCurrentState == EnemyStates1.Waiting)
		{
			// Do something if you want the enemy reaching their waiting waypoint 
			if (!enemy.pathPending && enemy.remainingDistance < enemy.stoppingDistance)
			{
				// Do here
			}
		}
		else if (EnemyManager1.Instance.enemyCurrentState == EnemyStates1.Kill)
		{
			// Trigger Enemy Kill animation here in Real Time
			animator.SetTrigger("Kill_Trigger");
		}
	}
	
	private IEnumerator DelayedReturnToIdle()
	{
		yield return new WaitForSeconds(searchToIdleTime);

		// Double check the state before switching to Idle
		if (EnemyManager1.Instance.enemyCurrentState == EnemyStates1.Search && !playerInView)
		{
			EnemyManager1.Instance.UpdateEenemyState(EnemyStates1.Idle);
		}

		delayedIdleCoroutine = null;
	}
	
	private IEnumerator DelayedReturnToPatrol()
	{
		yield return new WaitForSeconds(idleToPatrolTime);

		// Double check the state before switching to Idle
		if (EnemyManager1.Instance.enemyCurrentState == EnemyStates1.Idle)
		{
			EnemyManager1.Instance.UpdateEenemyState(EnemyStates1.Patrol);
		}

		delayedPatrolCoroutine = null;
	}


	/// <summary>
	/// Checks if the player is within the enemy's chase range and field of view.
	/// If the player is not hidden and within the specified angle and distance, 
	/// and there are no obstructions between the enemy and the player,
	/// the enemy state is updated to begin hunting the player.
	/// </summary>
	/// <param name="playerPos">The current position of the player.</param>
	/// <param name="enemyPos">The current position of the enemy.</param>
	/// <param name="distanceToPlayer">The distance between the player and the enemy.</param>
	private void CheckToChasePlayer(Vector3 playerPos, Vector3 enemyPos, float distanceToPlayer)
	{
		// Take Player Position 

		if (!Camera.main.transform.GetComponent<PlayerAndCameraController>().playerHid)
		{
			
			Vector3 direction = (playerPos - enemyPos);

			Vector3 enemyForward = transform.forward;
			enemyForward.y = 0;
			
			if (distanceToPlayer <= chaseDistance && Vector3.Angle(direction, enemyForward) < angle/2)
			{
				if (!Physics.Raycast(transform.position, direction, chaseDistance, obstructionMask	))
				{
					Debug.DrawRay(transform.position, direction,Color.green);
					Debug.Log("Player Within Sight");
					EnemyManager1.Instance.UpdateEenemyState(EnemyStates1.Hunt);

				}
			}
		}
	}

	
	/// <summary>
	/// Updates the animation blend value based on the object's movement speed.
	/// Calculates the speed by measuring the distance moved since the last frame,
	/// converts it to a suitable range, and sets it to the Animator's "Blend" parameter.
	/// </summary>

	private void SetAnimation()
	{
		// Calculate the distance the object has moved since the last frame
		Vector3 displacement = transform.position - previousPosition;

		// Calculate the speed (distance per time, where time is the duration of the frame)
		speed = displacement.magnitude / Time.deltaTime;

		speed = ConvertRange(speed);

		// Update the previous position to the current position
		previousPosition = transform.position;

		// Debug log the speed to the console
		//Debug.Log("Speed: " + speed);

		animator.SetFloat("Blend", speed);

	}

	// Function to convert a value from one range to another
	public float ConvertRange(float oldValue)
	{
		float oldRange = fastCrawlSpeed - 0;
		float newRange = 1 - 0;
		float newValue = 0 + ((oldValue - 0) * newRange / oldRange);
		return newValue;
	}
	
	bool RandomPoint(Vector3 center, float range, out Vector3 result)
	{
		for (int i = 0; i < 10; i++)
		{
			Vector3 randomPoint = center + Random.insideUnitSphere * range;
			NavMeshHit hit;
			if (NavMesh.SamplePosition(randomPoint, out hit, 1.0f, NavMesh.AllAreas))
			{
				result = hit.position;
				return true;
			}
		}
		result = Vector3.zero;
		return false;
	}
	

	private void OnDestroy()
	{
			EnemyManager1.Instance.EnemyStateChange -= EnemyStateChanged;
	}
	
	
	/// <summary>
	/// Call this if an object made a sound using EnemyManager1.Instance.EnemyController1.CheckSoundHeardPosition(..., ...)
	/// Do not change the state yourself in this case
	/// </summary>
	/// <param name="position">Position of the sound</param>
	/// <param name="checkRange">Allows the position to be checked even if not with enemy audio range</param>
	public void CheckSoundHeardPosition(Vector3 position, bool checkRange)
	{
		if (checkRange)
		{
			Vector2 enemyPos = new Vector2(transform.position.x, transform.position.z);
			bool checkWithinCircle = CheckWithInCircle(enemyPos,new Vector2(position.x,position.z));
			if (checkWithinCircle)
			{
				investigatePosition = new Vector3(position.x,0,position.z);
				enemy.SetDestination(investigatePosition);
				EnemyManager1.Instance.UpdateEenemyState(EnemyStates1.Investigate);
			}
		}
		else
		{
			animator.SetTrigger("Activate_Enemy");
			investigatePosition = new Vector3(position.x,0,position.z);
			enemy.SetDestination(investigatePosition);
			float num = enemy.remainingDistance;
			EnemyManager1.Instance.UpdateEenemyState(EnemyStates1.Investigate);
		}
	}

	/// <summary>
	/// Used when moving the enemy to a waiting position. So just use this when the enemy is waiting for the player to complete his mission
	/// </summary>
	/// <param name="destination">The waiting position where the enemy will stand</param>
	public void SetNavMeshAgentDestination(Vector3 destination)
	{
		enemy.SetDestination(destination);
		EnemyManager1.Instance.UpdateEenemyState(EnemyStates1.Waiting);
	}

	private bool CheckWithInCircle(Vector2 enemyPos, Vector2 position)
	{
		if (Mathf.Pow(enemyPos.x - position.x, 2) + Mathf.Pow(enemyPos.y - position.y, 2) <
		    Mathf.Pow(audioChaseDistance, 2))
			return true;
		else
			return false;
	}
	
	
	
	
}
