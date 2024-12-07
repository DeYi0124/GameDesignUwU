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
    // private Color32[] levelColors = new Color32[6] {Color.cyan. Color.magenta, Color.green, Color.yellow, Color.green, Color.orange, Color.red};
    private Color[] levelColors = new Color[6] {Color.cyan, Color.blue, Color.green, Color.yellow, Color.magenta, Color.red};

    // Start is called before the first frame update

    void Start()
    {
        oil = GameManager.Instance.maxPR;
    }

    // Update is called once per frame
    void Update()
    {
        oil = (GameManager.Instance.PR%100);
        GameManager.Instance.PRlevel = GameManager.Instance.PR/100 - 1;
        // if(GameManager.Instance.PRlevel >= 5) {
        //     GameManager.Instance.PRlevel = 4;
        // }
        slider.GetComponentInChildren<Image>().color = levelColors[GameManager.Instance.PRlevel+1];
        if(slider.value != oil) {
            slider.value = oil;
        }
        if(slider.value != easeSlider.value) {
            easeSlider.value = Mathf.Lerp(easeSlider.value, oil, lerpSpeed);
        }
        myText.text = "Reputation: " + oil.ToString() + '/' + GameManager.Instance.maxPR.ToString();
    }
}

