using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Rigidbody), typeof(Collider))]
public class DevMoveController : MonoBehaviour
{
	public Transform cameraTarget = null;
	public float speed = 12.5f;

	Rigidbody rb = null;

	private void Awake()
	{
		rb = GetComponent<Rigidbody>();
	}

	private void Update()
	{
		Camera.main.transform.position = cameraTarget.position;

		Vector3 delta = new Vector3
		{
			x = Input.GetAxis("Horizontal"),
			z = Input.GetAxis("Vertical"),
		};

		rb.AddRelativeForce(delta * speed);
	}
}