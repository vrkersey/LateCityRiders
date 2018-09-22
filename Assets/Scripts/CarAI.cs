using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class CarAI : MonoBehaviour {

    public Transform target;
    

	// Use this for initialization
	void Start () {
        if(target != null)
            GetComponent<NavMeshAgent>().SetDestination(target.position);
	}
	
	// Update is called once per frame
	void Update () {
	    
	}
}
