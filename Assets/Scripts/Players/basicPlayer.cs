using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class basicPlayer : MonoBehaviour, IPlayer {

    private Transform cTransform;

    float characterAcceleration = 10f;
    float maxSpeedThisJump;

    int SpecialsLeft = 0;
    public int CharacterSpecialAmmo = 1;

    Vector3 ForceToAdd;

    //const float MaxSpeedForTest = 
    Rigidbody currentRB;

    void Start()
    {
        cTransform = GameObject.FindGameObjectWithTag("MainCamera").transform;
    }

    void Update()
    {
        if (currentRB)
        {
            //Debug.Log(currentRB.velocity.magnitude);
            ForceToAdd = ForceToAdd.normalized;
            currentRB.AddForce(ForceToAdd * characterAcceleration);

            //max speed check, and reduce horizontal velocity if needed;
            Vector3 HorVelocityCheck = new Vector3(currentRB.velocity.x, 0, currentRB.velocity.z);

            if (HorVelocityCheck.magnitude > maxSpeedThisJump)
            {
                float saveY = currentRB.velocity.y;
                HorVelocityCheck = HorVelocityCheck.normalized;
                HorVelocityCheck *= maxSpeedThisJump;
                currentRB.velocity = new Vector3(HorVelocityCheck.x, saveY, HorVelocityCheck.z);
            }
        }
    }

    public void moveForward(Rigidbody rb, float value){
        ForceToAdd = new Vector3(-value, ForceToAdd.y, ForceToAdd.z);

        //Vector3 forward = calculateForward();
        //rb.AddForce(forward * value * 10f);
    }

    public void moveRight(Rigidbody rb, float value){
        
        ForceToAdd = new Vector3(ForceToAdd.x, ForceToAdd.y, value);
        //Vector3 forward = calculateForward();
        //Vector3 right = Vector3.Cross(Vector3.up, forward);
        //rb.AddForce(right * value * 10f);
    }

    public void exitVehicle(Rigidbody rb, GameObject car)
    {
        Debug.Log("Exit");
        currentRB = rb;
        //Rigidbody carRB = car.GetComponent<Rigidbody>();
        Driving_Controls CarControl = car.GetComponent<Driving_Controls>();
        float CarSpeed = CarControl.speed;
        maxSpeedThisJump = CarSpeed;
        float SpeedBoost = (CarControl.speed / (CarControl.maxSpeed * 0.8f));


        //add jump
        Debug.Log(((CarSpeed / 4) * SpeedBoost) + 5000f);
        rb.velocity = new Vector3(0f, ((CarSpeed / 4 ) * SpeedBoost) + 5, 0f);
        //add velocity
        rb.velocity -= car.transform.forward * CarSpeed * SpeedBoost;
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
