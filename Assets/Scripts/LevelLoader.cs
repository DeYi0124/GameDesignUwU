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

    public void fi() {
        StartCoroutine(lfi());
    }

    public void FadeIn() {
        StartCoroutine(LoadFadeIn());
    }

    public void FadeOut() {
        StartCoroutine(LoadFadeOut());
    }

    public void FadeInT() {
        StartCoroutine(LoadFadeInT());
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

    IEnumerator lfi() {
        LoadText.text = "Loading...";
        // Debug.Log("UwU");
        transition.SetTrigger("bait master");
        yield return new WaitForSecondsRealtime(3f);
    }

    IEnumerator LoadFadeIn() {
        LoadText.text = "Loading...";
        transition.SetTrigger("Start");
        yield return new WaitForSecondsRealtime(3f);
        SceneManager.LoadSceneAsync("MainScene");
        Destroy(this);
    }

    IEnumerator LoadFadeOut() {
        LoadText.text = "Loading...";
        transition.SetTrigger("master bait");
        yield return new WaitForSecondsRealtime(3f);
    }

    IEnumerator LoadFadeInT() {
        LoadText.text = "Guiding...";
        transition.SetTrigger("Start");
        yield return new WaitForSecondsRealtime(3f);
        SceneManager.LoadSceneAsync("TutorialScene");
        Destroy(this);
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
