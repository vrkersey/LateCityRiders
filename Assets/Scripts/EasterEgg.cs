using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EasterEgg : MonoBehaviour {

    public GameObject easteregg;
	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}

    void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.transform.GetComponent<Driving_Controls>() && other.gameObject.transform.GetComponent<Driving_Controls>().playerInCar)
        {
            Debug.Log("win");
            easteregg.SetActive(true);
        }
        else if (other.gameObject.transform.GetComponent<Driving_Controls>() && !other.gameObject.transform.GetComponent<Driving_Controls>().playerInCar)
        {
            Destroy(this);
            Debug.Log("win");
        }
    }
}
