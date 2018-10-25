using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Character_Karate : basicPlayer
{
    //karate
    public float magStorage;
    private float timeInAir = -1;

    void FixedUpdate()
    {
        //Increases time in air every interval.
        if (timeInAir > -1)
            timeInAir += Time.deltaTime;
    }

    public override void exitVehicle(Rigidbody rb, GameObject car)
    {
        // calls basicPlayer.exitVehicle passing in the same parameters given
        base.exitVehicle(rb, car);
        //reset ammo
        SpecialsLeft = CharacterSpecialAmmo;
        //reset specials
        CharAnim.SetBool("Is_Special", false);
    }

    public override void useSpecial(Rigidbody rb)
    {
        Debug.Log("USING KARATE");
        CharAnim.SetBool("Is_Special", true);
        if (SpecialsLeft > 0)
        {
            //Activate the karate ability, which is the divekick.
            CharAnim.SetTrigger("Special_Karate");
            SpecialsLeft -= 1;

            //Downward kick
            Vector3 dir = GetHorVelocityCheck().normalized;
            magStorage = rb.velocity.magnitude;
            rb.velocity = new Vector3(0 * rb.velocity.x / 2, -50f, 0 * rb.velocity.z / 2);
            rb.velocity += dir * 5f;

            //modify the basicPlayer variable
            base.maxSpeedThisJump *= .5f;
            timeInAir = 1f;
        }

    }

    public override Vector3 GetHorVelocityCheck()
    {
        if (SpecialsLeft == 0)
            return HorVelocityCheck.normalized * magStorage * 1.5f;
        return HorVelocityCheck;
    }

    public override int GetCharacter()
    {
        //I believe we return 2 here, since karate is the second of 3 characters.
        return 2;
    }
    //Does lookY need to be different for karate?
}
