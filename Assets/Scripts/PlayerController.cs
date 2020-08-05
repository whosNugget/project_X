using Photon.Pun;
using UnityEngine;
using TMPro;

public class PlayerController : MonoBehaviourPun, IPunObservable
{
	[SerializeField] float speed = 5.0f;
	[SerializeField] int totalLives = 3;
	[SerializeField] float bumpForce = 35f;
	[SerializeField, Range(0, 1)] float selfBumpFactor = .35f;

	[SerializeField] bool overrideControl;

	[HideInInspector] public int lives;

	GameUI ui = null;
	Rigidbody rb = null;
	TMP_Text playerName = null;

	private void Awake()
	{
		rb = GetComponent<Rigidbody>();
		lives = totalLives;
		playerName = GetComponentInChildren<TMP_Text>();

		if (photonView.IsMine)
			ui = FindObjectOfType<GameUI>();
	}

	void Update()
	{
		if (photonView.IsMine || overrideControl)
		{
			Vector3 delta = new Vector3
			{
				x = Input.GetAxis("Horizontal"),
				z = Input.GetAxis("Vertical"),
			}.normalized * speed;

			rb.drag = delta.sqrMagnitude > 0f ? 0f : 5f;
			rb.AddRelativeForce(delta);

			//if (ui.Lives != lives) ui.Lives = lives;
		}

		float vertical = rb.velocity.y;
		rb.velocity = Vector3.ClampMagnitude(rb.velocity, 20f);
		rb.velocity = new Vector3 { x = rb.velocity.x, y = vertical, z = rb.velocity.z };
	}

	private void OnCollisionEnter(Collision collision)
	{
		if (collision.gameObject.CompareTag("Player"))
		{
			Vector3 transformCenter = transform.position + Vector3.up;
			Vector3 collisionTransformCenter = collision.transform.position + Vector3.up;

			Vector3 otherBumpForce = (collisionTransformCenter - transformCenter).normalized * bumpForce;
			Vector3 selfBumpForce = -otherBumpForce * selfBumpFactor;

			Debug.DrawRay(transformCenter, selfBumpForce, Color.red);
			Debug.DrawRay(collisionTransformCenter, otherBumpForce, Color.green);

			rb.AddRelativeForce(selfBumpForce, ForceMode.Impulse);
			collision.gameObject.GetComponent<Rigidbody>().AddRelativeForce(otherBumpForce, ForceMode.Impulse);
		}
	}

	public void SetPlayerName(string playerName)
	{
		this.playerName.text = playerName;
		this.playerName.enabled = !photonView.IsMine;
	}

	public void OnPhotonSerializeView(PhotonStream stream, PhotonMessageInfo info)
	{
		if (stream.IsWriting)
		{
			stream.SendNext(playerName.text);
			stream.SendNext(transform.position);
			stream.SendNext(rb.velocity);
			stream.SendNext(rb.drag);
		}
		else
		{
			playerName.text = (string)stream.ReceiveNext();
			transform.position = (Vector3)stream.ReceiveNext();
			rb.velocity = (Vector3)stream.ReceiveNext();
			rb.drag = (float)stream.ReceiveNext();
		}
	}
}