using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class basicPlayer : MonoBehaviour {


    public Transform cTransform;

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
        //cTransform = GameObject.FindGameObjectWithTag("MainCamera").transform;
        //CharacterSelect = (Character)PlayerPrefs.GetInt("Character");

    }

    public void SetcTransform(Transform cam)
    {
        cTransform = cam;
    }

    void Update()
    {
        //Debug.Log("maxspeedthisjump" + maxSpeedThisJump);

        if (!cTransform)
        {
            cTransform = GameObject.FindGameObjectWithTag("MainCamera").transform;
        }

        if (currentRB)
        {

            //if (jumpholdtimer > 0)
            //{
            //    jumpholdtimer -= Time.deltaTime;
            //    jumpAddTo += jumpAdd * Time.deltaTime / jumpholdlimit;
            //    currentRB.velocity = new Vector3(currentRB.velocity.x, jumpAddTo.y, currentRB.velocity.z);
            //}

            ////max speed check, and reduce horizontal velocity if needed;
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

    public void moveForward(Rigidbody rb, float value)
    {
        if (currentRB)
        {
            currentRB.AddForce(calculateForward() * value * characterAcceleration);
        }
    }

    public void moveRight(Rigidbody rb, float value)
    {
        if (currentRB)
        {
            currentRB.AddForce(Vector3.Cross(Vector3.up, calculateForward()) * value * characterAcceleration);
        }
    }

    public void exitVehicle(Rigidbody rb, GameObject car, float holdTime)
    {
        currentRB = rb;
        Driving_Controls CarControl = car.GetComponent<Driving_Controls>();

        float CarSpeed = Mathf.Abs(CarControl.speed);
        float SpeedBoost = (CarSpeed > (CarControl.maxSpeed * 0.8f)) ? 1.2f : .3f;
        maxSpeedThisJump = Mathf.Max(CarSpeed * SpeedBoost, 3f);
        float holdMult = Mathf.Max(holdTime / 1f, .5f);

        Vector3 carVeloctiy = car.transform.GetComponent<Rigidbody>().velocity;
        carVeloctiy.y = 0f;
        currentRB.velocity = carVeloctiy * SpeedBoost;
        currentRB.AddForce(Vector3.up * 1000f * holdMult);
    }

    public void releaseJump(Rigidbody rb)
    {
        jumpholdtimer = 0f;
    }

    public void useSpecial(Rigidbody rb)
    {
        CharAnim.SetBool("Is_Special", true);
    }

    private Vector3 calculateForward()
    {
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
        return HorVelocityCheck;
    }

    public bool IsRocketMode()
    {
        return false;
    }

    public int GetSpecialsLeft()
    {
        return SpecialsLeft;
    }

    public void EnterVehicleCleanUp()
    {
    }

    public int GetCharacter()
    {
        return 0;
    }

    public float LookY()
    {
        return 1f;
    }
}
