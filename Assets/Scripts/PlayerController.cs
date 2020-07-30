using UnityEngine;

public class PlayerController : MonoBehaviour
{
	[SerializeField] float m_speed = 5.0f;

	void Update()
	{
		transform.Translate(Input.GetAxis("Vertical") * transform.forward * m_speed * Time.deltaTime);
		transform.Translate(Input.GetAxis("Horizontal") * transform.right * m_speed * Time.deltaTime);
	}

	private void OnCollisionEnter(Collision collision)
	{
		float force = 500;

		if (collision.gameObject.tag == "Player")
		{
			Vector3 dir = collision.contacts[0].point - transform.position;
			dir = -dir.normalized;
			collision.gameObject.GetComponent<Rigidbody>().AddForce(dir * force);
		}
	}
}