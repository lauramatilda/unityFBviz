using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class soundeffect : MonoBehaviour {

	// Use this for initialization
	void Start () {
		
	}

	void Update()
	{
		if (Input.GetMouseButtonDown(0) == true)
		{
			GetComponent<AudioSource>().Play();
		}
		if (Input.GetMouseButtonUp(0) == true)
		{
			GetComponent<AudioSource>().Stop();
		}
	}
}
