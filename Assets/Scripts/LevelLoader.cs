using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

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
        Debug.Log("UwU?");
        StartCoroutine(LoadLevel());
    }

    IEnumerator LoadLevel() {
        LoadText.text = "Loading...";
        GameManager.Instance.pause = true;
        transition.SetTrigger("Start");
        yield return new WaitForSecondsRealtime(transitionTime);
        transition.SetTrigger("End");
        GameManager.Instance.pause = false;
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
