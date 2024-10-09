using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class savePoint : MonoBehaviour
{
    void Awake()
    {
        CurrentBike[] currs = FindObjectsOfType<CurrentBike>();
        foreach (CurrentBike curr in currs) {
            curr.OnSave += OnSave;
        }
    }

    void OnSave(CurrentBike currBike) {
        //Debug.Log("UWUevent!");
    }

    void Update()
    {

    }
}
