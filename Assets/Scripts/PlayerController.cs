using Photon.Pun;
using UnityEngine;

public class PlayerController : MonoBehaviourPunCallbacks
{
	[SerializeField] float speed = 5.0f;
	[SerializeField] int totalLives = 3;
	[SerializeField] float bumpForce = 35f;
	[SerializeField, Range(0, 1)] float selfBumpFactor = .35f;

	[SerializeField] bool overrideControl;

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
		if (photonView.IsMine || overrideControl)
		{
			Vector3 delta = new Vector3
			{
				x = Input.GetAxis("Horizontal"),
				z = Input.GetAxis("Vertical"),
			}.normalized * speed;

			rb.AddRelativeForce(delta);

			//if (ui.Lives != lives) ui.Lives = lives;
		}
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
}