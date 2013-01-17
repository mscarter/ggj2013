using UnityEngine;
using System.Collections;

public class ExampleScript : MonoBehaviour {
	
	Rigidbody body;
	public float bounceInterval = 5f;
	float nextBounce;
	
	// Use this for initialization
	void Start () {
		body = rigidbody;
		nextBounce = bounceInterval;
	}
	
	// Update is called once per frame
	void Update () {
		if (Time.time > nextBounce)
		{
			nextBounce += bounceInterval;
			body.AddForce(Vector3.up*500);
		}
	}
}
