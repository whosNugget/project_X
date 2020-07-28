using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using Photon.Realtime;
using System.Collections;
using UnityEditor;
using UnityEngine.SceneManagement;

public class GameLobby : MonoBehaviourPunCallbacks
{
	[SerializeField] string playerName = "Player1";
	[SerializeField] string appVersion = "0.0";
	[SerializeField] string roomName = "Room 1";
	[SerializeField] string sceneName = "SceneName";

	[SerializeField] ServerSelect serverSelectUI = null;

	List<RoomInfo> createdRooms = new List<RoomInfo>();
	Vector2 roomListScroll = Vector2.zero;
	bool joiningRoom = false;

	private void Start()
	{
		PhotonNetwork.AutomaticallySyncScene = true;
		if (!PhotonNetwork.IsConnected)
		{
			PhotonNetwork.PhotonServerSettings.AppSettings.AppVersion = appVersion;
			PhotonNetwork.ConnectUsingSettings();
		}
	}

	public void CreateRoom(string roomName)
	{
		joiningRoom = true;

		RoomOptions roomOptions = new RoomOptions()
		{
			IsOpen = true,
			IsVisible = true,
			MaxPlayers = 10,
		};

		PhotonNetwork.NickName = playerName;
		PhotonNetwork.JoinOrCreateRoom(roomName, roomOptions, TypedLobby.Default);
	}

	public void JoinRoom(string roomName)
	{
		if (!string.IsNullOrWhiteSpace(roomName))
		{
			joiningRoom = true;
			PhotonNetwork.NickName = playerName;
			PhotonNetwork.JoinRoom(roomName);
		}
	}

	public void UpdatePlayerName(string newName)
	{
		playerName = string.IsNullOrWhiteSpace(newName) ? "Player1" : newName;
	}

	public void RefreshList()
	{
		if (PhotonNetwork.IsConnected) PhotonNetwork.JoinLobby(TypedLobby.Default);
		else PhotonNetwork.ConnectUsingSettings();
	}

	public override void OnConnectedToMaster()
	{
		Debug.Log("Connected to master!");
		PhotonNetwork.JoinLobby();
	}

	public override void OnRoomListUpdate(List<RoomInfo> roomList)
	{
		createdRooms = roomList;
		StartCoroutine(serverSelectUI.UpdateRoomList(createdRooms));
	}

	public override void OnJoinedRoom()
	{
		Debug.Log("Created a room successfully");
		PhotonNetwork.NickName = playerName;
		PhotonNetwork.LoadLevel(sceneName);
	}

	public override void OnCreatedRoom()
	{
		Debug.Log("Created a room successfully");
		PhotonNetwork.NickName = playerName;
		PhotonNetwork.LoadLevel(sceneName);
	}

	public override void OnCreateRoomFailed(short returnCode, string message)
	{
		Debug.LogWarning($"Failed to create a room\nCode: {returnCode}; Message: {message}");
	}

	public override void OnJoinRoomFailed(short returnCode, string message)
	{
		Debug.LogWarning($"Failed to join a room\nCode: {returnCode}; Message: {message}");
	}

	public override void OnJoinRandomFailed(short returnCode, string message)
	{
		Debug.LogWarning($"Faild to join a random room\nCode: {returnCode}; Message: {message}");
	}

	public override void OnDisconnected(DisconnectCause cause)
	{
		Debug.LogWarning($"Disconnect event ocurred... Status: {cause}\nServer Address: <color=red>{PhotonNetwork.ServerAddress}</color>");
	}
}

//public class GameLobby : MonoBehaviourPunCallbacks
//{
//	[SerializeField] string playerName = "Player 1";
//	[SerializeField] string appVersion = "0.0";
//	[SerializeField] string roomName = "Room 1";
//	[SerializeField] string sceneName = "SceneName";
//	List<RoomInfo> createdRooms = new List<RoomInfo>();
//	Vector2 roomListScroll = Vector2.zero;
//	bool joiningRoom = false;

//	void Start()
//	{
//		PhotonNetwork.AutomaticallySyncScene = true;
//		if (!PhotonNetwork.IsConnected)
//		{
//			PhotonNetwork.PhotonServerSettings.AppSettings.AppVersion = appVersion;
//			PhotonNetwork.ConnectUsingSettings(); // settings in PhotonServerSettings in Unity
//		}
//	}

//	public override void OnDisconnected(DisconnectCause cause)
//	{
//		Debug.Log("OnFailedToConnectToPhoton. StatusCode: " + cause.ToString() + " ServerAddress: " + PhotonNetwork.ServerAddress);
//	}
//	// connected to master PUN server    
//	public override void OnConnectedToMaster()
//	{
//		Debug.Log("OnConnectedToMaster");
//		PhotonNetwork.JoinLobby(TypedLobby.Default);
//	}
//	public override void OnRoomListUpdate(List<RoomInfo> roomList)
//	{
//		Debug.Log("We have received the Room list");
//		createdRooms = roomList;
//	}
//	void OnGUI()
//	{
//		GUI.Window(0, new Rect(Screen.width / 2 - 450, Screen.height / 2 - 200, 900, 400), LobbyWindow, "Lobby");
//	}
//	void LobbyWindow(int index)
//	{
//		GUILayout.BeginHorizontal();
//		GUILayout.Label("Status: " + PhotonNetwork.NetworkClientState);
//		if (joiningRoom || !PhotonNetwork.IsConnected || PhotonNetwork.NetworkClientState != ClientState.JoinedLobby)
//		{ GUI.enabled = false; }
//		GUILayout.FlexibleSpace();
//		roomName = GUILayout.TextField(roomName, GUILayout.Width(250));
//		if (GUILayout.Button("Create Room", GUILayout.Width(125)))
//		{
//			if (roomName != "")
//			{
//				joiningRoom = true;
//				RoomOptions roomOptions = new RoomOptions(); roomOptions.IsOpen = true; roomOptions.IsVisible = true; roomOptions.MaxPlayers = 10; // set any number                
//				PhotonNetwork.JoinOrCreateRoom(roomName, roomOptions, TypedLobby.Default);
//			}
//		}
//		GUILayout.EndHorizontal();        //Scroll through available rooms       
//		roomListScroll = GUILayout.BeginScrollView(roomListScroll, true, true);
//		if (createdRooms.Count == 0)
//		{
//			GUILayout.Label("No Rooms were created yet...");
//		}
//		else
//		{
//			for (int i = 0; i < createdRooms.Count; i++)
//			{
//				GUILayout.BeginHorizontal("box"); 
//				GUILayout.Label(createdRooms[i].Name, GUILayout.Width(400)); 
//				GUILayout.Label(createdRooms[i].PlayerCount + "/" + createdRooms[i].MaxPlayers);
//				GUILayout.FlexibleSpace();
//				if (GUILayout.Button("Join Room"))
//				{
//					joiningRoom = true;
//					PhotonNetwork.NickName = playerName; // set player name
//					PhotonNetwork.JoinRoom(createdRooms[i].Name); // join the Room        
//				}
//				GUILayout.EndHorizontal();
//			}
//		}
//		GUILayout.EndScrollView(); // set player name and refresh room button        
//		GUILayout.BeginHorizontal();
//		GUILayout.Label("Player Name: ", GUILayout.Width(85)); // player name text field       
//		playerName = GUILayout.TextField(playerName, GUILayout.Width(250));
//		GUILayout.FlexibleSpace();
//		GUI.enabled = (PhotonNetwork.NetworkClientState == ClientState.JoinedLobby || PhotonNetwork.NetworkClientState == ClientState.Disconnected) && !joiningRoom; 
//		if (GUILayout.Button("Refresh", GUILayout.Width(100)))
//		{
//			if (PhotonNetwork.IsConnected)
//			{   // re-join Lobby to get the latest Room list 
//				PhotonNetwork.JoinLobby(TypedLobby.Default);
//			}
//			else
//			{   // not connected, estabilish a new connection     
//				PhotonNetwork.ConnectUsingSettings();
//			}
//		}
//		GUILayout.EndHorizontal(); 
//		if (joiningRoom)
//		{
//			GUI.enabled = true; GUI.Label(new Rect(900 / 2 - 50, 400 / 2 - 10, 100, 20), "Connecting...");
//		}
//	}

//	public override void OnCreateRoomFailed(short returnCode, string message)
//	{
//		Debug.Log("OnCreateRoomFailed got called. This can happen if the room exists (even if not visible). Try another room name.");
//		joiningRoom = false;
//	}
//	public override void OnJoinRoomFailed(short returnCode, string message)
//	{
//		Debug.Log("OnJoinRoomFailed got called. This can happen if the room is not existing or full or closed.");
//		joiningRoom = false;
//	}
//	public override void OnJoinRandomFailed(short returnCode, string message)
//	{
//		Debug.Log("OnJoinRandomFailed got called. This can happen if the room is not existing or full or closed.");
//		joiningRoom = false;
//	}
//	public override void OnCreatedRoom()
//	{
//		Debug.Log("OnCreatedRoom");        // set player name        
//		PhotonNetwork.NickName = playerName;        // load the scene (make sure the scene is added to build settings)        
//		PhotonNetwork.LoadLevel(sceneName);
//	}
//	public override void OnJoinedRoom()
//	{
//		Debug.Log("OnJoinedRoom");
//	}
//}