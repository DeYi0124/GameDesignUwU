using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;
using UnityEngine.SceneManagement;

public class checkPointGen : MonoBehaviour
{
    public GameObject myPrefab;
    public GameObject[] bikePts;
    public static int rng;

    private int time = 0;
    private int allBike = 0;
    private int rngUpperLimit = 0;
    private bool[] alreadyFilled;


    public void genBike() {
        int tmpBike = Random.Range(0, allBike);
        //Debug.Log(tmpBike);
        if(time % 2 == 0 && !alreadyFilled[tmpBike] && !PauseMenu.isPaused && !GameManager.Instance.pause) {
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
        rngUpperLimit = 10;
        //Debug.Log(cph.id);
        alreadyFilled[cph.id] = false;
        rng = Random.Range(1, rngUpperLimit+1);
        Debug.Log(rng);
        rng = 9;
        if(rng <= 9) {
            GameManager.Instance.pause = true;
            GameManager.carPosition = GameObject.FindWithTag("Car").GetComponent<Transform>().position;
            SceneManager.LoadSceneAsync("DialogueTemplate");
            if(rng == 1) {
                CarController.Instance.bike += 3;
            }
            else if(rng == 4) {
                CarController.Instance.bike -= 1;
            }else if(rng == 6) {
                CarController.Instance.bike = 0;
            }else if(rng == 7) {
                GameManager.Instance.coins = 0;
            }
        }
    }

    
}
