using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class PauseMenu : MonoBehaviour
{
    public GameObject pauseMenu;
    public static bool isPaused;
    // Start is called before the first frame update
    void OnEnable()
    {
        Debug.Log("OnEnable called");
        SceneManager.sceneLoaded += OnSceneLoaded;
    }
    void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        //Debug.Log("OnSceneLoaded: " + scene.name);
        //Debug.Log(mode);
        if(scene.name == "MainScene") {
            pauseMenu = GameObject.Find("PauseMenu");
            pauseMenu.SetActive(false);
        }
    }
    void Awake() {
        //Debug.Log("Paused");
    }
    void Start()
    {
        pauseMenu.SetActive(false);
    }
    void OnDisable()
    {
        Debug.Log("OnDisable");
        SceneManager.sceneLoaded -= OnSceneLoaded;
    }

    // Update is called once per frame
    void Update()
    {
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
        SceneManager.LoadScene("MainMenu");
        Time.timeScale = 1;
    }
    public void QuitGame() {
        Application.Quit();
    }
}
