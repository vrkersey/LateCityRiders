using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Character_Business : basicPlayer
{
    //businessman
    public GameObject briefcasetospawn;

    public override void exitVehicle(Rigidbody rb, GameObject car, float holdTime)
    {
        //reset ammo
        SpecialsLeft = CharacterSpecialAmmo;

        //reset specials
        CharAnim.SetBool("Is_Special", false);
        base.exitVehicle(rb, car, holdTime);
    }

    public override void useSpecial(Rigidbody rb)
    {

        CharAnim.SetBool("Is_Special", true);
        if (SpecialsLeft > 0)
        {
            GameObject b = Instantiate(briefcasetospawn, transform.position - Vector3.up * 1f, CharAnim.transform.rotation);

            CharAnim.SetTrigger("Special_Buisness");
            SpecialsLeft -= 1;
            rb.velocity = new Vector3(1 * rb.velocity.x / 4, 0, 1 * rb.velocity.z / 4);
            rb.AddForce(Vector3.up * 500f);
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
