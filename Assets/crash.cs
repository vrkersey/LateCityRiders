using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class crash : MonoBehaviour {

    public bool crashed;

	// Use this for initialization
	void Start () {
		
	}

    // Update is called once per frame
    void Update()
    {


    }

    void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("Kill Zone") || other.gameObject.CompareTag("Car") || other.gameObject.CompareTag("Goal"))
        {
            Debug.Log("crashed");
            crashed = true;
        }
    }
}

    
