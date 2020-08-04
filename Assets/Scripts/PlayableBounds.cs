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

	private void OnTriggerExit(Collider other)
	{
		if (other.CompareTag("Player"))
			other.gameObject.GetComponent<PlayerController>().lives--;
	}
}