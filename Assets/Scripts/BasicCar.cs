using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BasicCar : MonoBehaviour {
    public List<AxleInfo> axleInfos;
    public float MotorTorque = 5000;
    public float MaxSteeringAngle = 45;
    public float SteeringRate = 500;
    public float GroundedStablizationRate = 1000;

    private Rigidbody rb;

    private void Start()
    {
        // Needed to keep it from being all wobbly
        // Doing this for one wheel collider does it for them all
        axleInfos[0].leftWheel.GetComponent<WheelCollider>().ConfigureVehicleSubsteps(5, 12, 15);

        rb = GetComponent<Rigidbody>();
    }

    private void FixedUpdate()
    {
        // TODO : replace with call inputs from the player class
        float motor = MotorTorque * Input.GetAxis("Vertical");
        float steeringInput = Input.GetAxis("Horizontal");

        foreach (AxleInfo axle in axleInfos)
        {
            if(axle.motor)
            {
                axle.leftWheel.motorTorque = motor;
                axle.rightWheel.motorTorque = motor;
            }
            if(axle.steering)
            {
                float newSteering = Mathf.MoveTowards(axle.leftWheel.steerAngle, MaxSteeringAngle * steeringInput, SteeringRate * Time.deltaTime);
                axle.leftWheel.steerAngle = newSteering;
                axle.rightWheel.steerAngle = newSteering;
            }
        }
        Stablization();
    }

    // 
    private void Stablization()
    {
        bool wheelsOnGround = true;
        foreach (AxleInfo axle in axleInfos)
        {
            if(!(axle.leftWheel.isGrounded && axle.rightWheel.isGrounded))
            {
                wheelsOnGround = false;
                break;
            }
        }

        if(wheelsOnGround)
        {
            Vector3 force = rb.velocity.magnitude * GroundedStablizationRate * -1 * transform.up;
            rb.AddForce(force);
        }
    }
}


[System.Serializable]
public class AxleInfo
{
    public WheelCollider leftWheel;
    public WheelCollider rightWheel;
    public bool motor;
    public bool steering;
}
