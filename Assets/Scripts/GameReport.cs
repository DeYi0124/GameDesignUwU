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

    void LoadingScreen() {
        StartCoroutine(LoadLevel());
    }

    void UpdateData() {
        bikes.text = GameManager.Instance.bike.ToString();
        coinsEarned.text = (GameManager.Instance.bike * 5).ToString();
    }

    void Update() {
        UpdateData();
    }

    IEnumerator LoadLevel() {
        LoadText.text = "Next day...";
        transition.SetTrigger("Start");
        yield return new WaitForSecondsRealtime(transitionTime);
        SceneManager.LoadScene("MainScene");   
        GameManager.Instance.reset();
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
