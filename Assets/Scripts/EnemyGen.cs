using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;
using UnityEngine.SceneManagement;

public class EnemyGen : MonoBehaviour
{
    public GameObject cp;
    public GameObject bp;
    public GameObject[] ePts;
    public int rng;

    private int time = 0;
    private int allBike = 0;
    private int currCnt = 0;
    private bool[] alreadyFilled;


    public void genBike() {
        int tmpBike = Random.Range(0, allBike);
        //Debug.Log(tmpBike);
        if(time % 2 == 0 && !alreadyFilled[tmpBike] && !PauseMenu.isPaused && currCnt < GameManager.Instance.EnemyLimit) {
            int tmp = Random.Range(0, 2);
            GameObject tmpgobj;
            if(tmp == 0) {
                tmpgobj = Instantiate(cp, ePts[tmpBike].transform.position, Quaternion.identity, transform);
            }else {
                tmpgobj = Instantiate(bp, ePts[tmpBike].transform.position, Quaternion.identity, transform);
            }
            time = 1;
            currCnt++;
            alreadyFilled[tmpBike] = true;
        } else {
            time ++;
        }
    }
    void Awake() {
        InvokeRepeating("genBike", 1f, 1f);
        allBike = ePts.Length;
        alreadyFilled = new bool[allBike];
        for(int i = 0; i < allBike; i++) {
            alreadyFilled[i] = false;
        }
    }
}
