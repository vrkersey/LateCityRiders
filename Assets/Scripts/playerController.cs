using System.Collections;
using System.Collections.Generic;
using UnityEngine.SceneManagement;
using UnityEngine;

public class playerController : MonoBehaviour {
	Transform player;
	Transform mainCamera;
	bool grounded = true;
	float control = 0;
    Rigidbody rb;
    bool inCar = false;

    public float drag = .05f;
	public float maxControl = 15;
	public float jumpMultiplier = 15;

	// Use this for initialization
	void Start () {
		player = transform;
		mainCamera = GameObject.FindGameObjectWithTag("MainCamera").transform;
        rb = player.GetComponent<Rigidbody>();
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
		
		if (!grounded)
		{
            // adds control the longer the player is in the air
			control = Mathf.Lerp(control, maxControl, .025f);
		}

        if (inCar){
            //do nothing
        }
		else if (Input.GetKey(KeyCode.A))
		{
			rb.AddForce(left * control);
		}
		else if (Input.GetKey(KeyCode.D))
		{
			rb.AddForce(-left * control);
		}
        else if (Input.GetKey(KeyCode.W))
        {
            rb.AddForce(forward * control/5);
        }
        else if (Input.GetKey(KeyCode.S))
        {
            rb.AddForce(-forward * control/5);
        }
        else
        {
            //reduce speed
            Vector3 horizontalVelocity = rb.velocity;

            float vertVelocity = horizontalVelocity.y;
            horizontalVelocity.y = 0;

            Vector3 newVelocity = Vector3.Lerp(horizontalVelocity, Vector3.zero, drag);
            newVelocity.y = vertVelocity;

            rb.velocity = newVelocity;
        }

		if (Input.GetKey(KeyCode.Space) && grounded)
		{

            rb.AddForce(Vector3.up * jumpMultiplier, ForceMode.Impulse);
			grounded = false;
            inCar = false;
        }
	}

	void OnCollisionEnter(Collision other)
	{
		grounded = true;
		control = 0;
        if (other.gameObject.CompareTag("Car")){
            GetComponent<MeshRenderer>().enabled = false;
            other.gameObject.GetComponent<Driving_Controls>().PlayerInCar = true;
            inCar = true;
            
        }
        if (other.gameObject.CompareTag("Kill Zone"))
        {
            SceneManager.LoadScene(SceneManager.GetActiveScene().name);
        }
    }

    void OnCollisionStay(Collision other)
    {
        grounded = true;
        control = 0;
        if (other.gameObject.CompareTag("Car"))
        {
            GetComponent<MeshRenderer>().enabled = false;
            other.gameObject.GetComponent<Driving_Controls>().PlayerInCar = true;
            inCar = true;

        }
        if (other.gameObject.CompareTag("Kill Zone"))
        {
            SceneManager.LoadScene(SceneManager.GetActiveScene().name);
        }
    }

    void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("Kill Zone"))
        {
            SceneManager.LoadScene(SceneManager.GetActiveScene().name);
        }
    }

    void OnCollisionExit(Collision other)
	{
		grounded = false;
        if (other.gameObject.CompareTag("Car"))
        {
            GetComponent<MeshRenderer>().enabled = true;
            other.gameObject.GetComponent<Driving_Controls>().PlayerInCar = false;
        }
    }
}
