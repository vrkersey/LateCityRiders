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

        if (!cTransform)
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

            //normal movement

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

    public void moveForward(Rigidbody rb, float value)
    {
        ForceToAdd = new Vector3(ForceToAdd.x, ForceToAdd.y, ForceToAdd.z) + calculateForward() * value;
    }

    public void moveRight(Rigidbody rb, float value)
    {
        ForceToAdd = new Vector3(ForceToAdd.x, ForceToAdd.y, ForceToAdd.z) + Vector3.Cross(Vector3.up, calculateForward()) * value;
    }

    public void exitVehicle(Rigidbody rb, GameObject car)
    {
        currentRB = rb;
        //Rigidbody carRB = car.GetComponent<Rigidbody>();
        Driving_Controls CarControl = car.GetComponent<Driving_Controls>();
        Debug.Log(CarControl.speed);
        float CarSpeed = Mathf.Abs(CarControl.speed);
        float SpeedBoost = (CarSpeed >= CarControl.maxSpeed * 0.8f ? CarSpeed * 1.2f : CarSpeed * 0.8f);

        //add jump
        jumpAdd = new Vector3(0f, ((CarSpeed / 4) * SpeedBoost), 0f);
        jumpholdtimer = jumpholdlimit;

        //add velocity
        rb.velocity += new Vector3(car.transform.GetComponent<Rigidbody>().velocity.x, 0, car.transform.GetComponent<Rigidbody>().velocity.z) * CarSpeed * SpeedBoost;
        //reset ammo
        SpecialsLeft = CharacterSpecialAmmo;

        //reset specials
        CharAnim.SetBool("Is_Special", false);
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
