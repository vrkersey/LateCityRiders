using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class basicPlayer : MonoBehaviour {


    public Transform cTransform;

    //end character variables
    public Animator CharAnim;

    public float characterAcceleration = 20f;
    protected float maxSpeedThisJump;

    protected int SpecialsLeft = 0;
    public int CharacterSpecialAmmo = 1;

    protected Vector3 ForceToAdd;
    [HideInInspector]
    public Vector3 HorVelocityCheck;

    public float SpeedBoost = 20;
    
    protected Rigidbody currentRB;

    protected float jumpholdtimer;
    protected float jumpholdlimit = 0.25f;
    protected Vector3 jumpAdd;
    protected Vector3 jumpAddTo;

    void Start()
    {

    }

    void Update()
    {
        if (!cTransform)
        {
            cTransform = GameObject.FindGameObjectWithTag("MainCamera").transform;
        }

        if (currentRB)
        {

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

    public void SetcTransform(Transform cam)
    {
        cTransform = cam;
    }

    public virtual void moveForward(Rigidbody rb, float value)
    {
        if (currentRB)
        {
            currentRB.AddForce(calculateForward() * value * characterAcceleration);
        }
    }

    public virtual void moveRight(Rigidbody rb, float value)
    {
        if (currentRB)
        {
            currentRB.AddForce(Vector3.Cross(Vector3.up, calculateForward()) * value * characterAcceleration);
        }
    }

    public virtual void exitVehicle(Rigidbody rb, GameObject car, float holdTime)
    {
        currentRB = rb;
        Driving_Controls CarControl = car.GetComponent<Driving_Controls>();

        float CarSpeed = Mathf.Abs(CarControl.speed);
        float SpeedBoost = (CarSpeed > (CarControl.maxSpeed * 0.8f)) ? 1.2f : .5f;
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

    public virtual void useSpecial(Rigidbody rb)
    {
    }

    protected Vector3 calculateForward()
    {
        if (!cTransform)
        {
            cTransform = GameObject.FindGameObjectWithTag("MainCamera").transform;
        }
        Vector3 forward = (this.transform.position - cTransform.position);
        forward.y = 0;
        forward = forward.normalized;
        return forward;
    }

    public virtual Vector3 GetHorVelocityCheck()
    {
        return HorVelocityCheck;
    }

    public virtual bool IsRocketMode()
    {
        return false;
    }

    public int GetSpecialsLeft()
    {
        return SpecialsLeft;
    }

    public virtual void EnterVehicleCleanUp()
    {
    }

    public virtual int GetCharacter()
    {
        return 0;
    }

    public virtual float LookY()
    {
        return 1f;
    }

    public virtual void EnterVehicleAnimation()
    {

    }
}
