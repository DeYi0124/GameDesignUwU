using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using Random = UnityEngine.Random;
using System.Linq;
using System;

public class DaManager : MonoBehaviour
{
    public TMP_InputField inputField;
    public GameObject Enemy;
    public GameObject[] SpawnPoint;
    public GameObject result;
    public Animator[] hearts;
    public TextMeshProUGUI TypedWord;
    public TextMeshProUGUI countDown;
    public Sprite[] EnemySprite;
    public GameObject Canvas;
    private List<string> words = new List<string>();
    private string submission = "";
    private float interval = 5f;
    private float nextTime = 1f;
    private int hp;
    private bool pauseTime = false;
    private float time;
    private float gameTime;
    private float startTime;

    // Start is called before the first frame update
    void Start()
    {
        setWords();
        result.SetActive(false);
        inputField.ActivateInputField();
        inputField.text = "";
        for(int i = 0;i < 3;i++){
            hearts[i].SetBool("isHurt",false);
        }
        hp = 0;
        startTime = Time.time;
        time = 30;
    }
    void Awake(){
        InvokeRepeating("updateTime",1f,1f);

    }
    void updateTime(){
        if(!pauseTime){
            time-=1f;
            countDown.text = time.ToString();
        }
    }
    // Update is called once per frame
    void Update()
    {
        gameTime = Time.time - startTime;
        if (gameTime >= 20)
            interval = 2f;
        else if(gameTime >= 10)
            interval = 2.5f;
        else if(gameTime >= 0)
            interval = 3f;
        if (gameTime >= nextTime) {
            GameObject tmp = Instantiate(Enemy,SpawnPoint[Random.Range(0,SpawnPoint.Length)].transform.position,Quaternion.identity);
            tmp.transform.SetParent(Canvas.transform);
            tmp.transform.localScale = new Vector3(1,1,1);
            nextTime += interval; 

        }
        if(time == 0){
            pauseTime = true;
            passedGame();
        }

    }
    IEnumerator SpawnEnemy(){
        yield return new WaitForSeconds(1f);
        GameObject tmp = Instantiate(Enemy,SpawnPoint[Random.Range(0,SpawnPoint.Length)].transform.position,Quaternion.identity);
        // StopAllCoroutines();
    }
    private void failedGame(){
        result.SetActive(true);
        pauseTime = true;
        TextMeshProUGUI resultText = result.GetComponent<TextMeshProUGUI>();
        resultText.color = Color.red;
        resultText.text = "YOU FAILED";
        CarController.Instance.bike = 0;
        StartCoroutine(waitAndChangeScene(1.5f));
    }
    private void passedGame(){
        pauseTime = true;
        result.SetActive(true);
        TextMeshProUGUI resultText = result.GetComponent<TextMeshProUGUI>();
        resultText.color = Color.green;
        resultText.text = "YOU PASSED";
        StartCoroutine(waitAndChangeScene(1.5f));
    }
    public void readAnswer(string inputText){
        submission = inputText;
    }
    public void userPressedEnter(string ans){
        submission = ans;
        inputField.text = "";
        inputField.ActivateInputField();
    }
    public void clearTextBox(){
        inputField.text = "";
        inputField.ActivateInputField();
    }
    public void minusHP(){
        if(hp == 3) return;
        hearts[hp].SetBool("isHurt",true);
        hp++;
        if (hp == 3){
            failedGame();
        }
    }
    IEnumerator waitAndChangeScene(float time){
        pauseTime = true;
        yield return new WaitForSeconds(time);
        SceneManager.LoadScene("MainScene");
        GameManager.Instance.pause = false;
    }
    public string getSubmission(){
        return submission;
    }
    public float getTime(){
        return gameTime;
    }
    public string getWord(int rng){
        return words[rng];
    }
    public int getWordCount(){
        return words.Count();
    }
    public void setWords(){
        // Debug.Log("setWords");
        words = new List<string>{"nationalism","vertical","transfer","cousin","theory","collection","bubble","transform","inhabitant","summit","betray","stadium","disability","incident","vacuum","primary","forbid","aspect","architect","rehearsal","mosque","abstract","nature","cemetery","crosswalk","explicit","dynamic","crutch","domestic","sausage","limited","landscape","unaware","bracket","follow","heaven","inflate","highlight","scramble","sweater","classroom","ballet","profile","scheme","threat","thrust","printer","infrastructure","extension","ground","connection","measure","immune","earthquake","double","entertain","attract","bottom","dilute","sermon","manufacture","struggle","residence","agriculture","machinery","effective","estimate","broadcast","predict","architecture","command","battlefield","litigation","design","impulse","housing","software","integration","misplace","highway","officer","variable","remember","breakfast","deputy","excavation","concept","morsel","penetrate","neighborhood","recession","passion","opponent","foreigner","suitcase","migration","reaction","tropical","obstacle","office","perceive","buffet","context","orange","orthodox","practical","confidence","position","defend","liberal","photography","marketing","cheque","neglect","building","first-hand","density","advocate","restaurant","policeman","prince","develop","charter","vigorous","flavor","governor","predator","principle","source","rubbish","charge","proportion","example","hallway","ignorant","prejudice","purpose","intention","snuggle","disclose","dollar","bloody","eliminate"};
    }
}
