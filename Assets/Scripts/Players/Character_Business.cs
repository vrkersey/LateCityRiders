using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Character_Business : MonoBehaviour //basicPlayer
{

//    public Transform cTransform;

//    ////characters
//    ////NEW: Added selected char, which uses playerprefs.
//    //public enum Character { BusinessMan = 0, Karate, Firework };
//    //public Character CharacterSelect;

//    //character variables

//    //businessman
//    public GameObject briefcasetospawn;

//    //end character variables
//    public Animator CharAnim;

//    public float characterAcceleration = 20f;
//    private float maxSpeedThisJump;

//    private int SpecialsLeft = 0;
//    public int CharacterSpecialAmmo = 1;

//    private Vector3 ForceToAdd;
//    [HideInInspector]
//    public Vector3 HorVelocityCheck;

//    public float SpeedBoost = 20;

//    //const float MaxSpeedForTest = 
//    Rigidbody currentRB;

//    float jumpholdtimer;
//    float jumpholdlimit = 0.25f;
//    Vector3 jumpAdd;
//    Vector3 jumpAddTo;

//    void Start()
//    {
//        //Debug.Log("try to get camera");
//        cTransform = GameObject.FindGameObjectWithTag("MainCamera").transform;
//        //CharacterSelect = (Character)PlayerPrefs.GetInt("Character");

//    }

//    public void SetcTransform(Transform cam)
//    {
//        cTransform = cam;
//    }

//    void Update()
//    {
//        //Debug.Log("maxspeedthisjump" + maxSpeedThisJump);

//        if (!cTransform)
//        {
//            cTransform = GameObject.FindGameObjectWithTag("MainCamera").transform;
//        }

//        if (currentRB)
//        {

//            if (jumpholdtimer > 0)
//            {
//                jumpholdtimer -= Time.deltaTime;
//                jumpAddTo += jumpAdd * Time.deltaTime / jumpholdlimit;
//                currentRB.velocity = new Vector3(currentRB.velocity.x, jumpAddTo.y, currentRB.velocity.z);
//            }
//            ForceToAdd = ForceToAdd.normalized;

//            //Debug.Log(currentRB.velocity.magnitude);
//            //Debug.Log(ForceToAdd);
//            currentRB.AddForce(ForceToAdd * characterAcceleration);
//            ForceToAdd = Vector3.zero;

//            //max speed check, and reduce horizontal velocity if needed;
//            HorVelocityCheck = new Vector3(currentRB.velocity.x, 0, currentRB.velocity.z);

//            if (HorVelocityCheck.magnitude > maxSpeedThisJump)
//            {
//                float saveY = currentRB.velocity.y;
//                HorVelocityCheck = HorVelocityCheck.normalized;
//                HorVelocityCheck *= maxSpeedThisJump;
//                currentRB.velocity = new Vector3(HorVelocityCheck.x, saveY, HorVelocityCheck.z);
//            }
//            //Animation code
//            float AnimatorSpeed;
//            AnimatorSpeed = HorVelocityCheck.magnitude;
//            CharAnim.SetFloat("Speed", AnimatorSpeed);
//        }
//    }
//    }

//    public void moveForward(Rigidbody rb, float value)
//    {
//        //ForceToAdd = new Vector3(-value, ForceToAdd.y, ForceToAdd.z);
//        ForceToAdd = new Vector3(ForceToAdd.x, ForceToAdd.y, ForceToAdd.z) + calculateForward() * value;
//    }

//    public void moveRight(Rigidbody rb, float value)
//    {

//        //ForceToAdd = new Vector3(ForceToAdd.x, ForceToAdd.y, value);
//        ForceToAdd = new Vector3(ForceToAdd.x, ForceToAdd.y, ForceToAdd.z) + Vector3.Cross(Vector3.up, calculateForward()) * value;

//        //Vector3 forward = calculateForward();
//        //Vector3 right = Vector3.Cross(Vector3.up, forward);
//        //rb.AddForce(right * value * 10f);
//    }

//    public void exitVehicle(Rigidbody rb, GameObject car)
//    {
//        Debug.Log("Exit");
//        currentRB = rb;
//        //Rigidbody carRB = car.GetComponent<Rigidbody>();
//        Driving_Controls CarControl = car.GetComponent<Driving_Controls>();

//        float CarSpeed = Mathf.Abs(CarControl.speed);
//        float SpeedBoost = (CarSpeed / (CarControl.maxSpeed * 0.8f));
//        maxSpeedThisJump = Mathf.Max(CarSpeed * SpeedBoost, 3f);
//        //float CarDirection = CarSpeed / CarControl.speed;


//        //add jump
//        jumpAdd = new Vector3(0f, ((CarSpeed / 4) * SpeedBoost), 0f);
//        jumpholdtimer = jumpholdlimit;
//        jumpAddTo = new Vector3(0, +5, 0);

//        //add velocity
//        rb.velocity += new Vector3(car.transform.GetComponent<Rigidbody>().velocity.x, 0, car.transform.GetComponent<Rigidbody>().velocity.z) * CarSpeed * SpeedBoost;
//        //reset ammo
//        SpecialsLeft = CharacterSpecialAmmo;

//        //reset specials
//        CharAnim.SetBool("Is_Special", false);
//    }

//    public void releaseJump(Rigidbody rb)
//    {
//        jumpholdtimer = 0f;

//    }

//    public void useSpecial(Rigidbody rb)
//    {

//        CharAnim.SetBool("Is_Special", true);
//            if (SpecialsLeft > 0)
//            {
//                GameObject b = Instantiate(briefcasetospawn, transform.position - Vector3.up * 1f, CharAnim.transform.rotation);
//                Debug.Log("CharAnim.transform.rotation " + CharAnim.transform.rotation);
//                //b.transform.LookAt(GetHorVelocityCheck());
//                //b.transform.rotation = transform.rotation;
//                //b.transform.eulerAngles = new Vector3(b.transform.eulerAngles.x, b.transform.eulerAngles.y + 90f, b.transform.eulerAngles.z);
//                //Debug.Log("brieflooki" + GetHorVelocityCheck());
//                Debug.Log("br2 " + b.transform.rotation);

//                //Debug.Log("use double jump");
//                CharAnim.SetTrigger("Special_Buisness");
//                SpecialsLeft -= 1;
//                rb.velocity = new Vector3(1 * rb.velocity.x / 4, 0, 1 * rb.velocity.z / 4);
//                jumpAdd = new Vector3(0, Mathf.Max(rb.velocity.y, 0) + 5, 0);
//                jumpholdtimer = jumpholdlimit;
//                jumpAddTo = new Vector3(0, +5f, 0);
//                maxSpeedThisJump *= .9f;


//            }
        
//    }

//    private Vector3 calculateForward()
//    {
//        if (!cTransform)
//        {
//            cTransform = GameObject.FindGameObjectWithTag("MainCamera").transform;
//        }
//        Vector3 forward = (this.transform.position - cTransform.position);
//        forward.y = 0;
//        forward = forward.normalized;
//        //Debug.Log("forward " + forward);
//        return forward;
//    }

//    public Vector3 GetHorVelocityCheck()
//    {
//        if (CharacterSelect == Character.BusinessMan)
//            return HorVelocityCheck * 1.5f;
//        return HorVelocityCheck;
//    }

//    public bool IsRocketMode()
//    {
//        return false;
//    }

//    public int GetSpecialsLeft()
//    {
//        return SpecialsLeft;
//    }

//    public void EnterVehicleCleanUp()
//    {
//    }

//    public int GetCharacter()
//    {
//        return (int)CharacterSelect;
//    }

//    public float LookY()
//    {
//        if (currentRB.velocity.y < 0)
//        {
//            return 1;
//        }
//        return 1f;
//    }
}
