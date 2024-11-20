using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class minimap : MonoBehaviour
{
    public Transform car;

    void LateUpdate(){
        if(GameManager.Instance.pause) return;
        var car = GameObject.FindWithTag("Car").GetComponent<Transform>().position;
        Vector3 newPosition = car;
        newPosition.z = transform.position.z;
        transform.position = newPosition;
    }



}
