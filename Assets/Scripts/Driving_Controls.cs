using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Driving_Controls : MonoBehaviour
{
    public float acceleration, speed, slowDown, maxSpeed, turnSpeed;
    private Vector3 temp;
    public bool grounded;
    private bool playerInCar = false;
    public bool PlayerInCar { get { return playerInCar; } set { playerInCar = value; } }

    //NEW: Set in inspector to determine what type of vehicle we're dealing with.
    //Currently either "Plow" or "Basic"
    public enum Vehicle { Basic = 0, Plow};
    public Vehicle VehicleSelect;
    Quaternion originalRotation;

    public Vector3 PlayerPositionInCar = new Vector3(0,1,0);

    void Start()
    {
        speed = 0;
        originalRotation = transform.rotation;

        //NEW: Sets variables for the plow vehicle.
        if(VehicleSelect == Vehicle.Plow)
        {
            acceleration = 0.2f;
            slowDown = 0.2f;
        }

        //Sets variables for a basic vehicle.
        else 
        {
            acceleration = 0.3f;
            slowDown = 0.15f;
        }
        maxSpeed = 60f;
        turnSpeed = 0.8f;
        grounded = false;

    }

    // Update is called once per frame
    void Update()
    {
        if (playerInCar) {
            //Debug.Log("speed " + speed);
           // Debug.Log("max speed " + maxSpeed);
        }
        

        if (PlayerInCar)
            KeyPress();

        //ideally this will be done with a box cast(s) in the future
        grounded = false;
        if (Physics.Raycast(transform.position, -transform.up, 5f))
        {
            grounded = true;
        }

        //set velocity to speed
        if (grounded && playerInCar)
        {
            transform.GetComponent<Rigidbody>().velocity = new Vector3(speed * transform.forward.x, transform.GetComponent<Rigidbody>().velocity.y, speed * transform.forward.z);
            //transform.GetComponent<Rigidbody>().velocity = speed * transform.forward;

        }
        else
        {
            //leave it to physics in the air
            speed = new Vector3(transform.GetComponent<Rigidbody>().velocity.x, 0, transform.GetComponent<Rigidbody>().velocity.z).magnitude;
        }
        
    }

    void KeyPress()
    {
        //If the player is grounded, the player can accelerate. The following if statements control acceleration.
        if (grounded == true)
        {
            //Speeds up the vehicle manually. 
            if ((Input.GetKey(KeyCode.W) || Input.GetKey(KeyCode.UpArrow)) && (!Input.GetKey(KeyCode.S) || !Input.GetKey(KeyCode.DownArrow)))
            {
                //Debug.Log("Forward");
                if (speed < 0)
                    speed += 2 * acceleration;
                else
                    speed += acceleration;
                if (speed > maxSpeed)
                    speed = maxSpeed;
            }

            // Slows down the vehicle manually. Acts as a reverse as well.
            if (Input.GetKey(KeyCode.S) || Input.GetKey(KeyCode.DownArrow) && (!Input.GetKey(KeyCode.W) || !Input.GetKey(KeyCode.UpArrow)))
            {
                if (speed > 0)
                    speed -= 2 * acceleration;
                else
                    speed -= acceleration;

                //NEW: Keeps player from reversing at an unreasonable speed.
                if (speed < -maxSpeed/2)
                    speed = -maxSpeed/2;
            }

            //Slows down forward moving vehicle. Sets slowed down car to 0 if necessary.
            if (speed > 0 && !Input.GetKey(KeyCode.S) && !Input.GetKey(KeyCode.DownArrow) && !Input.GetKey(KeyCode.W) && !Input.GetKey(KeyCode.UpArrow))
            {
                speed -= slowDown;
                if (speed < 0)
                    speed = 0;
            }

            //Slows down reversing vehicle. Sets slowed down car to 0 if necessary.
            if (speed < 0 && !Input.GetKey(KeyCode.S) && !Input.GetKey(KeyCode.DownArrow) && !Input.GetKey(KeyCode.W) && !Input.GetKey(KeyCode.UpArrow))
            {
                speed += slowDown;
                if (speed > 0)
                    speed = 0;
            }
        }

        //The following do not rely on being grounded. These control car direction.
        //Rotate left. Attempted to rotate around back of vehicle.
        //NEW: If on the ground car turns based on front of vehicle. Otherwise it turns around middle of car.
        if ((Input.GetKey(KeyCode.A) || Input.GetKey(KeyCode.LeftArrow)) && speed != 0)
        {
            temp = transform.position;
            if (grounded == true)
                temp.x += 3f;
            transform.RotateAround(temp, transform.up, -turnSpeed);
        }

        //Rotate Right. Attempted to rotate around back of vehicle.
        if ((Input.GetKey(KeyCode.D) || Input.GetKey(KeyCode.RightArrow)) && speed != 0)
        {
            temp = transform.position;
            if(grounded == true)
                temp.x += 3f;
            transform.RotateAround(temp, transform.up, turnSpeed);
        }

        if (Input.GetKey(KeyCode.R))
        {
            speed = 0;
            transform.rotation = originalRotation;
        }
    }

   
    ////On collison enter may not be necessary... TODO
    ////When driver lands on ground, regain acceleration based control.
    //NEW: Added collision for blocks. Only plows can move past these without losing/being flipped. 
    //Basic cars are stopped immediately, Plows cause blocks to be knocked away.

    //Sidenote:: May want to make turning off the kinematic a one time thing. That way, if a box is knocked onto traffic traffic won't necessarily stop when the car collides and sets kinematic to true.
    //TODO: see if above note is viable.
    void OnTriggerEnter(Collider other)
    {
        if(other.gameObject.CompareTag("Blocks") && PlayerInCar)
        {
            if(VehicleSelect == Vehicle.Plow)
            {
                Destroy(other.gameObject);
            }
        }
    }
    //void OnCollisionEnter(Collision other)
    //{
    //    if (other.gameObject.CompareTag("Blocks") && playerInCar)
    //    {
    //        if (vehicleType != "Plow")
    //        {
    //            other.gameObject.GetComponent<Rigidbody>().isKinematic = true;
    //        }
    //        else
    //        {
    //            other.gameObject.GetComponent<Rigidbody>().isKinematic = false;
    //        }
    //    }
    //}

    ////this seems redundant if we have oncollisionenter and oncollisionexit
    ////When driver is currently on ground, maintain acceleration control. 
    //void OnCollisionStay(Collision other)
    //{
    //    foreach (ContactPoint contact in other.contacts)
    //    {
    //        if (other.gameObject.CompareTag("Road"))
    //        {
    //            grounded = true;
    //        }
    //    }
    //}

    ////When driver detatches from ground, remove driver's ability to accelerate.
    //void OnCollisionExit(Collision other)
    //{
    //    if (other.gameObject.CompareTag("Kill Zone"))
    //    {
    //        grounded = false;
    //    }
    //}
}


//using System.Collections;
//using System.Collections.Generic;
//using UnityEngine;

//public class Driving_Controls : MonoBehaviour
//{

//    public float currentspeed, acceleration, topspeed, bottomspeed;
//    public Rigidbody rb;

//    private bool playerInCar = false;
//    public bool PlayerInCar { get { return playerInCar; } set { playerInCar = value; }}

//	// Update is called once per frame
//	void Update()
//    {   
//        if (playerInCar)
//            KeyPress();
//      //  transform.Translate(currentspeed, 0, 0);
//	}
//    private void KeyPress()
//    {
//        //Forward acceleration
//        if (Input.GetKey(KeyCode.W) || Input.GetKey(KeyCode.UpArrow)) 
//        {
//            rb.AddForce(-transform.forward * acceleration);
//            //if (currentspeed <= topspeed)
//            //    currentspeed = currentspeed + iteration;
//            //if (currentspeed > topspeed)
//            //    currentspeed = topspeed;
//        }

//        //Reverse/slowing down
//        if (Input.GetKey(KeyCode.S) || Input.GetKey(KeyCode.DownArrow)) 
//        {
//            rb.AddForce(transform.forward * acceleration);
//            //if(currentspeed >= bottomspeed)
//            //{
//            //    currentspeed = currentspeed - iteration;
//            //    if (currentspeed < bottomspeed)
//            //        currentspeed = bottomspeed;
//            //}
//        }

//        //Rotate left
//        if (Input.GetKey(KeyCode.A) || Input.GetKey(KeyCode.LeftArrow))
//        {
//            transform.Rotate(0, -0.5F, 0);
//        }

//        //Rotate Right
//        if (Input.GetKey(KeyCode.D) || Input.GetKey(KeyCode.RightArrow))
//        {
//            transform.Rotate(0, 0.5F, 0);
//        }

//        //Reverse Direction and instant stop (Debugging)
//        if (Input.GetKeyUp(KeyCode.Space))
//        {
//            transform.Rotate(0, 180, 0);

//            rb.velocity = new Vector3(0,0,0);
//        }

//    }
//}
