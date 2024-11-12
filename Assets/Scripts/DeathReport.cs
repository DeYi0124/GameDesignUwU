using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using TMPro;

public class DeathReport : MonoBehaviour
{
    public Animator transition;
    public float transitionTime = 3f;
    public TextMeshProUGUI LoadText;
    public TextMeshProUGUI ReasonText;
    public TextMeshProUGUI bikes;
    public TextMeshProUGUI creditEarned;

    void LoadingScreen() {
        StartCoroutine(LoadLevel());
    }

    void UpdateData() {
        bikes.text = GameManager.Instance.bike.ToString();
        creditEarned.text = 0.ToString();
        ReasonText.text = GameManager.Instance.ReasonText;
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
        SceneManager.LoadScene("MainMenu");   
        GameManager.Instance.reset();
    }
}
