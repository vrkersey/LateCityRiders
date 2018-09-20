using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class basicCameraScript : MonoBehaviour {
	public Transform player;
	public Transform mainCamera;
	public Vector3 angle = new Vector3(0, -3f, 10);

	// Use this for initialization
	void Start()
	{
		player = GameObject.FindGameObjectWithTag("Player").transform;
	}

	// Update is called once per frame
	void Update () {
		transform.position = player.position - angle;
	}
}
