using Photon.Pun;
using Photon.Realtime;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class RoomUI : MonoBehaviour
{
	[SerializeField] TMP_Text roomNameLabel = null;
	[SerializeField] TMP_Text playerCountLabel = null;

	[Space]
	[SerializeField] string roomName = "";
	[SerializeField] int currentPlayers = 0;
	[SerializeField] byte maxPlayers = 0;

	public RoomOptions RoomSettings { get; } = new RoomOptions();

	//TODO Error checking
	public string RoomName { get { return roomName; } set { roomName = value; UpdateLabels(); } }
	public int CurrentPlayers { get { return currentPlayers; }  set { currentPlayers = value; UpdateLabels(); } }
	public byte MaxPlayers { get { return maxPlayers; } set { maxPlayers = value; UpdateLabels(); } }

	private void Awake()
	{
		GetComponentInChildren<Button>().onClick.AddListener(JoinRoom);
	}

	public void SetLabels(string roomName, byte maxPlayers, int currentPlayers = 0)
	{
		RoomSettings.IsOpen = true;
		RoomSettings.IsVisible = true;
		RoomSettings.MaxPlayers = maxPlayers;

		this.roomName = roomName;
		this.currentPlayers = currentPlayers;
		this.maxPlayers = maxPlayers;
		UpdateLabels();
	}

	private void OnValidate() => UpdateLabels();
	private void UpdateLabels()
	{
		RoomSettings.MaxPlayers = maxPlayers;

		roomNameLabel.text = roomName;
		playerCountLabel.text = $"{currentPlayers}/{maxPlayers}";
	}

	public void JoinRoom()
	{
		PhotonNetwork.JoinRoom(roomName);
	}
}
