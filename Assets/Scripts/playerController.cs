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
    GameObject car;

    public float drag = .05f;
	public float maxControl = 15;
	public float jumpMultiplier = 15;
    public float exitSpeedVelocity = .5f;
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

		if (Input.GetKey(KeyCode.A) && !inCar)
		{
			rb.AddForce(left * control);
		}
        if (Input.GetKey(KeyCode.D) && !inCar)
		{
			rb.AddForce(-left * control);
		}
        if (Input.GetKey(KeyCode.W) && !inCar)
        {
            rb.AddForce(forward * control);
        }
        if (Input.GetKey(KeyCode.S) && !inCar)
        {
            rb.AddForce(-forward * control);
        }
        /*else
        {
            //reduce speed
            Vector3 horizontalVelocity = rb.velocity;

            float vertVelocity = horizontalVelocity.y;
            horizontalVelocity.y = 0;

            Vector3 newVelocity = Vector3.Lerp(horizontalVelocity, Vector3.zero, drag);
            newVelocity.y = vertVelocity;

            //rb.velocity = newVelocity;
        }*/

		if (Input.GetKey(KeyCode.Space) && grounded)
		{
            
            player.parent = null;
            if (rb == null)
            {
                rb = gameObject.AddComponent<Rigidbody>();
                rb.velocity = car.GetComponent<Rigidbody>().velocity * exitSpeedVelocity;
            }
            else
            {
                rb.AddForce(Vector3.up * jumpMultiplier, ForceMode.Impulse);
            }
			grounded = false;
        }
	}

	void OnCollisionEnter(Collision other)
	{
        if (other.gameObject.CompareTag("Kill Zone"))
        {
            SceneManager.LoadScene(SceneManager.GetActiveScene().name);
        }
    }

    void OnTriggerEnter(Collider other)
    {
        grounded = true;
        control = 0;
        if (other.gameObject.CompareTag("Car") && !inCar)
        {
            car = other.gameObject;
            GetComponent<MeshRenderer>().enabled = false;
            other.gameObject.GetComponent<Driving_Controls>().PlayerInCar = true;
            inCar = true;
            Destroy(player.GetComponent<Rigidbody>());
            player.parent = other.transform;
        }
        if (other.gameObject.CompareTag("Kill Zone"))
        {
            SceneManager.LoadScene(SceneManager.GetActiveScene().name);
        }
    }

    void OnTriggerExit(Collider other)
	{
		grounded = false;
        if (other.gameObject.CompareTag("Car"))
        {
            GetComponent<MeshRenderer>().enabled = true;
            other.gameObject.GetComponent<Driving_Controls>().PlayerInCar = false;

            inCar = false;
        }
    }
}
