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
    public GameObject PlayerMesh;
    public GameObject RagdollPrefab;
    public Rigidbody RagdollPelvis;
    GameObject car;
    public AudioSource soundEffects;
    public AudioSource carSound;
    public AudioSource music1;
    public AudioSource music2;
    public AudioSource music3;
    public AudioSource windSound;
    public AudioClip jumpSound;
    public AudioClip stompSound;
    public AudioClip doubleSound;
    public AudioClip killSound;

    float nextCarDelay = 1f;
    float nextCarTimer;
    public IPlayer thePlayer;

    private bool IsKilled;

    float deathcammultiplier;

    // Use this for initialization
    void Start()
    {
        deathcammultiplier = 0f;
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

        switch (thePlayer.GetCharacter())
        {
            case 0:
                music1.Play();
                break;
            case 1:
                music2.Play();
                break;
            case 2:
                music3.Play();
                break;
            default:
                break;
        }
    }

    // Update is called once per frame
    void Update()
    {
        Keyboard_Input();
        Mouse_Input();
        UpdateAudio();
        nextCarTimer -= Time.deltaTime;

        if (!inCar && !IsKilled)
        {
            carSound.Pause();
            DropShadow.SetActive(true);
        }
        else
        {
            transform.position = car.transform.position + car.GetComponent<Driving_Controls>().PlayerPositionInCar;
            carSound.UnPause();
            DropShadow.SetActive(false);
            if (IsKilled)
            {
                Debug.Log("lloking");
                cameraSpawned.transform.LookAt(RagdollPelvis.transform);
                deathcammultiplier += Time.deltaTime;
                cameraSpawned.transform.position += ((RagdollPelvis.transform.position + Vector3.up * deathcammultiplier) - cameraSpawned.transform.position).normalized * ((1f * ((RagdollPelvis.transform.position + Vector3.up * deathcammultiplier) - cameraSpawned.transform.position).magnitude) - 2f) * deathcammultiplier * Time.deltaTime ;
            }
        }
        
    }

    private void Keyboard_Input()
    {
        if ((Input.GetKey(KeyCode.W) || Input.GetAxis("Vertical") > 0.1) && !inCar)
        {
            thePlayer.moveForward(rb, 1);
        }
        else if ((Input.GetKey(KeyCode.S) || Input.GetAxis("Vertical") < -0.1) && !inCar)
        {
            thePlayer.moveForward(rb, -1);
        }
        else
        {
            //Debug.Log("zero f");
            thePlayer.moveForward(rb, 0);
        }

        if ((Input.GetKey(KeyCode.D) || Input.GetAxis("Horizontal") > 0.1) && !inCar)
        {
            thePlayer.moveRight(rb, 1);
            if (thePlayer.IsRocketMode())
            {
                rotationX += 0.5f * sensitivityX;
            }
        }
        else if ((Input.GetKey(KeyCode.A) || Input.GetAxis("Horizontal") < -0.1) && !inCar)
        {
            thePlayer.moveRight(rb, -1);
            if (thePlayer.IsRocketMode())
            {
                rotationX -= 0.5f * sensitivityX;
            }
        }
        else
        {
            //Debug.Log("zero h");
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
            //GetComponent<MeshRenderer>().enabled = true;
            PlayerMesh.SetActive(true);
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

    private void UpdateAudio()
    {
        if (!inCar)
        {
            carSound.Pause();
            music1.volume = .3f;
            music2.volume = .3f;
            music3.volume = .3f;
            windSound.UnPause();
            if (rb != null)
            {
                windSound.volume = rb.velocity.magnitude / 200;
                //print(rb.velocity.magnitude);
            }
        }
        else
        {
            transform.position = car.transform.position + car.transform.up * 1f;
            carSound.UnPause();
            music1.volume = 1f;
            music2.volume = 1f;
            music3.volume = 1f;
            windSound.Pause();

        }
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
    
    IEnumerator WaitToRagdoll(Vector3 impactVelocity)
    {
        PlayerMesh.SetActive(false);
        DropShadow.SetActive(false);
        RagdollPrefab.SetActive(true);
        RagdollPrefab.transform.parent = null;
        Debug.Log(impactVelocity);
        RagdollPelvis.velocity = (impactVelocity + (Vector3.up * impactVelocity.magnitude / 4)) * 4f;
        cameraSpawned.transform.parent = null;
        
        yield return new WaitForSeconds(6f);
        SceneManager.LoadScene("MainMenu");
    }
    
    void OnCollisionEnter(Collision other)
    {
        if (other.gameObject.CompareTag("Kill Zone"))
        {

            IsKilled = true;
            soundEffects.PlayOneShot(killSound);
            StartCoroutine(WaitToRagdoll(player.GetComponent<Rigidbody>().velocity));
            
        }

        //NEW: Ends the level with a success. For prototype it simply restarts stage.
        if (other.gameObject.CompareTag("Goal"))
        {
            Debug.Log("goal");
            StartCoroutine(WaitToRagdoll(player.GetComponent<Rigidbody>().velocity));
        }
    }

    void OnTriggerStay(Collider other)
    {
        

        //if (other.gameObject.CompareTag("Car") && !inCar && nextCarTimer <= 0f)
        if (other.gameObject.CompareTag("Car") && !inCar && player.transform.GetComponent<Rigidbody>().velocity.y <0)
        {
            EnterCar(other);
        }
    }

    void EnterCar(Collider other)
    {
        grounded = true;
        control = 0;
        car = other.gameObject;
        GetComponent<MeshRenderer>().enabled = false;
        PlayerMesh.SetActive(false);
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
        car.GetComponent<Driving_Controls>().maxSpeed = Mathf.Max(car.GetComponent<Driving_Controls>().maxSpeed, car.GetComponent<Driving_Controls>().speed + 15);

        car.GetComponent<NavMeshAgent>().enabled = false;

        car.transform.rotation = player.transform.rotation;
        car.transform.eulerAngles = new Vector3(car.transform.eulerAngles.x * 0, car.transform.eulerAngles.y, car.transform.eulerAngles.z);

        thePlayer.EnterVehicleCleanUp();
    }


}
