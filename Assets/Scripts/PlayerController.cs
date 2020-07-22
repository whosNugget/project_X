using UnityEngine;

public class PlayerController : MonoBehaviour
{
	[SerializeField] float m_speed = 5.0f;

	void Update()
	{
		transform.Translate(Input.GetAxis("Vertical") * transform.forward * m_speed * Time.deltaTime);
		transform.Translate(Input.GetAxis("Horizontal") * transform.right * m_speed * Time.deltaTime);
	}
}