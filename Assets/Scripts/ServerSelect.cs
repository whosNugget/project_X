using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;
using Photon.Pun;

public class ServerSelect : MonoBehaviour
{
	[SerializeField] TMP_Text connectionStatus = null;
	[SerializeField] TMP_InputField rooomName = null;
	[SerializeField] ScrollRect availableLobbies = null;
	[SerializeField] TMP_InputField playerNickname = null;
	[SerializeField] Button refreshLobbies = null;

	[Space]
	[SerializeField] RoomUI roobObjectUI = null;

	private void Start()
	{
		//NOTE: When adding children to the available lobbies, just instantiate a new prefab and set the prefab's parent to availableLobbies.content
	}

	private void OnGUI()
	{
		connectionStatus.text = PhotonNetwork.NetworkClientState.ToString();
	}

	private void Update()
	{
		if(Input.GetKey(KeyCode.Space))
		{
			RoomUI next = Instantiate(roobObjectUI);
			int tempMax = Random.Range(0, 30);
			next.SetLabels(Random.Range(-1000, 1000).ToString(), tempMax, Random.Range(0, tempMax + 1));
			next.gameObject.transform.parent = availableLobbies.content;
		}
	}
}
