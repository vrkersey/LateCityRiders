using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class basicPlayerOLD : MonoBehaviour, IPlayer {

    public Transform cTransform;

    //characters
    //NEW: Added selected char, which uses playerprefs.
    public enum Character {BusinessMan = 0, Karate, Firework};
    public Character CharacterSelect;

    //character variables

    //businessman
    public GameObject briefcasetospawn;

    //karate
    public float magStorage;
    private float timeInAir = -1;

    //firework
    public GameObject rocketModel;
    private float rocketTimer = -1;
    float rocketTimeSet = 5f;
    float rocketPitch;
    float rocketPitchMax = 50f;
    float rocketAccel = 50f;
    float rocketSpeedBoostFromPitch;
    float rocketSpeed;
    float newMaxSpeed;
    float rocketgravity;


    //end character variables
    public Animator CharAnim;

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

    float jumpholdtimer;
    float jumpholdlimit = 0.25f;
    Vector3 jumpAdd;
    Vector3 jumpAddTo;

    void Start()
    {
        //Debug.Log("try to get camera");
        cTransform = GameObject.FindGameObjectWithTag("MainCamera").transform;
        //CharacterSelect = (Character)PlayerPrefs.GetInt("Character");
        
    }

    public void SetcTransform(Transform cam)
    {
        cTransform = cam;
    }

    void Update()
    {
        //Debug.Log("maxspeedthisjump" + maxSpeedThisJump);
        
        if(!cTransform)
        {
            cTransform = GameObject.FindGameObjectWithTag("MainCamera").transform;
        }        

        if (currentRB)
        {

            if (jumpholdtimer > 0)
            {
                jumpholdtimer -= Time.deltaTime;
                jumpAddTo += jumpAdd * Time.deltaTime / jumpholdlimit;
                currentRB.velocity = new Vector3(currentRB.velocity.x, jumpAddTo.y, currentRB.velocity.z);
            }


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

                //currentRB.velocity = currentRB.velocity.normalized * (maxSpeedThisJump + rocketSpeedBoostFromPitch); original, no buildup
                currentRB.velocity = currentRB.velocity.normalized * (rocketSpeed + rocketSpeedBoostFromPitch);
                if(rocketPitch > 0)
                {
                    rocketgravity += Time.deltaTime * (rocketPitch) / 1 ;
                }
                if(rocketPitch <= 0)
                {
                    //rocketgravity += Time.deltaTime * (rocketPitch / .1f);
                    

                }
                rocketgravity -= Time.deltaTime * 10f;
                if (rocketgravity <= 0)
                {
                    rocketgravity = 0;
                }

                Debug.Log(rocketgravity);
                currentRB.velocity = new Vector3(currentRB.velocity.x / (rocketgravity * 0.1f + 1f), rocketPitch , currentRB.velocity.z / (rocketgravity * 0.1f + 1f));
                transform.position -= Vector3.up * rocketgravity * Time.deltaTime;

                //get new max speed
                newMaxSpeed = Mathf.Max(rocketSpeed + rocketSpeedBoostFromPitch, newMaxSpeed);
                if(rocketSpeedBoostFromPitch <0)
                {
                    rocketSpeed += Time.deltaTime * (-rocketSpeedBoostFromPitch);
                    if(rocketSpeed > newMaxSpeed)
                    {
                        rocketSpeed = newMaxSpeed;
                    }
                }
                else
                {
                    rocketSpeed -= Time.deltaTime * 0.5f;
                }
                maxSpeedThisJump = rocketSpeed;
                if(rocketTimer <= 0)
                {
                    currentRB.velocity = new Vector3(currentRB.velocity.x, rocketPitch - rocketgravity, currentRB.velocity.z);
                }
                //Debug.Log("rocketSpeed " + rocketSpeed);
                //Debug.Log("newMaxSpeed " + newMaxSpeed);
            }
            //normal movement
            else
            {
                if(CharacterSelect == Character.Firework)
                {

                    rocketModel.SetActive(false);
                }

                //Debug.Log(currentRB.velocity.magnitude);
                //Debug.Log(ForceToAdd);
                currentRB.AddForce(ForceToAdd * characterAcceleration);
                ForceToAdd = Vector3.zero; 

                //max speed check, and reduce horizontal velocity if needed;
                HorVelocityCheck = new Vector3(currentRB.velocity.x, 0, currentRB.velocity.z);

                if (HorVelocityCheck.magnitude > maxSpeedThisJump)
                {
                    float saveY = currentRB.velocity.y;
                    HorVelocityCheck = HorVelocityCheck.normalized;
                    HorVelocityCheck *= maxSpeedThisJump;
                    currentRB.velocity = new Vector3(HorVelocityCheck.x, saveY, HorVelocityCheck.z);
                }
                //Animation code
                float AnimatorSpeed;
                AnimatorSpeed = HorVelocityCheck.magnitude;
                CharAnim.SetFloat("Speed", AnimatorSpeed);
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
            rocketPitch -= Time.deltaTime * 2;
            if (rocketPitch > rocketPitchMax)
            {
                rocketPitch = rocketPitchMax;
            }
            else if (rocketPitch < -rocketPitchMax)
            {
                rocketPitch = -rocketPitchMax;
            }

            if(rocketPitch > 0)
            {
                rocketSpeedBoostFromPitch -= (rocketPitch - rocketgravity) / 1f * (Mathf.Abs( rocketSpeed / 50)) * Time.deltaTime;
            }
            else if (rocketPitch <= 0)
            {
                rocketSpeedBoostFromPitch -= (rocketPitch - rocketgravity) / 1f * (Mathf.Abs(rocketSpeed / 50)) * Time.deltaTime;
            }
            
            //if(rocketPitch > 0)
            //{
            //    rocketSpeedBoostFromPitch -= rocketPitch / 0.5f * Time.deltaTime;
            //}
            //float rocketSpeedBoostFromPitchMax = maxSpeedThisJump / 2; original, no buildup
            float rocketSpeedBoostFromPitchMax = rocketSpeed / 2;
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
        jumpAdd = new Vector3(0f, ((CarSpeed / 4) * SpeedBoost), 0f);
        jumpholdtimer = jumpholdlimit;
        jumpAddTo = new Vector3(0, +5, 0);

        //add velocity
        rb.velocity += new Vector3(car.transform.GetComponent<Rigidbody>().velocity.x, 0, car.transform.GetComponent<Rigidbody>().velocity.z) * CarSpeed * SpeedBoost;
        //reset ammo
        SpecialsLeft = CharacterSpecialAmmo;

        //reset specials
        CharAnim.SetBool("Is_Special", false);
        timeInAir = -1;
        rocketTimer = -1f;
        rocketgravity = 0;
        rocketSpeed = 0;
    }

    public void releaseJump(Rigidbody rb)
    {
        jumpholdtimer = 0f;

        if(CharacterSelect == Character.Firework && rocketTimer >0)
        {
            currentRB.velocity = new Vector3(currentRB.velocity.x, rocketPitch - rocketgravity, currentRB.velocity.z);
            rocketTimer = 0f;
        }
        
    }

    public void useSpecial(Rigidbody rb){

        CharAnim.SetBool("Is_Special", true);
        if (CharacterSelect == Character.BusinessMan)
        {
            if (SpecialsLeft > 0)
            {
                GameObject b = Instantiate(briefcasetospawn, transform.position - Vector3.up * 1f, CharAnim.transform.rotation);
                Debug.Log("CharAnim.transform.rotation " + CharAnim.transform.rotation);
                //b.transform.LookAt(GetHorVelocityCheck());
                //b.transform.rotation = transform.rotation;
                //b.transform.eulerAngles = new Vector3(b.transform.eulerAngles.x, b.transform.eulerAngles.y + 90f, b.transform.eulerAngles.z);
                //Debug.Log("brieflooki" + GetHorVelocityCheck());
                Debug.Log("br2 " + b.transform.rotation);

                //Debug.Log("use double jump");
                CharAnim.SetTrigger("Special_Buisness");
                SpecialsLeft -= 1;
                rb.velocity = new Vector3(1 * rb.velocity.x / 4, 0, 1 * rb.velocity.z / 4);
                jumpAdd = new Vector3(0, Mathf.Max(rb.velocity.y, 0) + 5, 0);
                jumpholdtimer = jumpholdlimit;
                jumpAddTo = new Vector3(0, +5f, 0);
                maxSpeedThisJump *= .9f;

                
            }
        }
        else if(CharacterSelect == Character.Karate)
        {
            if (SpecialsLeft > 0)
            {
                Debug.Log("use divekick");
                CharAnim.SetTrigger("Special_Karate");
                SpecialsLeft -= 1;
                Vector3 dir = GetHorVelocityCheck().normalized;
                magStorage = rb.velocity.magnitude;
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
                rocketSpeed = maxSpeedThisJump;
            }
        }

    }

    private Vector3 calculateForward(){
        if (!cTransform)
        {
            cTransform = GameObject.FindGameObjectWithTag("MainCamera").transform;
        }
        Vector3 forward = (this.transform.position - cTransform.position);
        forward.y = 0;
        forward = forward.normalized;
        //Debug.Log("forward " + forward);
        return forward;
    }

    public Vector3 GetHorVelocityCheck()
    {
        if(CharacterSelect == Character.BusinessMan)
        {
            return HorVelocityCheck * 1.5f;
        }
        if(CharacterSelect == Character.Karate && SpecialsLeft == 0)
        {

            //Debug.Log("timeinair " + timeInAir);
            //Debug.Log("car boost " + HorVelocityCheck.magnitude * 5f);
            //Debug.Log("car boost 2 " + HorVelocityCheck.magnitude * 5f * timeInAir);
            Debug.Log("car boost 2 " + (HorVelocityCheck.normalized * magStorage * 1.3f));
            return HorVelocityCheck.normalized * magStorage * 1.5f;
        }
        if (CharacterSelect == Character.Firework && SpecialsLeft == 0)
        {

            //Debug.Log("rocketSpeedBoostFromPitch " + rocketSpeedBoostFromPitch);
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
        if(Character.Firework == CharacterSelect)
        rocketModel.SetActive(false);
    }

    public int GetCharacter()
    {
        return (int)CharacterSelect;
    }

    public float LookY()
    {
        if(CharacterSelect == Character.Firework && SpecialsLeft == 0)
        {
            return -1f;
        }
        else if (CharacterSelect == Character.Karate && SpecialsLeft == 0)
        {
            return 0;
        }
        else if(currentRB.velocity.y < 0)
        {
            return 1;
        }
        return 1f;
    }
}
