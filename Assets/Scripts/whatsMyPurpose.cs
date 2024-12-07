using System.Collections;
using System.Collections.Generic;
using System;
using UnityEngine;
using Random = UnityEngine.Random;
using UnityEngine.Events;
using UnityEngine.SceneManagement;
using System.IO;
using TMPro;
using UnityEngine.Networking;


public class whatsMyPurpose : MonoBehaviour
{
    public static whatsMyPurpose Instance;
    private string playerName = "";
    // Start is called before the first frame update
    void Awake(){
        if (Instance != null && Instance != this) {
            Destroy(gameObject);
        }
        else
            Instance = this;
        
        if(FindObjectsOfType<CarController>().Length > 1) {
            Debug.Log("DestroyinG" + FindObjectsOfType<CarController>().Length.ToString());
            this.gameObject.SetActive(false);
            Destroy(this.gameObject);
        } else
            DontDestroyOnLoad(gameObject);
    }
    public void readPlayerNameFromScript(){
        playerName = sayMyNameManager.playerName;
        Debug.Log(playerName);
    }
    public string getPlayerName(){
        return playerName;
    }



}

