using UnityEngine;
using System.Collections;

public class LookAtBackwards : MonoBehaviour
{
	public Transform target2;

	void Update()
	{
		transform.LookAt(transform.position - target2.position); // tried earlier transform.position - stareat.position
	}
}