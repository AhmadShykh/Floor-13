using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class CollectableBase : MonoBehaviour
{
	protected CollectableTypes[] destroyable = { CollectableTypes.Key};

	public string collectableName = "";
	public CollectableTypes type;
	public string collectableText;

}
