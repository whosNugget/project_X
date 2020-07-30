using Photon.Pun;
using UnityEngine;

public class PlayerController : MonoBehaviourPunCallbacks
{
	[SerializeField] float speed = 5.0f;

	Rigidbody rb = null;

	private void Awake()
	{
		rb = GetComponent<Rigidbody>();
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
		}
	}
}