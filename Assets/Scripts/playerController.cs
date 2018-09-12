using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class playerController : MonoBehaviour {
	Transform player;
	Transform mainCamera;
	bool grounded = true;
	float control = 0;

	public float drag = .05f;
	public float maxControl = 15;
	public float jumpMultiplier = 15;

	// Use this for initialization
	void Start () {
		player = GameObject.FindGameObjectWithTag("Player").transform;
		mainCamera = GameObject.FindGameObjectWithTag("MainCamera").transform;
	}
	
	// Update is called once per frame
	void Update () {
		Keyboard_Input();
	}

	private void Keyboard_Input()
	{
		Vector3 forward = (player.position - mainCamera.position);
		forward.y = 0;
		forward = forward.normalized;
		Vector3 left = Vector3.Cross(forward, Vector3.up);
		Rigidbody rb = player.GetComponent<Rigidbody>();

		if (!grounded)
		{
			control = Mathf.Lerp(control, maxControl, .025f);
		}

		if (Input.GetKey(KeyCode.A) && Input.GetKey(KeyCode.D) || !Input.GetKey(KeyCode.A) && !Input.GetKey(KeyCode.D))
		{
			//reduce speed
			Vector3 horizontalVelocity = rb.velocity;
			
			float vertVelocity = horizontalVelocity.y;
			horizontalVelocity.y = 0;
			
			Vector3 newVelocity = Vector3.Lerp(horizontalVelocity, Vector3.zero, drag);
			//Debug.Log(newVelocity);
			newVelocity.y = vertVelocity;
			
			rb.velocity = newVelocity;
		}
		if (Input.GetKey(KeyCode.A))
		{
			rb.AddForce(left * control);
		}
		if (Input.GetKey(KeyCode.D))
		{
			rb.AddForce(-left * control);
		}

		if (Input.GetKey(KeyCode.Space) && grounded)
		{
			rb.AddForce(Vector3.up * jumpMultiplier, ForceMode.Impulse);
			grounded = false;
		}
	}

	void OnCollisionEnter(Collision other)
	{
		grounded = true;
		control = 0;
	}

	void OnCollisionExit(Collision other)
	{
		grounded = false;
	}
}
