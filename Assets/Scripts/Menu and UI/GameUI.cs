using Photon.Pun;
using TMPro;
using UnityEngine;

public class GameUI : MonoBehaviourPun, IPunObservable
{
	[SerializeField] TMP_Text livesText = null;
	[SerializeField] TMP_Text timeRemainingText = null;

	[SerializeField] int minutes = 3;
	[SerializeField] float seconds = 0f;

	public int Lives { get => Lives; set { Lives = value; UpdateUIElements(); } }
	public int MinutesRemaining { get => minutes; set { minutes = value; UpdateUIElements(); } }
	public float SecondsRemaining { get => seconds; set { seconds = value; UpdateUIElements(); } }

	private void Update()
	{
		if(photonView.IsMine)
		{
			SecondsRemaining -= Time.deltaTime;
			if (SecondsRemaining < 0f)
			{
				MinutesRemaining--;
				SecondsRemaining = 60f;
			}
		}
	}

	public void UpdateUIElements()
	{
		timeRemainingText.text = $"{MinutesRemaining}:{SecondsRemaining:f0}";
		livesText.text = $"Lives: {Lives}";
	}

	public void OnPhotonSerializeView(PhotonStream stream, PhotonMessageInfo info)
	{
		if (stream.IsWriting)
		{
			stream.SendNext(MinutesRemaining);
			stream.SendNext(SecondsRemaining);
		}
		else
		{
			MinutesRemaining = (int)stream.ReceiveNext();
			SecondsRemaining = (int)stream.ReceiveNext();
		}
	}
}
