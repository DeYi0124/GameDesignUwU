using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using TMPro;

public class GameReport : MonoBehaviour
{
    public Animator transition;
    public float transitionTime = 3f;
    public TextMeshProUGUI LoadText;
    public TextMeshProUGUI bikes;
    public TextMeshProUGUI coinsEarned;

    private int bikesCnt = 0;
    private int coinsCnt = 0;

    void LoadingScreen() {
        StartCoroutine(LoadLevel());
    }

    void UpdateData() {
        bikes.text = bikesCnt.ToString();
        if(bikesCnt > GameManager.Instance.KPI) {
            bikes.text += ("(extra bike: " + (bikesCnt - GameManager.Instance.KPI) +")");
        }
        coinsEarned.text = coinsCnt.ToString();
    }
    void Start(){
        UpdateData();
        bikesCnt = GameManager.Instance.bike;
        coinsCnt = (GameManager.Instance.bike * 5);
    }
    void Update() {
        UpdateData();
    }

    IEnumerator LoadLevel() {
        LoadText.text = "Next day...";
        transition.SetTrigger("Start");
        yield return new WaitForSecondsRealtime(transitionTime);
        GameManager.Instance.reset();
        SceneManager.LoadScene("MainScene");   
    }
    public void ContinueGame() {
        // LoadingScreen();
        SceneManager.LoadScene("MainScene");
        GameManager.Instance.reset();
    }
    public void EnterShop() {
        //LoadingScreen();
        SceneManager.LoadSceneAsync("Shop");
    }
}
