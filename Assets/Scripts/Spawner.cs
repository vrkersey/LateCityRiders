using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Spawner : MonoBehaviour {

    public GameObject BusinessMan;
    public GameObject Karate;
    public GameObject Firework;

	// Use this for initialization
	void Start () {
        switch (PlayerPrefs.GetInt("Character"))
        {
            case 0:
                Debug.Log("b");
                Instantiate(BusinessMan, transform.position, transform.rotation);
                break;
            case 1:
                Instantiate(Karate, transform.position, transform.rotation);
                break;
            case 2:
                Instantiate(Firework, transform.position, transform.rotation);
                break;
            default:

                Debug.Log("no" + PlayerPrefs.GetInt("Character"));
                break;
        }
    }
	
	// Update is called once per frame
	void Update () {
		
	}
}
