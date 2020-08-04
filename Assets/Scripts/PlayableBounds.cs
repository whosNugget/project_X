using System;
using UnityEngine;

public class PlayableBounds : MonoBehaviour
{
	RoomController activeController = null;
	GameUI localPlayerUI = null;

	private void Awake()
	{
		activeController = FindObjectOfType<RoomController>();
		localPlayerUI = FindObjectOfType<GameUI>();
	}

	private void OnTriggerEnter(Collider other)
	{
		Debug.Log("Entered bounds: " + other.gameObject.name);

		if (other.CompareTag("Player"))
			activeController.RespawnPlayer(other.gameObject.GetComponent<PlayerController>());
		else
			other.gameObject.SetActive(false);
	}
}