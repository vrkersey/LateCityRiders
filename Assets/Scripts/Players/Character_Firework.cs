using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Character_Firework : basicPlayer
{
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

    void FixedUpdate()
    {
        //rocket movement
        if (rocketTimer > 0)
        {
            rocketModel.SetActive(true);
            rocketTimer -= Time.deltaTime;
            currentRB.velocity = new Vector3(currentRB.velocity.x, 0, currentRB.velocity.z);
            currentRB.velocity = Vector3.RotateTowards(currentRB.velocity, calculateForward(), 1.57f * Time.deltaTime, 0f);

            currentRB.velocity = currentRB.velocity.normalized * (rocketSpeed + rocketSpeedBoostFromPitch);
            if (rocketPitch > 0)
            {
                rocketgravity += Time.deltaTime * (rocketPitch) / 1;
            }
            if (rocketPitch <= 0)
            {
                //rocketgravity += Time.deltaTime * (rocketPitch / .1f);
            }
            rocketgravity -= Time.deltaTime * 10f;
            if (rocketgravity <= 0)
            {
                rocketgravity = 0;
            }
            currentRB.velocity = new Vector3(currentRB.velocity.x / (rocketgravity * 0.1f + 1f), rocketPitch, currentRB.velocity.z / (rocketgravity * 0.1f + 1f));
            transform.position -= Vector3.up * rocketgravity * Time.deltaTime;

            newMaxSpeed = Mathf.Max(rocketSpeed + rocketSpeedBoostFromPitch, newMaxSpeed);
            if (rocketSpeedBoostFromPitch < 0)
            {
                rocketSpeed += Time.deltaTime * (-rocketSpeedBoostFromPitch);
                if (rocketSpeed > newMaxSpeed)
                {
                    rocketSpeed = newMaxSpeed;
                }
            }
            else
            {
                rocketSpeed -= Time.deltaTime * 0.5f;
            }
            maxSpeedThisJump = rocketSpeed;
            if (rocketTimer <= 0)
            {
                currentRB.velocity = new Vector3(currentRB.velocity.x, rocketPitch - rocketgravity, currentRB.velocity.z);
            }
        }
        //normal movement
        else
        {
            rocketModel.SetActive(false);
        }
    }

    public override void moveForward(Rigidbody rb, float value)
    {
        //firework
        if (rocketTimer > 0)
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

            if (rocketPitch > 0)
            {
                rocketSpeedBoostFromPitch -= (rocketPitch - rocketgravity) / 1f * (Mathf.Abs(rocketSpeed / 50)) * Time.deltaTime;
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

    public override void exitVehicle(Rigidbody rb, GameObject car)
    {
        base.exitVehicle(rb, car);
        //reset ammo
        SpecialsLeft = CharacterSpecialAmmo;
        CharAnim.SetBool("Is_Special", false);

        //reset specials
        rocketTimer = -1f;
        rocketgravity = 0;
        rocketSpeed = 0;
    }

    public override void releaseJump(Rigidbody rb)
    {
        base.releaseJump(rb);
        if (rocketTimer > 0)
        {
            currentRB.velocity = new Vector3(currentRB.velocity.x, rocketPitch - rocketgravity, currentRB.velocity.z);
            rocketTimer = 0f;
        }

    }

    public override void useSpecial(Rigidbody rb)
    {
        Debug.Log("USING FIREWORKS");
        CharAnim.SetBool("Is_Special", true);
        if (SpecialsLeft > 0)
        {
            SpecialsLeft -= 1;
            maxSpeedThisJump *= 1.2f;
            rocketTimer = rocketTimeSet;
            rocketPitch = 0f;
            rocketSpeedBoostFromPitch = 0f;
            rocketSpeed = maxSpeedThisJump;
        }
    }

    public override Vector3 GetHorVelocityCheck()
    {
        if (SpecialsLeft == 0)
            return HorVelocityCheck + HorVelocityCheck.normalized * rocketSpeedBoostFromPitch * 2;
        return HorVelocityCheck;
    }

    public override bool IsRocketMode()
    {
        if (rocketTimer > 0)
            return true;
        return false;

    }

    public override void EnterVehicleCleanUp()
    {
        rocketModel.SetActive(false);
    }

    public override int GetCharacter()
    {
        //Does 3 represent fireworks?
        return 3;
    }

    //Does lookY need to be different for fireworks?
}
