using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;
using UnityEngine.SceneManagement;
using TMPro;

public class LGgameManager : MonoBehaviour
{
    public TextMeshProUGUI A;
    public TextMeshProUGUI B;
    public TextMeshProUGUI C;
    public TextMeshProUGUI D;
    public AudioSource lockSound;
    public GameObject exitButton;

    private int pA, pB, pC, pD;
    private int gA, gB, gC, gD;
    private int attempts;
    private bool passed;
    // Start is called before the first frame update
    void Start()
    { 
        pA = Random.Range(0, 10);
        pB = Random.Range(0, 10);
        pC = Random.Range(1, 10);
        pD = Random.Range(0, 10);
        gA = gB = gC = gD = attempts = 0;
        exitButton.SetActive(false);
        passed = false;
    }

    void playSound(){
        // Debug.Log("Sound played");
        lockSound.Play();
    }

    public void changeA(){
        if(passed) return;
        gA++;
        gA = (gA+10)%10;
        if(gA == pA) {
            playSound();
        }
        attempts++;
    }

    public void changeB(){
        if(passed) return;
        gB--;
        gB = (gB+10)%10;
        if(gB == pB) {
            playSound();
        }
        attempts++;
    }
    public void changeC(){
        if(passed) return;
        gC += 2;
        gC = (gC+10)%10;
        if(gC == pC) {
            playSound();
        }
        attempts++;
    }
    public void changeD(){
        if(passed) return;
        gD++;
        if(gD >= 10) {
            gC = (gC+1)%10;
            if(gC == pC) {
                playSound();
            }
        }
        gD = (gD+10)%10;
        if(gD == pD) {
            playSound();
        }
        attempts++;
    }

    // Update is called once per frame
    void Update()
    {
        A.text = gA.ToString();
        B.text = gB.ToString();
        C.text = gC.ToString();
        D.text = gD.ToString();
        if(gA == pA && gB == pB && gC == pC && gD == pD){
            A.color = Color.green;
            B.color = Color.green;
            C.color = Color.green;
            D.color = Color.green;
            exitButton.SetActive(true);
            passed = true;
        }
    }

    public void onExit() {
        Debug.Log(((80 / attempts) + 1));
        GameManager.Instance.pause = false;
        GameManager.Instance.bike += ((80 / attempts) + 1);
        SceneManager.LoadSceneAsync("MainScene");

    }
}
