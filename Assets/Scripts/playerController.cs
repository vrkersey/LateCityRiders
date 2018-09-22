using System.Collections;
using System.Collections.Generic;
using UnityEngine.SceneManagement;
using UnityEngine;
using UnityEngine.AI;

public class playerController : MonoBehaviour
{
    Transform player;
    Transform mainCamera;
    bool grounded = true;
    float control = 0;
    Rigidbody rb;
    bool inCar = false;
    GameObject car;

    float nextCarDelay = 1f;
    float nextCarTimer;

    //public float drag = .05f; - to be deleted
    //public float maxControl = 15;
    //public float jumpMultiplier = 15;
    //public float exitSpeedVelocity = .5f;
    public IPlayer thePlayer;

    // Use this for initialization
    void Start()
    {
        player = transform;
        mainCamera = GameObject.FindGameObjectWithTag("MainCamera").transform;
        rb = player.GetComponent<Rigidbody>();
        thePlayer = GetComponent<IPlayer>();
    }

    // Update is called once per frame
    void Update()
    {
        Keyboard_Input();
        nextCarTimer -= Time.deltaTime;
    }

    private void Keyboard_Input()
    {
        if (Input.GetKey(KeyCode.W) && !inCar)
        {
            thePlayer.moveForward(rb, 1);
        }
        else if (Input.GetKey(KeyCode.S) && !inCar)
        {
            thePlayer.moveForward(rb, -1);
        }
        else
        {
            thePlayer.moveForward(rb, 0);
        }

        if (Input.GetKey(KeyCode.D) && !inCar)
        {
            thePlayer.moveRight(rb, 1);
        }
        else if (Input.GetKey(KeyCode.A) && !inCar)
        {
            thePlayer.moveRight(rb, -1);
        }
        else
        {
            thePlayer.moveRight(rb, 0);
        }

        if (Input.GetKeyDown(KeyCode.Space) && grounded)
        {
            player.parent = null;
            if (rb == null)
            {
                rb = gameObject.AddComponent<Rigidbody>();
                //rb.velocity = car.GetComponent<Rigidbody>().velocity * exitSpeedVelocity;
            }
            //else
            //{
            //rb.AddForce(Vector3.up * jumpMultiplier, ForceMode.Impulse);
            //}

            thePlayer.exitVehicle(rb, car);
            grounded = false;

            //if (other.gameObject.CompareTag("Car"))
            //{
            GetComponent<MeshRenderer>().enabled = true;
            car.gameObject.GetComponent<Driving_Controls>().PlayerInCar = false;
            inCar = false;
            nextCarTimer = nextCarDelay;
            //}
        }
        else if (Input.GetKeyDown(KeyCode.Space) && !grounded)
        {
            thePlayer.useSpecial(rb);
            nextCarTimer = 0;
        }
    }

    void OnCollisionEnter(Collision other)
    {
        if (other.gameObject.CompareTag("Kill Zone"))
        {
            Debug.Log("kill");
            SceneManager.LoadScene(SceneManager.GetActiveScene().name);
        }

        //NEW: Ends the level with a success. For prototype it simply restarts stage.
        if (other.gameObject.CompareTag("Goal"))
        {
            Debug.Log("goal");
            SceneManager.LoadScene(SceneManager.GetActiveScene().name);
        }
    }

    void OnTriggerEnter(Collider other)
    {
        

        if (other.gameObject.CompareTag("Car") && !inCar && nextCarTimer <= 0f)
        {
            grounded = true;
            control = 0;
            car = other.gameObject;
            GetComponent<MeshRenderer>().enabled = false;
            other.gameObject.GetComponent<Driving_Controls>().PlayerInCar = true;

            //NEW: Adds a slight increase of initial speed when entering vehicles.
            //other.gameObject.GetComponent<Driving_Controls>().speed = 15f;
            inCar = true;
            Destroy(player.GetComponent<Rigidbody>());
            player.position = car.transform.position + (car.transform.up * 2) + (car.transform.forward * -1);
            player.parent = other.transform;
            car.GetComponent<Driving_Controls>().speed = thePlayer.GetHorVelocityCheck().magnitude / 2;

            car.GetComponent<NavMeshAgent>().enabled = false;
        }
        //if (other.gameObject.CompareTag("Kill Zone"))
        //{
        //    Debug.Log("kill");
        //    //SceneManager.LoadScene(SceneManager.GetActiveScene().name);
        //}

        ////NEW: Ends the level with a success. For prototype it simply restarts stage.
        //if (other.gameObject.CompareTag("Goal"))
        //{
        //    Debug.Log("goal");
        //    //SceneManager.LoadScene(SceneManager.GetActiveScene().name);
        //}
    }
}

 //   void OnTriggerExit(Collider other)
	//{
	//	grounded = false;
 //       if (other.gameObject.CompareTag("Car"))
 //       {
 //           GetComponent<MeshRenderer>().enabled = true;
 //           other.gameObject.GetComponent<Driving_Controls>().PlayerInCar = false;

//<<<<<<< HEAD
 //           inCar = false;
 //       }
 //   }
//=======
            //NEW: Slows down cars to avoid crash.
            //other.gameObject.GetComponent<Driving_Controls>().speed = 5f;

           // inCar = false;
        //}
    //}
//>>>>>>> ed57bfea656f6215d3a1cb3cef0bee3824eb1a03
//}
