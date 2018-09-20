using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class basicPlayer : MonoBehaviour, IPlayer {

    private Transform cTransform;

    public enum Character {BusinessMan = 0, Karate};
    public Character CharacterSelect;

    float characterAcceleration = 20f;
    float maxSpeedThisJump;

    int SpecialsLeft = 0;
    public int CharacterSpecialAmmo = 1;

    Vector3 ForceToAdd;
    public Vector3 HorVelocityCheck;

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
            HorVelocityCheck = new Vector3(currentRB.velocity.x, 0, currentRB.velocity.z);

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
        float CarSpeed = Mathf.Abs(CarControl.speed);
        maxSpeedThisJump = CarSpeed;
        float SpeedBoost = (CarSpeed / (CarControl.maxSpeed * 0.8f));
        //float CarDirection = CarSpeed / CarControl.speed;


        //add jump
        rb.velocity = new Vector3(0f, ((CarSpeed / 4 ) * SpeedBoost) + 5, 0f);

        //add velocity
        rb.velocity += new Vector3(car.transform.GetComponent<Rigidbody>().velocity.x, 0, car.transform.GetComponent<Rigidbody>().velocity.z) * CarSpeed * SpeedBoost;

        //reset ammo
        SpecialsLeft = CharacterSpecialAmmo;
    }

    public void useSpecial(Rigidbody rb){
        if(CharacterSelect == Character.BusinessMan)
        {
            if (SpecialsLeft > 0)
            {
                Debug.Log("use double jump");
                SpecialsLeft -= 1;
                rb.velocity = new Vector3(0 * rb.velocity.x / 2, 10f, 0 * rb.velocity.z / 2);
                maxSpeedThisJump *= .5f;
            }
        }
        else if (CharacterSelect == Character.Karate)
        {
            if (SpecialsLeft > 0)
            {
                Debug.Log("use divekick");
                SpecialsLeft -= 1;
                rb.velocity = new Vector3(0 * rb.velocity.x / 2, -20f, 0 * rb.velocity.z / 2);
                maxSpeedThisJump *= .5f;
            }
        }

    }

    private Vector3 calculateForward(){
        Vector3 forward = (this.transform.position - cTransform.position);
        forward.y = 0;
        forward = forward.normalized;
        return forward;
    }

    public Vector3 GetHorVelocityCheck()
    {
        return HorVelocityCheck;
    }
}
