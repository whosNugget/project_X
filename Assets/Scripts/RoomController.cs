using UnityEngine;
using Photon.Pun;
using System.Collections;
using TMPro;

public class RoomController : MonoBehaviourPunCallbacks
{
	// player instance prefab, must be located in the Resources folder 
	[SerializeField] GameObject playerPrefab;    // player spawn point  
	[SerializeField] Transform[] spawnPoints;
	[SerializeField] bool devMode = false;

	void Start()
	{
		// in case we started this scene with the wrong scene being active, simply load the menu scene only when the room controller is not in dev mode
		if (PhotonNetwork.CurrentRoom == null && !devMode)
		{
			Debug.Log("Is not in the room, returning back to Lobby");
			UnityEngine.SceneManagement.SceneManager.LoadScene("GameLobby");
			return;
		}
		// spawn a character for the local player. it gets synced by using PhotonNetwork.Instantiate       
		PlayerController spawnedPlayer = PhotonNetwork.Instantiate(playerPrefab.name, spawnPoints[Random.Range(0, spawnPoints.Length)].position, Quaternion.identity, 0).GetComponent<PlayerController>();
		spawnedPlayer.SetPlayerName(PhotonNetwork.NickName);
	}

	void OnGUI()
	{
		if (PhotonNetwork.CurrentRoom == null) return;        // leave this Room   
		if (GUI.Button(new Rect(5, 5, 125, 25), "Leave Room"))
		{
			PhotonNetwork.LeaveRoom();
		}        // show the Room name   
		GUI.Label(new Rect(135, 5, 200, 25), PhotonNetwork.CurrentRoom.Name);
		// show the list of the players connected to this Room   
		for (int i = 0; i < PhotonNetwork.PlayerList.Length; i++)
		{            // show if this player is a Master Client. There can only be one Master Client per Room so use this to define the authoritative logic etc.)      
			string isMasterClient = (PhotonNetwork.PlayerList[i].IsMasterClient ? ": MasterClient" : "");
			GUI.Label(new Rect(5, 35 + 30 * i, 200, 25), PhotonNetwork.PlayerList[i].NickName + isMasterClient);
		}
	}

	public override void OnLeftRoom()
	{        // left the Room, return back to the GameLobby  
		UnityEngine.SceneManagement.SceneManager.LoadScene("GameLobby");
	}

	public void RespawnPlayer(PlayerController toRespawn)
	{
		if (toRespawn.lives > 0)
		{
			toRespawn.lives--;
			Transform spawn = spawnPoints[Random.Range(0, spawnPoints.Length)];

			toRespawn.GetComponent<Rigidbody>().isKinematic = true;
			toRespawn.transform.position = (Vector3.up * 3) + spawn.position;
			toRespawn.transform.rotation = spawn.rotation;

			StartCoroutine(Countdown(toRespawn));
		}
		else
		{
			toRespawn.gameObject.SetActive(false);
		}
	}
	IEnumerator Countdown(PlayerController toRespawn)
	{
		yield return new WaitForSeconds(3f);
		toRespawn.GetComponent<Rigidbody>().isKinematic = false;
	}
}