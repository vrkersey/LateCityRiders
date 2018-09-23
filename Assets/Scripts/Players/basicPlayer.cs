using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class basicPlayer : MonoBehaviour, IPlayer {

    private Transform cTransform;

    //characters
    //NEW: Added selected char, which uses playerprefs.
    public enum Character {BusinessMan = 0, Karate, Firework};
    public Character CharacterSelect;

    //character variables

    //karate
    private float timeInAir = -1;

    //firework
    public GameObject rocketModel;
    private float rocketTimer = -1;
    float rocketTimeSet = 5f;
    float rocketPitch;
    float rocketPitchMax = 50f;
    float rocketAccel = 50f;
    float rocketSpeedBoostFromPitch;
    


    //end character variables

    public float characterAcceleration = 20f;
    private float maxSpeedThisJump;

    private int SpecialsLeft = 0;
    public int CharacterSpecialAmmo = 1;

    private Vector3 ForceToAdd;
    [HideInInspector]
    public Vector3 HorVelocityCheck;

    public float SpeedBoost = 20;

    //const float MaxSpeedForTest = 
    Rigidbody currentRB;

    void Start()
    {
        cTransform = GameObject.FindGameObjectWithTag("MainCamera").transform;
        //CharacterSelect = (Character)PlayerPrefs.GetInt("Character");
    }

    void Update()
    {
        //Debug.Log("maxspeedthisjump" + maxSpeedThisJump);

        
        if (currentRB)
        {
            ForceToAdd = ForceToAdd.normalized;

            //rocket movement
            if (CharacterSelect == Character.Firework && rocketTimer > 0)
            {
                rocketModel.SetActive(true );

                //Debug.Log(rocketTimer);
                rocketTimer -= Time.deltaTime;
                //currentRB.velocity = (calculateForward() * maxSpeedThisJump);


                currentRB.velocity = new Vector3(currentRB.velocity.x, 0, currentRB.velocity.z);
                currentRB.velocity = Vector3.RotateTowards(currentRB.velocity, calculateForward(), 1.57f * Time.deltaTime, 0f);
                
                currentRB.velocity = currentRB.velocity.normalized * (maxSpeedThisJump + rocketSpeedBoostFromPitch);
                currentRB.velocity = new Vector3(currentRB.velocity.x, rocketPitch, currentRB.velocity.z);

            }
            //normal movement
            else
            {
                rocketModel.SetActive(false);

                //Debug.Log(currentRB.velocity.magnitude);
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


        //karate
        if (timeInAir > -1)
        {
            timeInAir += Time.deltaTime;
        }
    }

    public void moveForward(Rigidbody rb, float value){
        //ForceToAdd = new Vector3(-value, ForceToAdd.y, ForceToAdd.z);
        ForceToAdd = new Vector3(ForceToAdd.x, ForceToAdd.y, ForceToAdd.z) + calculateForward() * value;


        //firework
        if (CharacterSelect == Character.Firework && rocketTimer > 0)
        {
            rocketPitch -= value * Time.deltaTime * rocketAccel;
            if (rocketPitch > rocketPitchMax)
            {
                rocketPitch = rocketPitchMax;
            }
            else if (rocketPitch < -rocketPitchMax)
            {
                rocketPitch = -rocketPitchMax;
            }

            rocketSpeedBoostFromPitch -= rocketPitch / 0.5f * Time.deltaTime;
            float rocketSpeedBoostFromPitchMax = maxSpeedThisJump / 2;
            if (rocketSpeedBoostFromPitch > rocketSpeedBoostFromPitchMax)
            {
                rocketSpeedBoostFromPitch = rocketSpeedBoostFromPitchMax;
            }
            else if (rocketSpeedBoostFromPitch < -rocketSpeedBoostFromPitchMax)
            {
                rocketSpeedBoostFromPitch = -rocketSpeedBoostFromPitchMax;
            }
        }
        

        //Vector3 forward = calculateForward();
        //rb.AddForce(forward * value * 10f);
    }

    public void moveRight(Rigidbody rb, float value){

        //ForceToAdd = new Vector3(ForceToAdd.x, ForceToAdd.y, value);
        ForceToAdd = new Vector3(ForceToAdd.x, ForceToAdd.y, ForceToAdd.z) + Vector3.Cross(Vector3.up, calculateForward()) * value;

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
        float SpeedBoost = (CarSpeed / (CarControl.maxSpeed * 0.8f));
        maxSpeedThisJump = Mathf.Max(CarSpeed * SpeedBoost, 3f);
        //float CarDirection = CarSpeed / CarControl.speed;


        //add jump
        rb.velocity = new Vector3(0f, ((CarSpeed / 4) * SpeedBoost) + 5, 0f);

        //add velocity
        rb.velocity += new Vector3(car.transform.GetComponent<Rigidbody>().velocity.x, 0, car.transform.GetComponent<Rigidbody>().velocity.z) * CarSpeed * SpeedBoost;

        //reset ammo
        SpecialsLeft = CharacterSpecialAmmo;

        //reset specials
        timeInAir = -1;
        rocketTimer = -1f;
    }

    public void useSpecial(Rigidbody rb){
        if(CharacterSelect == Character.BusinessMan)
        {
            if (SpecialsLeft > 0)
            {
                Debug.Log("use double jump");
                SpecialsLeft -= 1;
                rb.velocity = new Vector3(1 * rb.velocity.x / 4, 10f, 1 * rb.velocity.z / 4);
                maxSpeedThisJump *= .5f;
            }
        }
        else if(CharacterSelect == Character.Karate)
        {
            if (SpecialsLeft > 0)
            {
                Debug.Log("use divekick");
                SpecialsLeft -= 1;
                Vector3 dir = GetHorVelocityCheck().normalized;
                rb.velocity = new Vector3(0 * rb.velocity.x / 2, -50f, 0 * rb.velocity.z / 2);
                rb.velocity += dir * 5f;
                maxSpeedThisJump *= .5f;

                timeInAir = 1f;
            }
        }
        else if (CharacterSelect == Character.Firework)
        {
            if(SpecialsLeft > 0)
            {
                Debug.Log("use firework");
                SpecialsLeft -= 1;
                maxSpeedThisJump *= 1.2f;
                rocketTimer = rocketTimeSet;
                rocketPitch = 0f;
                rocketSpeedBoostFromPitch = 0f;
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
        if(CharacterSelect == Character.Karate && SpecialsLeft == 0)
        {

            Debug.Log("timeinair " + timeInAir);
            Debug.Log("car boost " + HorVelocityCheck.magnitude * 5f);
            Debug.Log("car boost 2 " + HorVelocityCheck.magnitude * 5f * timeInAir);
            return HorVelocityCheck * 5f * timeInAir * timeInAir;
        }
        if (CharacterSelect == Character.Firework && SpecialsLeft == 0)
        {

            Debug.Log("rocketSpeedBoostFromPitch " + rocketSpeedBoostFromPitch);
            return HorVelocityCheck + HorVelocityCheck.normalized * rocketSpeedBoostFromPitch * 2;
        }
        return HorVelocityCheck;
    }

    public bool IsRocketMode()
    {
        if (CharacterSelect == Character.Firework && rocketTimer > 0)
        {
            return true;
        }
        return false;
    }

    public int GetSpecialsLeft()
    {
        return SpecialsLeft;
    }

    public void EnterVehicleCleanUp()
    {
        rocketModel.SetActive(false);
    }
}
