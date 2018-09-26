using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CarSpawner : MonoBehaviour {

    public GameObject car;
    public float timereset = 3f;
    float timer;
    GameObject cartokill;

    public bool dontdestroy;

	// Use this for initialization
	void Start () {
        timer = timereset;
	}
	
	// Update is called once per frame
	void Update () {
		if(timer > 0)
        {
            timer -= Time.deltaTime;
        }
        else
        {
            if (!dontdestroy && cartokill && !cartokill.transform.GetComponent< Driving_Controls>().playerInCar)
            {
                Destroy(cartokill);

                cartokill = Instantiate(car, transform.position, car.transform.rotation);
            }
            else
            {
                Instantiate(car, transform.position, car.transform.rotation);
            }
            timer = timereset;
        }
	}
}
