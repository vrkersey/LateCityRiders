using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Destruction : MonoBehaviour {

    public GameObject DestructMesh;
    
	void Update()
    {
        if (Input.GetKey(KeyCode.Space))
        {
            Destruct();
        }
    }
	void Destruct()
    {
        Instantiate(DestructMesh,gameObject.transform.position,gameObject.transform.rotation);
        Destroy(gameObject);
    }
}
