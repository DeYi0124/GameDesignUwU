using System.Collections;
using System.Collections.Generic;
using UnityEngine.SceneManagement;
using UnityEngine;
using UnityEngine.UI;
using Random = UnityEngine.Random;
using TMPro;

public class memGameManager : MonoBehaviour
{
    public Animator[] buttons;
    public TextMeshProUGUI counterText;
    public TextMeshProUGUI bullshitText;
    [SerializeField]
    List<Button> butt = new List<Button>();
    int[] res = new int[25];

    private int counter = 0;
    private float time = 0;
    private int pathIndex = 0;
    private string[] bullshits = {"As a ShueiYuan noob, you are quite impressive.", "Wow, you really ARE something.", "I am suprised that a person like you can reach this level.", "Wow, you are a very talented person.", "The fate of NTU is relied on you."};
    
    void Start()
    {
        for (int i = 0; i < buttons.Length; i++)
        {
            buttons[i].SetBool("isGlowing",false);
            int cacheIndex = i; // we have to cache i to have the right value in the anonymous method
            butt[i].onClick.AddListener(() => ButtonClicked(cacheIndex));
        }
        time = 50;
        counter = 1;
        bullshitText.text = "Prove that you are capable to catch up the speed of god.";
        StartMemGame(counter);
        // SceneManager.LoadScene("MainScene");
    }

    void StartMemGame(int cnt){
        pathIndex = 0;
        int prev = -1;
        for(int i = 0; i < cnt; i++){
            res[i] = Random.Range(0, 25);
            while(res[i] == prev){
                res[i] = Random.Range(0, 25);
            }
            prev = res[i];
        }
        StartCoroutine(UwU(0.5f, cnt, res));
        Debug.Log("Start mem game with " + cnt);
    }

    IEnumerator UwU(float seconds, int cnt, int[] res){
        for(int i = 0; i < 25; i++) {
            butt[i].interactable = false;
        }
        for(int i = 0; i < cnt; i++) {
            StartCoroutine(showPath(1f, res[i]));
            yield return new WaitForSeconds(1f);
        }
        for(int i = 0; i < 25; i++) {
            butt[i].interactable = true;
        }
        pathIndex = 0;
        yield return null;
    }

    IEnumerator showPath(float seconds, int index){
        buttons[index].SetBool("isGuild",true);
        yield return new WaitForSeconds(seconds);
        buttons[index].SetBool("isGuild",false);
    }
    IEnumerator ShowError(float seconds, int index){
        buttons[index].SetBool("isError",true);
        yield return new WaitForSeconds(seconds);
        buttons[index].SetBool("isError",false);
    }


    IEnumerator ShowText(string fullText)
    {
        string currentText = "";
        for (int i = 0; i < fullText.Length; i++)
        {
            if(fullText[i] == '<') {
                while(fullText[i] != '>') {
                    currentText += fullText[i];
                    i++;
                }
                currentText += fullText[i];
            }
            else currentText += fullText[i];
            bullshitText.text = currentText;
            yield return new WaitForSeconds(0.05f);
        }
    }


    IEnumerator wait(float seconds, int index){
        for(int i = 0; i < 25; i++) {
            butt[i].interactable = false;
        }
        if(res[pathIndex] != index){
            for(int i = 0; i < 25; i++) {
                butt[i].interactable = false;
            }
            // bullshitText.text = "You Lose, oops";
            StartCoroutine(ShowError(1f, index));
            StartCoroutine(showPath(1f, res[pathIndex]));
            StartCoroutine(ShowText("You failed, oops."));
            yield return new WaitForSeconds(3f);
            if(counter <= 5) {
                GameManager.Instance.ReasonText = "You failed to pass the test of the NTHU god and thus you are expelled from NTU";
                SceneManager.LoadScene("DeathReportScene");
            }
            CarController.Instance.bike += counter;
            SceneManager.LoadScene("MainScene");
        }
        pathIndex++;
        butt[index].interactable = false;
        buttons[index].SetBool("isGlowing",true);
        yield return new WaitForSeconds(seconds);
        buttons[index].SetBool("isGlowing",false);
        butt[index].interactable = true; 
        if(pathIndex == counter){
            Debug.Log(pathIndex + "=" + counter);
            for(int i = 0; i < 25; i++) {
                butt[i].interactable = false;
            }
            StartCoroutine(ShowText(bullshits[Random.Range(0, bullshits.Length)]));
            yield return new WaitForSeconds(5f);
            counter++;
            if(counter > 20) {
                StartCoroutine(Victory());
            }else
                StartMemGame(counter);
        }
        for(int i = 0; i < 25; i++) {
            butt[i].interactable = true;
        }
    }
    IEnumerator Victory(){
        StartCoroutine(ShowText("You've passed my test, congratulations!"));
        yield return new WaitForSeconds(3f);
        CarController.Instance.bike += 20;
        SceneManager.LoadScene("MainScene");
    }
        
    void ButtonClicked(int index)
    {
        // do something with the index
        StartCoroutine(wait(1f, index));
    }

    // Update is called once per frame
    void Update()
    {
        counterText.text = "Counter: " + counter.ToString();

    }
}
