using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Rigidbody), typeof(Collider))]
public class BumperWall : MonoBehaviour
{
	[SerializeField] string collisionTag = "Player";

	Rigidbody rb = null;

	private void Awake()
	{
		rb = GetComponent<Rigidbody>();
		rb.isKinematic = true;
	}

	private void OnCollisionEnter(Collision collision)
	{
		//Debug.Log("Collision occurred: " + collision.gameObject.name);

		if (collision.gameObject.CompareTag(collisionTag))
		{
			//Collided with a rigidbody/collider of the specified mask
			rb.isKinematic = false;
		}
	}
}
