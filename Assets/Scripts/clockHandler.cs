using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class clockHandler : MonoBehaviour
{
    public Slider slider;
    public TextMeshProUGUI myText;
    int startingTime = 0;
    // Start is called before the first frame update
    void Start()
    {
        slider.maxValue = GameManager.Instance.maxTime;
    }

    // Update is called once per frame
    void Update()
    {
        slider.value = GameManager.Instance.getTime() - startingTime;
        if(GameManager.Instance.getTime() < GameManager.Instance.maxTime/2) {
            myText.text = "MORNING";
        }else {// if(GameManager.Instance.getTime() < 40) {
            myText.text = "EVENING";
        }
        
    }
}
