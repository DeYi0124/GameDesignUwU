using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CarController : MonoBehaviour
{
    public static CarController Instance;
    [Header("Car Settings")]
    public float accelerate = 30.0f;
    public float turn = 3.5f;
    public float drift = 0.95f;
    public float maxSpeed = 260;
    public int bike = 0;
    public int maxBike = 20;
    public bool onGrass = false;
    public bool onBroken = false;
    public Animator carAnimator;
    float accInput = 0;
    float steerInput = 0;

    float rotatingAngle = 0;
    public float velocityVsUp = 0;
    Rigidbody2D carRigidbody2D;

    private LayerMask solidObjectsLayer;


    void Awake()
    {
        CurrentBike[] currs = FindObjectsOfType<CurrentBike>();
        foreach (CurrentBike curr in currs) {
            curr.OnSave += OnSave;
        }
        if (Instance != null && Instance != this) {
            Destroy(gameObject);
        }
        else
            Instance = this;
        carRigidbody2D = GetComponent<Rigidbody2D>();
        bike = 0;
        maxBike = 20;
        onGrass = false;
        onBroken = false;
        maxSpeed = 26;
        carAnimator.SetBool("isGlowing", false);
        // if(FindObjectsOfType<CarController>().Length > 1) {
        //     Debug.Log("DestroyinG" + FindObjectsOfType<CarController>().Length.ToString());
        //     this.gameObject.SetActive(false);
        //     Destroy(this.gameObject);
        // } else
        //     DontDestroyOnLoad(gameObject);
    }

    void OnSave(CurrentBike currBike) {
        //Debug.Log("UWUevent!");
    }

    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        if(GameManager.Instance.PR >= 100){
            GameObject.FindWithTag("Car").transform.GetChild(0).GetChild(5).gameObject.SetActive(true);
            carAnimator.SetBool("isGlowing", true);
        }
        else{
            GameObject.FindWithTag("Car").transform.GetChild(0).GetChild(5).gameObject.SetActive(false);
            carAnimator.SetBool("isGlowing", false);
        }
        //Debug.Log(transform.position);
        //Debug.Log(transform.rotation);
    }

    void FixedUpdate()
    {
        if(!GameManager.Instance.pause) {
            ApplyEngineForce();
            KillOrthogonalVelocity();
            ApplySteering();
        }
    }

    void ApplyEngineForce()
    {
        if(GameManager.Instance.pause) {
            return;
        }
        velocityVsUp = Vector2.Dot(carRigidbody2D.velocity, transform.right);
        if(velocityVsUp > maxSpeed && accInput > 0) {
            return;
        }
        if(velocityVsUp < -maxSpeed * 0.5f && accInput < 0) {
            return;
        }
        if(carRigidbody2D.velocity.sqrMagnitude > maxSpeed * maxSpeed && accInput > 0) {
            return;
        }
        if (accInput == 0) {
            carRigidbody2D.drag = Mathf.Lerp(carRigidbody2D.drag, 3.0f, Time.fixedDeltaTime*3);
        } else {
            carRigidbody2D.drag = 0;
        }
        Vector2 engineForceVector = transform.right*accInput*accelerate;
        carRigidbody2D.AddForce(engineForceVector, ForceMode2D.Force);
    }

    void ApplySteering()
    {
        if(GameManager.Instance.pause) {
            return;
        }
        float steeringMin = (carRigidbody2D.velocity.magnitude / 8);
        steeringMin = Mathf.Clamp01(steeringMin);
        rotatingAngle -= steerInput * turn * steeringMin;
        carRigidbody2D.MoveRotation(rotatingAngle);
    }

    void KillOrthogonalVelocity()
    {
        if(GameManager.Instance.pause) {
            return;
        }
        Vector2 fwdVelocity = transform.right * Vector2.Dot(carRigidbody2D.velocity, transform.right);
        Vector2 rightVelocity = (-transform.up) * Vector2.Dot(carRigidbody2D.velocity, (-transform.up));

        carRigidbody2D.velocity = fwdVelocity + rightVelocity*drift;
    }

    float GetLateralVelocity() {
        return Vector2.Dot(-transform.up, carRigidbody2D.velocity);
    }

    public float GetVelocityMagnitude() {
        return carRigidbody2D.velocity.magnitude;
    }

    public Vector2 GetPostion() {
        return carRigidbody2D.position;
    }

    public void SetPosition(Vector2 pos) {
        carRigidbody2D.velocity = Vector2.zero;
        //carRigidbody2D.MovePosition(pos);
    }

    public bool IsTireScreeching(out float lateralVelocity, out bool isBraking) {
        lateralVelocity = GetLateralVelocity();
        isBraking = false;

        if (accInput < 0 && velocityVsUp > 0) {
            isBraking = true;
            return true;
        }

        if (Mathf.Abs(GetLateralVelocity()) > 4.0f) {
            return true;
        }

        return false;
    }

    void OnTriggerEnter2D(Collider2D collider2D) {
        if(collider2D.CompareTag("Grass")) {
            carRigidbody2D.velocity *= 0.1f;
            onGrass = true;
        }
    }
    void OnTriggerExit2D(Collider2D collider2D) {
        if(collider2D.CompareTag("Grass")) {
            onGrass = false;
        }
    }
    public void SetInputVector(Vector2 inputVector) {
        steerInput = inputVector.x;
        accInput = inputVector.y;
        if(accInput < 0) {
            steerInput = -steerInput;
        }
        if(onGrass) {
            //Debug.Log("GRASS!");
            steerInput *= 0.3f;
            accInput *= 0.3f;
        }
        if(onBroken || GameManager.Instance.pause) {
            carRigidbody2D.drag = 0;
            steerInput *= 0;
            accInput *= 0;
        }
    }

    public void reset(){
        Vector3 spawnPoint = new Vector3(-27,-2,0);
        var carTransform= GameObject.FindWithTag("Car").GetComponent<Transform>();
        carTransform.rotation = Quaternion.Euler(new Vector3(0,0,0));
        carTransform.position = spawnPoint;
        carRigidbody2D.drag = 0;
        rotatingAngle = 0;

    }
    public void resetMomentum(){

        var carTransform= GameObject.FindWithTag("Car").GetComponent<Transform>();
        carRigidbody2D.drag = 0;
    }
}
