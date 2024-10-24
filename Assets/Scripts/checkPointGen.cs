using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class checkPointGen : MonoBehaviour
{
    public GameObject myPrefab;
    public GameObject[] bikePts;

    private int time = 0;
    private int allBike = 0;
    private bool[] alreadyFilled;

    public void genBike() {
        int tmpBike = Random.Range(0, allBike);
        //Debug.Log(tmpBike);
        if(time % 5 == 0 && !alreadyFilled[tmpBike]) {
            GameObject tmpgobj = Instantiate(myPrefab, bikePts[tmpBike].transform.position, Quaternion.identity, transform);
            checkPointHandler tmpcph = tmpgobj.GetComponent<checkPointHandler>();
            tmpcph.id = tmpBike;
            tmpcph.OnReceiving += OnReceiving;
            time = 1;
            alreadyFilled[tmpBike] = true;
        } else {
            time ++;
        }
    }
    void Awake() {
        InvokeRepeating("genBike", 1f, 1f);
        allBike = bikePts.Length;
        alreadyFilled = new bool[allBike];
        for(int i = 0; i < allBike; i++) {
            alreadyFilled[i] = false;
        }
    }

    void OnReceiving(checkPointHandler cph) {
        //Debug.Log(cph.id);
        alreadyFilled[cph.id] = false;
    }
}
