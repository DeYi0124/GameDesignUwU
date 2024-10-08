using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class smokeHandler : MonoBehaviour
{
    float emissionRate = 0;

    CarController carController;
    ParticleSystem smoke;
    ParticleSystem.EmissionModule module;

    void Awake()
    {
        carController = GetComponentInParent<CarController>();
        smoke = GetComponent<ParticleSystem>();
        module = smoke.emission;
        module.rateOverTime = 0;
    }
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        emissionRate = Mathf.Lerp(emissionRate, 0, Time.deltaTime * 5);
        module.rateOverTime = emissionRate;

        if(carController.IsTireScreeching(out float lateralVelocity, out bool isBraking)) {
            if(isBraking) {
                emissionRate = 30;
            } else {
                emissionRate = Mathf.Abs(lateralVelocity) * 2;
            }
        }
    }
}
