using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class minimap : MonoBehaviour
{
    public Transform car;

    void LateUpdate(){
        Vector3 newPosition = car.position;
        newPosition.z = transform.position.z;
        transform.position = newPosition;
    }



}
