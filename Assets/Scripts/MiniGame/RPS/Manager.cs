using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using TMPro;

public class Manager : MonoBehaviour
{
    public GameObject exitButton;

    public TextMeshProUGUI Result;
    public TextMeshProUGUI Exit;
    public Image AIchoice;
    private Transform needleTransform;

    private float pinAngle = 0;
    private float delta = 50f;
    private float stoppingPoint = 0;
    private bool played = false;
    private bool matched = false;
    private int playerId = 0;
    void Awake() {
        exitButton.SetActive(false);
        needleTransform = transform.Find("pin");
        delta = 0f;
        pinAngle = 0f;
        stoppingPoint = 0f;
        played = false;
        matched = false;
        Exit.text = "";
    }
    void Update() {
        
        if(played && delta == 0) {
            UpdateResult();
            return;
        }
        //Debug.Log("delta: " + delta.ToString());
        needleTransform.eulerAngles = new Vector3(0, 0, pinAngle);
        pinAngle += delta;
        if(pinAngle >= 360) {
            pinAngle -= 360;
        }
        if(Mathf.Abs(pinAngle - stoppingPoint) <= delta) {
            stoppingPoint = Random.Range(0, 14)*24;
            delta -= 0.2f;
            if(delta < 0.01) delta = 0;
            //Debug.Log(delta);
        }
        
    }
    public void Play(int id) {
        playerId = id;
        Button []btns = this.GetComponentsInChildren<Button>();
        Image []imgs = this.GetComponentsInChildren<Image>();
        imgs[id].color = new Color32(34, 234, 69, 100);
        
        for (int i = 0; i < 15; i++) {
            // Debug.Log("????" + i.ToString());
            btns[i].enabled = false;
            // if(i == id) {
            //     //Debug.Log(i);
            //     continue;
            // }
            //btns[i].interactable = false;
            
        }
        delta = Random.Range(10, 30);
        played = true;
        pinAngle = Random.Range(0, 359);
        stoppingPoint = Random.Range(0, 14)*24;
    }
    void UpdateResult() {
        if(matched) return;
        int currAngle = Mathf.RoundToInt(pinAngle);
        currAngle += 4;
        int oppoId = currAngle / 24;
        oppoId = (15 - oppoId)%15;
        //Debug.Log(oppoId);
        Button []btns = this.GetComponentsInChildren<Button>();
        Image []imgs = this.GetComponentsInChildren<Image>();
        //btns[oppoId].interactable = true;
        imgs[oppoId].color = new Color32(90, 4, 1, 150);
        Exit.text = "Exit";
        exitButton.SetActive(true);

        //equal
        if(oppoId == playerId) {
            Result.text = "TIE";
            return;
        }
        //greater
        for(int i = 1; i <= 7; i++) {
            if((oppoId + i)%15 == playerId) {
                Result.text = "WIN";
                CarController.Instance.bike += 10;
                return;
            }
        }
        //else
        CarController.Instance.bike += 10;
        Result.text = "LOST";
        //exitButton.SetActive(true);
        //imgs[15].color = new Color32(255, 255, 255, 255);
        matched = true;
    }

    public void onExit() {
        GameObject.FindWithTag("Car").GetComponent<Transform>().position = GameManager.carPosition;
        GameManager.Instance.pause = false;
        SceneManager.LoadSceneAsync("MainScene");

    }
}
