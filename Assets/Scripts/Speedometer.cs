using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Speedometer : MonoBehaviour
{
    private const float maxSpeedAngle = -200f;
    private const float zeroSpeedAngle = 16f;
    private Transform needleTransform;
    private float maxSpeed;
    private float speed;
    void Awake() {
        needleTransform = transform.Find("needle");
        speed = 0f;
        maxSpeed = 260f;
    }

    void Update() {
        speed = Mathf.Abs(CarController.Instance.velocityVsUp);
        if (speed > maxSpeed) speed = maxSpeed;

        needleTransform.eulerAngles = new Vector3(0, 0, GetSpeedRotation());
    }

    private float GetSpeedRotation() {
        float totalAngle = zeroSpeedAngle - maxSpeedAngle;
        float speedNorm = speed / maxSpeed;
        return zeroSpeedAngle - speedNorm * totalAngle;
    }
}
