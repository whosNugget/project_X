using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraController : MonoBehaviour
{
	Dictionary<string, Transform> observables = new Dictionary<string, Transform>();
	
	Vector3 observationCenter = Vector3.zero;
	float observationSquareMagnitude = 0f;

	public bool Observe(string key, Transform observe)
	{
		if (observables.ContainsKey(key)) return false;

		observables.Add(key, observe);
		return true;
	}

	public bool Ignore(string key)
	{
		if (observables.ContainsKey(key))
		{
			observables.Remove(key);
			return true;
		}

		return false;
	}

	private void Update()
	{
		foreach (var t in observables) observationCenter += t.Value.position;
		observationCenter /= observables.Count;
		observationSquareMagnitude = observationCenter.sqrMagnitude;


	}
}
