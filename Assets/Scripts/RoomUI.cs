using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class RoomUI : MonoBehaviour
{
	[SerializeField] TMP_Text roomNameLabel = null;
	[SerializeField] TMP_Text playerCountLabel = null;

	[Space]
	[SerializeField] string roomName = "";
	[SerializeField] int currentPlayers = 0;
	[SerializeField] int maxPlayers = 0;

	//TODO Error checking
	public string RoomName { get { return roomName; } set { roomName = value; UpdateLabels(); } }
	public int CurrentPlayers { get { return currentPlayers; }  set { currentPlayers = value; UpdateLabels(); } }
	public int MaxPlayers { get { return maxPlayers; } set { maxPlayers = value; UpdateLabels(); } }

	public void SetLabels(string roomName, int maxPlayers, int currentPlayers = 0)
	{
		this.roomName = roomName;
		this.currentPlayers = currentPlayers;
		this.maxPlayers = maxPlayers;
		UpdateLabels();
	}

	private void OnValidate() => UpdateLabels();
	private void UpdateLabels()
	{
		roomNameLabel.text = roomName;
		playerCountLabel.text = $"{currentPlayers}/{maxPlayers}";
	}
}
