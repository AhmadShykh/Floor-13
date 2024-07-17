using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CanvasLogic : MonoBehaviour
{
    [SerializeField] GameObject textCanvas;

    private GameObject _player;

	private void Start()
	{
		_player = GameObject.FindGameObjectWithTag("Player");
	}

	public void ResetTextCanvas()
	{
		_player.SetActive(true);
		textCanvas.SetActive(false);
	}
}
