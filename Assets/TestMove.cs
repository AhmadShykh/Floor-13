using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class TestMove : MonoBehaviour
{
    [SerializeField] NavMeshAgent agent;


	private void Start()
	{
	}


	private void Update()
	{
		if (Input.GetKeyDown(KeyCode.W))
		{
			Vector3 position = transform.position + (new Vector3(1,0,0));
			agent.SetDestination(position);
		}
		else if (Input.GetKeyDown(KeyCode.S))
		{
			Vector3 position = transform.position + (new Vector3(-1, 0, 0));
			agent.SetDestination(position);
		}
		else if (Input.GetKeyDown(KeyCode.A))
		{
			Vector3 position = transform.position + (new Vector3(0, 0, 1));
			agent.SetDestination(position);
		}
		else if (Input.GetKeyDown(KeyCode.D))
		{
			Vector3 position = transform.position + (new Vector3(0, 0, -1));
			agent.SetDestination(position);
		}
	}
}
