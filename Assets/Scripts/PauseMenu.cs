using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using TMPro;

public class PauseMenu : MonoBehaviour
{
    public GameObject pauseMenu;
    // public TextMeshProUGUI timeText;
    public static bool isPaused;
    private bool inMainScene;
    // Start is called before the first frame update
    void OnEnable()
    {
        SceneManager.sceneLoaded += OnSceneLoaded;
    }
    void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        if(scene.name == "MainScene" || scene.name == "TutorialScene") {
            // if(inMainScene)
            //     timeText = GameObject.FindWithTag("TimeText").GetComponent<TextMeshProUGUI>();
            pauseMenu = GameObject.Find("PauseMenu");
            pauseMenu.SetActive(false);
        }
    }
    void Awake() {
        //Debug.Log("Paused"); 
    }
    void Start()
    {
        //pauseMenu.SetActive(false);
    }
    void OnDisable()
    {
        // Debug.Log("OnDisable");
        SceneManager.sceneLoaded -= OnSceneLoaded;
    }

    // Update is called once per frame
    void Update()
    {
        inMainScene = (SceneManager.GetActiveScene().name == "MainScene" || SceneManager.GetActiveScene().name == "TutorialScene");
        // int sec = (int)Time.time%60;
        // int min = (int)Time.time/60;
        // if(inMainScene)
        //     timeText.text = "Time Played: " + string.Format("{0:00}:{1:00}", min, sec);
        if(Input.GetKeyDown(KeyCode.Escape)) {
            if(isPaused) {
                ResumeGame();
            } else {
                PauseGame();
            }
        }
    }
    public void PauseGame() {
        Time.timeScale = 0;
        pauseMenu.SetActive(true);
        isPaused = true;
    }
    public void ResumeGame() {
        Time.timeScale = 1;
        pauseMenu.SetActive(false);
        isPaused = false;
    }
    public void GoToMainMenu() {
        Destroy(GameObject.Find("GameManager"));
        SceneManager.LoadScene("MainMenu");
        Time.timeScale = 1;
    }
    public void QuitGame() {
        Application.Quit();
    }
}
