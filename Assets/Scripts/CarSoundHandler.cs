using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CarSoundHandler : MonoBehaviour
{
    [Header("Audio Sources")]
    public AudioSource tireScreech;
    public AudioSource engine;
    public AudioSource hit;
    public AudioSource horn;

    CarController carController;
    float pitch = 0.5f;
    float tireScreechPitch = 0.5f;
    void Awake()
    {
        carController = GetComponentInParent<CarController>();
    }

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        UpdateEngineSFX();
        UpdateScreechSFX();
        if(GameManager.Instance.pause) return;
        if (Input.GetKey("space"))
        {
            //Debug.Log("space key was pressed");
            horn.volume = 4.5f;
            if(!horn.isPlaying) {
                horn.Play();
            }
        } else {
            horn.volume = 0;
        }
    }

    void UpdateEngineSFX()
    {
        float velocityMag = carController.GetVelocityMagnitude();
        float volume = velocityMag * 0.05f;
        volume = Mathf.Clamp(volume, 0.2f, 1.0f);
        engine.volume = Mathf.Lerp(engine.volume, volume, Time.deltaTime * 10);
        pitch = velocityMag * 0.2f;
        pitch = Mathf.Clamp(pitch, 0.5f, 2f);
        engine.pitch = Mathf.Lerp(engine.pitch, pitch, Time.deltaTime * 1.5f);
    }

    void UpdateScreechSFX()
    {
        if(carController.IsTireScreeching(out float lateralVelocity, out bool isBraking)) {
            if(isBraking) {
                tireScreech.volume = Mathf.Lerp(tireScreech.volume, 1.0f, Time.deltaTime * 10);
                tireScreechPitch = Mathf.Lerp(tireScreechPitch, 0.5f, Time.deltaTime * 10);
            } else {
                tireScreech.volume = Mathf.Abs(lateralVelocity) * 0.05f;
                tireScreechPitch = Mathf.Abs(lateralVelocity) * 0.1f;
            }
        }
        else {
            tireScreech.volume = Mathf.Lerp(tireScreech.volume, 0, Time.deltaTime * 10);
        }
    }

    void OnCollisionEnter2D(Collision2D collision2D) {
        if(collision2D.gameObject.tag != "building") return;
        GameManager.Instance.oil -= 10;
        float relativeVelocity = collision2D.relativeVelocity.magnitude;
        float volume = relativeVelocity * 0.1f;
        hit.pitch = Random.Range(0.95f, 1.05f);
        hit.volume = volume;
        if(!hit.isPlaying) {
            hit.Play();
        }
    }
}
