using System.Collections;
using System.Collections.Generic;
using UnityEngine.SceneManagement;
using UnityEngine;
using UnityEngine.AI;

public class playerController : MonoBehaviour
{

    public float sensitivityX;
    public float sensitivityY;
    private float minimumX = -360F;
    private float maximumX = 360F;
    private float minimumY = -85F;
    private float maximumY = 85F;
    private float rotationX = 0F;
    private float rotationY = 0F;
    private Quaternion originalRotation;

    Transform player;

    public GameObject cameraPrefab;
    GameObject cameraSpawned;

    public GameObject StartCar;

    public GameObject DropShadow;

    Transform mainCamera;
    bool grounded = true;
    float control = 0;
    Rigidbody rb;
    bool inCar = true;
    GameObject car;
    public AudioSource soundEffects;
    public AudioSource carSound;
    public AudioClip jumpSound;
    public AudioClip stompSound;
    public AudioClip doubleSound;
    public AudioClip killSound;

    float nextCarDelay = 1f;
    float nextCarTimer;
    public IPlayer thePlayer;

    // Use this for initialization
    void Start()
    {

        player = transform;
        cameraSpawned = Instantiate(cameraPrefab);
        cameraSpawned.transform.parent = player;

        cameraSpawned.transform.localPosition = cameraPrefab.transform.position;
        cameraSpawned.transform.localRotation = cameraPrefab.transform.rotation;

        mainCamera = GameObject.FindGameObjectWithTag("MainCamera").transform;
        rb = player.GetComponent<Rigidbody>();
        thePlayer = GetComponent<IPlayer>();

        originalRotation = transform.localRotation;

        EnterCar(StartCar.GetComponent<Collider>());
    }

    // Update is called once per frame
    void Update()
    {
        Keyboard_Input();
        Mouse_Input();
        nextCarTimer -= Time.deltaTime;

        if (!inCar)
        {
            carSound.Pause();
            DropShadow.SetActive(true) ;
        }
        else
        {
            transform.position = car.transform.position + car.GetComponent<Driving_Controls>().PlayerPositionInCar;
            carSound.UnPause();
            DropShadow.SetActive(false);
        }
    }

    private void Keyboard_Input()
    {
        if ((Input.GetKey(KeyCode.W) || Input.GetAxis("Vertical") > 0) && !inCar)
        {
            thePlayer.moveForward(rb, 1);
        }
        else if ((Input.GetKey(KeyCode.S) || Input.GetAxis("Vertical") < 0) && !inCar)
        {
            thePlayer.moveForward(rb, -1);
        }
        else
        {
            thePlayer.moveForward(rb, 0);
        }

        if ((Input.GetKey(KeyCode.D) || Input.GetAxis("Horizontal") > 0) && !inCar)
        {
            thePlayer.moveRight(rb, 1);
            if (thePlayer.IsRocketMode())
            {
                rotationX += 0.5f * sensitivityX;
            }
        }
        else if ((Input.GetKey(KeyCode.A) || Input.GetAxis("Horizontal") < 0) && !inCar)
        {
            thePlayer.moveRight(rb, -1);
            if (thePlayer.IsRocketMode())
            {
                rotationX -= 0.5f * sensitivityX;
            }
        }
        else
        {
            thePlayer.moveRight(rb, 0);
        }
        //exit vehicle
        if ((Input.GetKeyDown(KeyCode.Space) || Input.GetKeyDown(KeyCode.JoystickButton0)) && grounded)
        {
            //player.parent = null;
            if (rb == null)
            {
                rb = gameObject.AddComponent<Rigidbody>();
            }

            soundEffects.PlayOneShot(jumpSound);
            thePlayer.exitVehicle(rb, car);
            grounded = false;
            GetComponent<MeshRenderer>().enabled = true;
            car.gameObject.GetComponent<Driving_Controls>().PlayerInCar = false;
            inCar = false;
            nextCarTimer = nextCarDelay;
            player.transform.GetComponent<SphereCollider>().isTrigger = false;
        }
        //special
        else if ((Input.GetKeyDown(KeyCode.Space) || Input.GetKeyDown(KeyCode.JoystickButton0)) && !grounded)
        {
            thePlayer.useSpecial(rb);
            nextCarTimer = 0;
            // if player's special is double
            if(thePlayer.GetSpecialsLeft() > 0)
            soundEffects.PlayOneShot(doubleSound);
        }
    }

    private void Mouse_Input()
    {
        //Debug.Log(rotationX);
        //Debug.Log(rotationY);

        // Read the mouse input axis
        rotationX += (Input.GetAxis("Mouse X")) * sensitivityX;
        rotationY += (Input.GetAxis("Mouse Y")) * sensitivityY;

        rotationX = ClampAngle(rotationX, minimumX, maximumX);
        rotationY = ClampAngle(rotationY, minimumY, maximumY);

        Quaternion xQuaternion = Quaternion.AngleAxis(rotationX, Vector3.up);
        Quaternion yQuaternion = Quaternion.AngleAxis(rotationY, -Vector3.right);
        transform.localRotation = originalRotation * xQuaternion * yQuaternion;
    }

    public void resetRotation()
    {
        rotationX = 0f;
        rotationY = 0f;
    }

    public static float ClampAngle(float angle, float min, float max)
    {
        if (angle <= -360F)
            angle += 360F;
        if (angle >= 360F)
            angle -= 360F;
        return Mathf.Clamp(angle, min, max);
    }

    void OnCollisionEnter(Collision other)
    {
        if (other.gameObject.CompareTag("Kill Zone"))
        {
            Debug.Log("kill");
            soundEffects.PlayOneShot(killSound);
            SceneManager.LoadScene("MainMenu");
        }

        //NEW: Ends the level with a success. For prototype it simply restarts stage.
        if (other.gameObject.CompareTag("Goal"))
        {
            Debug.Log("goal");
            SceneManager.LoadScene("MainMenu");
        }
    }

    void OnTriggerStay(Collider other)
    {
        

        //if (other.gameObject.CompareTag("Car") && !inCar && nextCarTimer <= 0f)
        if (other.gameObject.CompareTag("Car") && !inCar && player.transform.GetComponent<Rigidbody>().velocity.y <0)
            {

            EnterCar(other);
            //grounded = true;
            //control = 0;
            //car = other.gameObject;
            //GetComponent<MeshRenderer>().enabled = false;
            //other.gameObject.GetComponent<Driving_Controls>().PlayerInCar = true;

            ////NEW: Adds a slight increase of initial speed when entering vehicles.
            ////other.gameObject.GetComponent<Driving_Controls>().speed = 15f;
            //inCar = true;
            //Destroy(player.GetComponent<Rigidbody>());

            ////player.parent = other.transform;
            ////player.transform.rotation = Quaternion.identity;
            //player.transform.GetComponent<SphereCollider>().isTrigger = true;


            //player.position = car.transform.position + (car.transform.up * 2) + (car.transform.forward * -1);
            //car.GetComponent<Driving_Controls>().speed = thePlayer.GetHorVelocityCheck().magnitude / 2;

            //car.GetComponent<NavMeshAgent>().enabled = false;
        }
    }

    void EnterCar(Collider other)
    {
        grounded = true;
        control = 0;
        car = other.gameObject;
        GetComponent<MeshRenderer>().enabled = false;
        other.gameObject.GetComponent<Driving_Controls>().PlayerInCar = true;

        //NEW: Adds a slight increase of initial speed when entering vehicles.
        //other.gameObject.GetComponent<Driving_Controls>().speed = 15f;
        inCar = true;
        Destroy(player.GetComponent<Rigidbody>());

        //player.parent = other.transform;
        //player.transform.rotation = Quaternion.identity;
        player.transform.GetComponent<SphereCollider>().isTrigger = true;

        //car speed boost from player moementum
        //Debug.Log("carboost " + thePlayer.GetHorVelocityCheck().magnitude / 2);
        car.GetComponent<Driving_Controls>().speed = thePlayer.GetHorVelocityCheck().magnitude / 2;

        car.GetComponent<NavMeshAgent>().enabled = false;
    }
}
