using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LevelLoader : MonoBehaviour
{
    public Animator transition;
    public float transitionTime = 1f;
    void Awake()
    {
        CurrentBike[] currs = FindObjectsOfType<CurrentBike>();
        foreach (CurrentBike curr in currs) {
            curr.OnSave += OnSave;
        }
    }

    void OnSave(CurrentBike currBike) {
        //Debug.Log("UWUevent!");
        LoadingScreen();
    }

    void LoadingScreen() {
        StartCoroutine(LoadLevel());
    }

    IEnumerator LoadLevel() {
        transition.SetTrigger("Start");

        yield return new WaitForSeconds(transitionTime);

        transition.SetTrigger("End");
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
