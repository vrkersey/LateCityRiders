using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Driving_Direct : MonoBehaviour
{
    public float acceleration, speed, slowDown, maxSpeed, turnSpeed;
    private Vector3 temp;
    public bool grounded;
    private bool playerInCar = false;
    public bool PlayerInCar { get { return playerInCar; } set { playerInCar = value; } }

    void Start()
    {
        speed = 0;
        acceleration = 0.3f;
        slowDown = 0.2f;
        maxSpeed = 60f;
        turnSpeed = 0.5f;
        grounded = false;

    }

    // Update is called once per frame
    void Update()
    {
        //ok = PlayerInCar;
        //if (playerInCar)
            KeyPress();
        //Fix this TODO:
        transform.position += -transform.forward * speed * Time.deltaTime;
    }

    void KeyPress()
    {
        if (grounded == true)
        {
            //Speeds up the vehicle manually. 
            if (Input.GetKey(KeyCode.W) || Input.GetKey(KeyCode.UpArrow))
            {
                speed += acceleration;
                if (speed > maxSpeed)
                    speed = maxSpeed;
            }

            // Slows down the vehicle manually. Acts as a reverse as well.
            if (Input.GetKey(KeyCode.S) || Input.GetKey(KeyCode.DownArrow))
            {
                if (speed > 0)
                    speed -= 2 * acceleration;
                else
                    speed -= acceleration;
                if (speed < -maxSpeed)
                    speed = -maxSpeed;

            }
        }

        //Rotate left. Attempted to rotate around back of vehicle.
        if (Input.GetKey(KeyCode.A) || Input.GetKey(KeyCode.LeftArrow))
        {
            temp = transform.position;
            temp.x += 5f;
            transform.RotateAround(temp, transform.up, -turnSpeed);
        }

        //Rotate Right. Attempted to rotate around back of vehicle.
        if (Input.GetKey(KeyCode.D) || Input.GetKey(KeyCode.RightArrow))
        {
            temp = transform.position;
            temp.x += 5f;
            transform.RotateAround(temp, transform.up, turnSpeed);
        }
        //Slows down forward moving vehicle. Sets slowed down car to 0 if necessary.
        if (speed > 0 && !Input.anyKey)
        {
            speed -= slowDown;
            if (speed < 0)
                speed = 0;
        }

        //Slows down reversing vehicle. Sets slowed down car to 0 if necessary.
        if (speed < 0 && !Input.anyKey)
        {
            speed += slowDown;
            if (speed > 0)
                speed = 0;
        }
    }

    void OnCollisionEnter(Collision other)
    {
        if (other.gameObject.CompareTag("Road"))
        {
            grounded = true;
        }
    }
    void OnCollisionExit(Collision other)
    {
        if (other.gameObject.CompareTag("Road"))
        {
            grounded = false;
        }
    }
}
