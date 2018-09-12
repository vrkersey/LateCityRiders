using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Driving_Controls : MonoBehaviour
{

    public float currentspeed, acceleration, topspeed, bottomspeed;
    public Rigidbody rb;

	// Update is called once per frame
	void Update()
    {   
        KeyPress();
      //  transform.Translate(currentspeed, 0, 0);
	}
    private void KeyPress()
    {
        //Forward acceleration
        if (Input.GetKey(KeyCode.W) || Input.GetKey(KeyCode.UpArrow)) 
        {
            rb.AddForce(transform.forward * acceleration);
            //if (currentspeed <= topspeed)
            //    currentspeed = currentspeed + iteration;
            //if (currentspeed > topspeed)
            //    currentspeed = topspeed;
        }

        //Reverse/slowing down
        if (Input.GetKey(KeyCode.S) || Input.GetKey(KeyCode.DownArrow)) 
        {
            rb.AddForce(-transform.forward * acceleration);
            //if(currentspeed >= bottomspeed)
            //{
            //    currentspeed = currentspeed - iteration;
            //    if (currentspeed < bottomspeed)
            //        currentspeed = bottomspeed;
            //}
        }

        //Rotate left
        if (Input.GetKey(KeyCode.A) || Input.GetKey(KeyCode.LeftArrow))
        {
            transform.Rotate(0, -0.5F, 0);
        }

        //Rotate Right
        if (Input.GetKey(KeyCode.D) || Input.GetKey(KeyCode.RightArrow))
        {
            transform.Rotate(0, 0.5F, 0);
        }

        //Reverse Direction and instant stop (Debugging)
        if (Input.GetKeyUp(KeyCode.Space))
        {
            transform.Rotate(0, 180, 0);
         
            rb.velocity = new Vector3(0,0,0);
        }

    }
}
