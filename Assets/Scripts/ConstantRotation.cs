using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ConstantRotation : MonoBehaviour {

    public float x;
    public float y;
    public float z;

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
        transform.RotateAround(transform.position, transform.up, x);
        transform.RotateAround(transform.position, transform.right, y);
        transform.RotateAround(transform.position, transform.forward, z);
    }
}
