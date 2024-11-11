using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Events;
using TMPro;

public class PRBar : MonoBehaviour
{
    public Slider slider;
    public Slider easeSlider;
    public float oil;
    private float lerpSpeed = 0.01f;
    public TextMeshProUGUI myText;

    // Start is called before the first frame update

    void Start()
    {
        oil = GameManager.Instance.maxPR;
    }

    // Update is called once per frame
    void Update()
    {
        oil = GameManager.Instance.PR;
        if(slider.value != oil) {
            slider.value = oil;
        }
        if(slider.value != easeSlider.value) {
            easeSlider.value = Mathf.Lerp(easeSlider.value, oil, lerpSpeed);
        }
        myText.text = "Reputation: " + oil.ToString() + '/' + GameManager.Instance.maxPR.ToString();
    }
}

