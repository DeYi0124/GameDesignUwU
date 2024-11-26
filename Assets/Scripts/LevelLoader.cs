using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.SceneManagement;

public class LevelLoader : MonoBehaviour
{
    public Animator transition;
    public float transitionTime = 3f;
    public TextMeshProUGUI LoadText;
    void Awake()
    {
        transitionTime = 3f;
        CurrentBike[] currs = FindObjectsOfType<CurrentBike>();
        foreach (CurrentBike curr in currs) {
            curr.OnSave += OnSave;
        }
    }

    void OnDestroy()
    {
        CurrentBike[] currs = FindObjectsOfType<CurrentBike>();
        foreach (CurrentBike curr in currs) {
            curr.OnSave -= OnSave;
        }
    }

    void OnSave(CurrentBike currBike) {
        //Debug.Log("UWUevent!");
        LoadingScreen();
    }

    void LoadingScreen() {
        //Debug.Log("UwU?");
        StartCoroutine(LoadLevel());
    }

    public void FadeIn() {
        StartCoroutine(LoadFadeIn());
    }

    IEnumerator LoadLevel() {
        LoadText.text = "Loading...";
        GameManager.Instance.pause = true;
        transition.SetTrigger("Start");
        yield return new WaitForSecondsRealtime(transitionTime);
        //CarController.Instance.transform.position += new Vector3(-24, 4, 10);
        transition.SetTrigger("End");
        GameManager.Instance.pause = false;
    }

    IEnumerator LoadFadeIn() {
        LoadText.text = "Loading...";
        transition.SetTrigger("Start");
        yield return new WaitForSecondsRealtime(3f);
        SceneManager.LoadSceneAsync("MainScene");
        Destroy(this);
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
