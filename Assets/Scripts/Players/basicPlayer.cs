using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class basicPlayer : MonoBehaviour, IPlayer{
    private Transform cTransform;

    void Start()
    {
        cTransform = GameObject.FindGameObjectWithTag("MainCamera").transform;
    }

    void Update()
    {

    }

    public void moveForward(Rigidbody rb, float value){
        Vector3 forward = calculateForward();
        rb.AddForce(forward * value);
    }

    public void moveRight(Rigidbody rb, float value){
        Vector3 forward = calculateForward();
        Vector3 right = Vector3.Cross(Vector3.up, forward);
        rb.AddForce(right * value);
    }

    public void exitVehicle(Rigidbody rb)
    {

    }

    public void useSpecial(Rigidbody rb){

    }

    private Vector3 calculateForward(){
        Vector3 forward = (this.transform.position - cTransform.position);
        forward.y = 0;
        forward = forward.normalized;
        return forward;
    }
}
