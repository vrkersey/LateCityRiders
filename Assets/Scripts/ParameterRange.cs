using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ParameterRange : MonoBehaviour
{
    public GameObject Player, Parameter;
    public float timeLimit, timePassed;

    //Checks when the trigger parameter should deactivate
    void FixedUpdate()
    {
        timePassed += Time.deltaTime;
        if (timePassed > timeLimit)
        {
            Parameter.SetActive(false);
        }
    }

    public void StartGrab()
    {
        Debug.Log("BeginJump");
        timePassed = 0;
    }

    //If a trigger is entered and it's with a car, enter car.
    //Debug... Maybe be able to exit and immediately reenter original vehicle. Take steps to fix after code works.
    void OnTriggerEnter(Collider other)
    {
        Debug.Log(other.gameObject.tag);
        if (other.gameObject.CompareTag("Car"))
        {
            Player.GetComponent<playerController>().BeginEntrance(other);
            Parameter.SetActive(false);
        }
    }
}
