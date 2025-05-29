using System.Collections;
using System.Collections.Generic;
using System.Data.Common;
using System.Linq;
using Unity.VisualScripting;
using UnityEditor;
using UnityEditor.Rendering;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.Rendering;

//[ExecuteInEditMode]
public class EnemyController : MonoBehaviour
{

    [SerializeField] NavMeshAgent enemy;
    [SerializeField] int waitTime;
	[SerializeField] float range;
	[SerializeField] Animator animator;
	[SerializeField] float idleToWalkSpeedValue;
	[SerializeField] float crawlSpeed;
	[SerializeField] float fastCrawlSpeed;
	[SerializeField] float patrolDistance;
	[SerializeField] LayerMask obstructionMask;
	[SerializeField] float playerReachThreshold;
	[SerializeField] float enemyPlayerYValue;

	public float angle;
	public float chaseDistance;
	// Assing new hiding locations to the enemy from the game controller;


	private Rigidbody rb;
	private float speed;
	private Vector3 previousPosition;
	private List<Transform> hidingLocations = new List<Transform>();
	//private Vector3 hidingSpot;
	//private Transform player;

	private void Awake()
	{
	}


    void EnemyStateChanged(EnemyStates enemyStates)
    {
		Debug.Log(enemyStates);

		switch (enemyStates)
		{
			case EnemyStates.Waiting:
				StartCoroutine(WaitForEnemyToPatrol());

				break;
			case EnemyStates.Patroling:
				enemy.speed = crawlSpeed;

				break;
			case EnemyStates.Chasing:
				enemy.speed = crawlSpeed;

				break;
			case EnemyStates.Hiding:
				// getting the nearest hiding spot
				Vector3 nearstLocation;

				//Debug.Log(hidingLocations.Count);

				if (hidingLocations.Count > 0)
					nearstLocation = hidingLocations.OrderBy(loc => Vector3.Distance(loc.position, enemy.transform.position)).FirstOrDefault().position;
				else
					nearstLocation = Vector3.zero;

				enemy.speed = fastCrawlSpeed;
				enemy.SetDestination(nearstLocation);
				break;
		}

	}

	IEnumerator WaitForEnemyToPatrol()
	{
		yield return new WaitForSeconds(waitTime);
		if(EnemyManager.Instance.enemyCurrentState == EnemyStates.Waiting)
			EnemyManager.Instance.UpdateEenemyState(EnemyStates.Patroling);
	}

	// Start is called before the first frame update
	void Start()
    {
		// Subscribing here because we the enemy manager is assigned at awake
		EnemyManager.Instance.EnemyStateChange += EnemyStateChanged;


		EnemyManager.Instance.UpdateEenemyState(EnemyStates.Hiding);
		rb = GetComponent<Rigidbody>();
		previousPosition = transform.position;
		//GameObject obj = GameObject.FindGameObjectWithTag("Player");
		//if (obj != null)
		//	player = obj.transform;
		//else
		//	Debug.Log("Player Could not be found");
    }

    // Update is called once per frame
    void Update()
	{
		//Debug.Log(EnemyManager.Instance.enemyCurrentState);
		PatrolEnemy();
		CheckEnemyHiding();
		DetectPlayer();
		SetAnimation();



		//if (rb.velocity.magnitude > idleToWalkSpeedValue)
		//	animator.SetTrigger("Crawl",true);
		//else
		//	animator.SetBool("Run",true );
	}

	private void CheckEnemyHiding()
	{
		if (EnemyManager.Instance.enemyCurrentState == EnemyStates.Hiding && enemy.remainingDistance <= enemy.stoppingDistance)
		{
			EnemyManager.Instance.UpdateEenemyState(EnemyStates.Waiting);
		}
	}

	private void PatrolEnemy()
	{
		if (EnemyManager.Instance.enemyCurrentState == EnemyStates.Patroling)
		{
			//Debug.Log("Patrol Enemy");
			if (enemy.remainingDistance <= enemy.stoppingDistance) //done with path
			{
				Vector3 point;
				if (RandomPoint(transform.position, range, out point)) //pass in our centre point and radius of area
				{
					Debug.DrawRay(point, Vector3.up, Color.blue, 1.0f); //so you can see with gizmos
					enemy.SetDestination(point);
				}
			}
		}
	}

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

	//bool RandomPoint(Vector3 center, float range, out Vector3 result)
	//{

	//	Vector3 randomPoint = center + Random.insideUnitSphere * range; //random point in a sphere 
	//	NavMeshHit hit;
	//	if (NavMesh.SamplePosition(randomPoint, out hit, 1.0f, NavMesh.AllAreas)) //documentation: https://docs.unity3d.com/ScriptReference/AI.NavMesh.SamplePosition.html
	//	{
	//		//the 1.0f is the max distance from the random point to a point on the navmesh, might want to increase if range is big
	//		//or add a for loop like in the documentation
	//		result = hit.position;
	//		return true;
	//	}

	//	result = Vector3.zero;
	//	return false;
	//}
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

	private void DetectPlayer()
	{
		
		if (PlayerManager.Instance.currentPlayerObject == null)

			return;


		Transform player = PlayerManager.Instance.currentPlayerObject.transform;
		//Debug.DrawRay();

		Ray ray = new Ray(transform.position,transform.forward);
		Debug.DrawRay(ray.origin,ray.direction*chaseDistance);

		Vector3 playerPos = player.position, enemyPos = transform.position;
		float distanceToPlayer = Vector3.Distance(enemyPos, playerPos);
		playerPos.y = 0;
		enemyPos.y = 0;



		//Debug.Log("Plyer Detected");
		// Check if the player is within chase distance and in line of sight
		//Debug.Log("Player Found Distance: " + distanceToPlayer);	
		if (EnemyManager.Instance.enemyCurrentState == EnemyStates.Patroling && distanceToPlayer <= chaseDistance)
		{
			//Debug.Log("Player Within Reach");

			Vector3 direction = (playerPos - enemyPos);

			Vector3 enemyForward = transform.forward;
			enemyForward.y = 0;

			//	Debug.Log(Vector3.Angle(direction, enemyForward));

			if (Vector3.Angle(direction, enemyForward) < angle/2)
			{
				direction.y = player.position.y - transform.position.y;
				//Debug.Log(direction);
				if (!Physics.Raycast(transform.position, direction, chaseDistance, obstructionMask	))
				{
					Debug.DrawRay(transform.position, direction,Color.green);
					Debug.Log("Player Within Sight");
					EnemyManager.Instance.UpdateEenemyState(EnemyStates.Chasing);

				}
			}

			//RaycastHit hit;
			//Vector3 directionToPlayer = (player.position - transform.position).normalized;
			//if (Physics.Raycast(transform.position, directionToPlayer, out hit, chaseDistance))
			//{
			//	if (hit.transform == player)
			//	{
			//		// Player is in line of sight, start chasing
			//		EnemyManager.Instance.UpdateEenemyState(EnemyStates.Chasing);
			//		enemy.SetDestination(player.position);
			//	}
			//}
		}
		else if (EnemyManager.Instance.enemyCurrentState == EnemyStates.Chasing )
		{
			// Player is far enough away, start patrolling
			if (distanceToPlayer > patrolDistance )
				EnemyManager.Instance.UpdateEenemyState(EnemyStates.Patroling);
			else if (Vector3.Distance(playerPos,enemyPos) <= playerReachThreshold)
			{
				GameManager.Instance.GameLoose();
				this.enabled = false;
			}
			else
				enemy.SetDestination(PlayerManager.Instance.currentPlayerObject.transform.position);
		}
	}


	private void OnDestroy()
	{
			EnemyManager.Instance.EnemyStateChange -= EnemyStateChanged;
	}

	internal void SetHidingLocations(List<Transform> levelHidingLocations)
	{
		hidingLocations = levelHidingLocations;
	}
}
