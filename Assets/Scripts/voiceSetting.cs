using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Audio;

public class voiceSetting : MonoBehaviour
{
    [SerializeField] AudioMixer audioMixer;
    [SerializeField] Slider slider;
    void Start() {
        SetVolume();
    }

    public void SetVolume()
    {
        float volume = slider.value;
        audioMixer.SetFloat("BGM", Mathf.Log10(volume) * 20);
    }
}
