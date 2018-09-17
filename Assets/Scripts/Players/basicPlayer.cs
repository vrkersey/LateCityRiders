using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class basicPlayer : MonoBehaviour, IPlayer {

    private Transform cTransform;

    public float maxControl = 15;
    public float jumpMultiplier = 15;
    public float exitSpeedVelocity = .5f;

    int SpecialsLeft = 0;
    public int CharacterSpecialAmmo = 1;

    //const float MaxSpeedForTest = 
    Rigidbody currentRB;

    void Start()
    {
        cTransform = GameObject.FindGameObjectWithTag("MainCamera").transform;
    }

    void Update()
    {
        if(currentRB)
        Debug.Log(currentRB.velocity.magnitude);
    }

    public void moveForward(Rigidbody rb, float value){
        Vector3 forward = calculateForward();
        rb.AddForce(forward * value * 10f);
    }

    public void moveRight(Rigidbody rb, float value){
        Vector3 forward = calculateForward();
        Vector3 right = Vector3.Cross(Vector3.up, forward);
        rb.AddForce(right * value * 10f);
    }

    public void exitVehicle(Rigidbody rb, GameObject car)
    {
        currentRB = rb;
        Rigidbody carRB = car.GetComponent<Rigidbody>();
        float CarSpeed = Mathf.Min(carRB.velocity.magnitude, 60f);
        float SpeedBoost = (CarSpeed / 40f);


        rb.velocity = new Vector3(carRB.velocity.x * SpeedBoost, (carRB.velocity.magnitude / 4 ) * SpeedBoost, carRB.velocity.z * SpeedBoost);
        SpecialsLeft = CharacterSpecialAmmo;
    }

    public void useSpecial(Rigidbody rb){
        if(SpecialsLeft > 0)
        {
            Debug.Log("use special");
            SpecialsLeft -= 1;
            rb.velocity = new Vector3(rb.velocity.x/2, 10f, rb.velocity.z/2);
        }
    }

    private Vector3 calculateForward(){
        Vector3 forward = (this.transform.position - cTransform.position);
        forward.y = 0;
        forward = forward.normalized;
        return forward;
    }
}
