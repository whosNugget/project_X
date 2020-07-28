using Photon.Pun;
using Photon.Realtime;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class ServerSelect : MonoBehaviour
{
	[SerializeField] TMP_Text connectionStatus = null;
	[SerializeField] TMP_InputField roomName = null;
	[SerializeField] ScrollRect availableLobbies = null;
	[SerializeField] TMP_InputField playerNickname = null;
	[SerializeField] Button refreshLobbies = null;

	[Space]
	[SerializeField] RoomUI roomObjectUI = null;

	GameLobby lobbyManager = null;

	private void Start()
	{
		lobbyManager = FindObjectOfType<GameLobby>();
		//NOTE: When adding children to the available lobbies, just instantiate a new prefab and set the prefab's parent to availableLobbies.content
	}

	private void OnGUI()
	{
		connectionStatus.text = PhotonNetwork.NetworkClientState.ToString();
	}

	private void Update()
	{
		//Debug creating labels
		if (Input.GetKey(KeyCode.Space))
		{
			RoomUI next = Instantiate(roomObjectUI);
			byte tempMax = (byte)Random.Range(0, 30);
			next.SetLabels(Random.Range(-1000, 1000).ToString(), tempMax);
			next.gameObject.transform.parent = availableLobbies.content;
		}
	}

	public void CreateRoom()
	{
		if (!string.IsNullOrWhiteSpace(roomName.text))
			lobbyManager.CreateRoom(roomName.text);
	}

	public void UpdatePlayerName()
	{
		lobbyManager.UpdatePlayerName(playerNickname.text);
	}

	public IEnumerator UpdateRoomList(List<RoomInfo> newList)
	{
		foreach (RoomInfo ri in newList)
		{
			RoomUI newUI = Instantiate(roomObjectUI);
			newUI.SetLabels(ri.Name, ri.MaxPlayers, ri.PlayerCount);
			newUI.transform.parent = availableLobbies.content;

			yield return null;
		}
	}
}
