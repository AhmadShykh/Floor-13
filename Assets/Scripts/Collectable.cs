using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class Collectable : MonoBehaviour,InteractableObject
{
    enum CollectableTypes
    {
        Paper,
        Trophy
    }


    [SerializeField] CollectableTypes type;
    [SerializeField] string collectableText;


    private GameObject _textShownCanvas;
    private GameObject _player;
	private void Start()
	{
        _textShownCanvas = GameObject.FindGameObjectWithTag("TextCanvas");
        _player = GameObject.FindGameObjectWithTag("Player");
	}

	public void InitiateInteractingSequence()
	{
        _textShownCanvas.SetActive(true); 
        _textShownCanvas.transform.Find("TextArea").GetComponent<TextMeshProUGUI>().text = collectableText;
        _player.SetActive(false);
	}

}
