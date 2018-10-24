using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class basicPlayer : MonoBehaviour {

    public Animator CharAnim;
    public float characterAcceleration = 20f;
    public float SpeedBoost = 20;

    protected Transform cTransform;
    protected float maxSpeedThisJump;
    protected Rigidbody currentRB;
    protected bool jumphold;
    protected Vector3 HorVelocityCheck;

    // basic player doesn't use these variables but all characters will use them so storing them here
    public int CharacterSpecialAmmo = 1;
    protected int SpecialsLeft = 0;

    void Start()
    {

    }

    void FixedUpdate()
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

    public virtual void exitVehicle(Rigidbody rb, GameObject car)
    {
        currentRB = rb;
        Driving_Controls CarControl = car.GetComponent<Driving_Controls>();

        float CarSpeed = Mathf.Abs(CarControl.speed);
        float SpeedBoost = (CarSpeed > (CarControl.maxSpeed * 0.8f)) ? 1.2f : .5f;
        maxSpeedThisJump = Mathf.Max(CarSpeed * SpeedBoost, 3f);

        Vector3 carVeloctiy = car.transform.GetComponent<Rigidbody>().velocity;
        carVeloctiy.y = 0f;

        //use car velocity to calculate player velocity
        currentRB.velocity = carVeloctiy * SpeedBoost;
        //jump force
        currentRB.AddForce(Vector3.up * 500f);

        //the player can jump higher if they hold jump while leaving the car
        jumphold = true;
        StartCoroutine(jumpHoldForce());
    }

    public virtual void releaseJump(Rigidbody rb)
    {
        jumphold = false;
    }

    //function meant to be overridden by all child character scripts
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

    // coroutine to continue adding an upward force the longer the player holds space after leaving car
    protected IEnumerator jumpHoldForce()
    {
        float jumpForce = 80f;
        if (jumphold && currentRB != null && jumpForce > 1f)
        {
            jumpForce = Mathf.Lerp(jumpForce, 0, .1f);
            currentRB.AddForce(Vector3.up * jumpForce);
            yield return null;
        }
    }
}
