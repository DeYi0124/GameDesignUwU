using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using System.Linq;
using System;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
public class test : MonoBehaviour
{
    public TMP_InputField realCodeText;
    public TextMeshProUGUI fakeText;
    private string submission;
    
    
    public void readAnswer(string ans){
        submission = ans;
    }
    void Update(){
        fakeText.text = submission;
        if(fakeText.text != null){
            if(fakeText.text.Contains("cock")){
                fakeText.text = fakeText.text.Replace("cock","<color=red>cock</color>");
                Debug.Log(fakeText.text);
            }
        }

    }
}
