﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine.SceneManagement;
using UnityEngine;

public class playerController_Direct : MonoBehaviour
{
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
    }

    private void Keyboard_Input()
    {
        if (Input.GetKey(KeyCode.W) && !inCar)
        {
            thePlayer.moveForward(rb, 1);
        }
        if (Input.GetKey(KeyCode.S) && !inCar)
        {
            thePlayer.moveForward(rb, -1);
        }
        if (Input.GetKey(KeyCode.D) && !inCar)
        {
            thePlayer.moveRight(rb, 1);
        }
        if (Input.GetKey(KeyCode.A) && !inCar)
        {
            thePlayer.moveRight(rb, -1);
        }

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
            other.gameObject.GetComponent<Driving_Direct>().PlayerInCar = true;
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
            other.gameObject.GetComponent<Driving_Direct>().PlayerInCar = false;
            inCar = false;
        }
    }
}
