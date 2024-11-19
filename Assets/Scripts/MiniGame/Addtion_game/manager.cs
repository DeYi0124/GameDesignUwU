using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using TMPro;

public class manager : MonoBehaviour
{
    public TextMeshProUGUI num1;
    public TextMeshProUGUI num2;
    public TMP_InputField text;
    public TextMeshProUGUI countDown;
    public GameObject correct;
    public GameObject wrong;
    private string submit;
    private int ans;
    private bool isCorrect = false;
    private bool isChecked = false;
    private float time;
    private bool pauseTime = false;
    void Start(){
        StartCoroutine(waitAndActive());
        time = 10;
        correct.SetActive(false);
        wrong.SetActive(false);
    }
    void Awake() {
        InvokeRepeating("updateTime",1f,1f);
        int A = Random.Range(100, 1000);
        int B = Random.Range(100, 1000);
        ans = A + B;
        num1.text = A.ToString();
        num2.text = B.ToString();
    }
    void Update() {
        if(time == 0){
            wrong.SetActive(true);
            GameManager.Instance.pause = false;
            pauseTime = true;
            SceneManager.LoadScene("MainScene");

        }
    }
    public void readAnswer(string submition){
        submit = submition;
        if(submit == ans.ToString()){
            isCorrect = true;
            correct.SetActive(true);
            CarController.Instance.bike += 3;
            StartCoroutine(showAndWait());
        }
        else{
            wrong.SetActive(true);
            StartCoroutine(showAndWait());
            isCorrect = false;

        }
    }
    void updateTime(){
        if(!pauseTime){
            time-=1f;
            countDown.text = time.ToString();
        }
    }
    IEnumerator waitAndActive(){
        yield return new WaitForSeconds(1f);
        text.ActivateInputField();
    }
    IEnumerator showAndWait(){
        pauseTime = true;
        yield return new WaitForSeconds(1f);
        SceneManager.LoadScene("MainScene");
        GameManager.Instance.pause = false;
    }
}