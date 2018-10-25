using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Character_Business : basicPlayer
{
    //businessman only variables
    public GameObject briefcasetospawn;

    // Business character exitVehicle is the same as basicPlayer 
    // except we need to do stuff with the specials count and the animation
    public override void exitVehicle(Rigidbody rb, GameObject car)
    {
        // calls basicPlayer.exitVehicle passing in the same parameters given
        base.exitVehicle(rb, car);

        //reset ammo
        SpecialsLeft = CharacterSpecialAmmo;
        //reset specials
        CharAnim.SetBool("Is_Special", false);
    }

    // Business character useSpecial completely overrides the basicPlayer.useSpecial
    // function so we do what we need without calling back to base
    public override void useSpecial(Rigidbody rb)
    {
        Debug.Log("USING BUSINESS CHARACTER!!!");

        CharAnim.SetBool("Is_Special", true);
        if (SpecialsLeft > 0)
        {
            //spawn a briefcase
            GameObject b = Instantiate(briefcasetospawn, transform.position - Vector3.up * 1f, CharAnim.transform.rotation);

            CharAnim.SetTrigger("Special_Buisness");
            SpecialsLeft -= 1;

            //jump
            rb.velocity = new Vector3(1 * rb.velocity.x / 4, 0, 1 * rb.velocity.z / 4);
            rb.AddForce(Vector3.up * 500f);

            //modify a basicPlayer variable
            base.maxSpeedThisJump *= .9f;
        }
    }


    public override Vector3 GetHorVelocityCheck()
    {
        return HorVelocityCheck * 1.5f;
    }

    public override int GetCharacter()
    {
        return 1;
    }
}
