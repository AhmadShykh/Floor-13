using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MainObjectCollection : MonoBehaviour, InteractableObject
{
	public void Interacting()
	{
		FindObjectOfType<GF_GameController>().CallElevatorSequence();
		Destroy(transform.parent.gameObject);
	}

	public void NotInteracting()
	{
		throw new System.NotImplementedException();
	}

	// Start is called before the first frame update
	void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
