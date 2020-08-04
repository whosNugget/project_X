using Photon.Pun;
using UnityEngine;

public class PlayerController : MonoBehaviourPunCallbacks
{
	[SerializeField] float speed = 5.0f;
	[SerializeField] int totalLives = 3;

	[HideInInspector] public int lives;

	GameUI ui = null;
	Rigidbody rb = null;

	private void Awake()
	{
		rb = GetComponent<Rigidbody>();
		lives = totalLives;

		if (photonView.IsMine)
			ui = FindObjectOfType<GameUI>();
	}

	void Update()
	{
		if (photonView.IsMine)
		{
			Vector3 delta = new Vector3
			{
				x = Input.GetAxis("Horizontal"),
				z = Input.GetAxis("Vertical"),
			} * speed;

			rb.AddRelativeForce(delta);

			//if (ui.Lives != lives) ui.Lives = lives;
		}
	}
}