using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Events;
using TMPro;

public class OilBar : MonoBehaviour
{
    public static OilBar Instance;
    public Slider slider;
    public Slider easeSlider;
    public float maxOil = 100f;
    public float oil;
    private float lerpSpeed = 0.01f;
    public TextMeshProUGUI myText;

    // Start is called before the first frame update
    void Awake() {
        //Debug.Log(GameManager.OnTimeUpdated);
        Instance = this;
        InvokeRepeating("oilCutPerSecond", 1f, 1f);
        //GameManager.OnTimeUpdated.AddListener(oilCutPerSecond);
    }

    void oilCutPerSecond() {
        //Debug.Log(CarController.Instance.GetVelocityMagnitude());
        if(GameManager.Instance.pause)
            return;
        float uwu = (int)Mathf.Round(2 * CarController.Instance.GetVelocityMagnitude() / CarController.Instance.maxSpeed);
        if(CarController.Instance.onGrass) {
            uwu *= 3;
        }
        oilCut(1 + uwu);
    }

    void Start()
    {
        oil = maxOil;
    }

    // Update is called once per frame
    void Update()
    {
        if(slider.value != oil) {
            slider.value = oil;
        }
        if (Input.GetKeyDown("space"))
        {
            oilCut(10);
        }
        if(slider.value != easeSlider.value) {
            easeSlider.value = Mathf.Lerp(easeSlider.value, oil, lerpSpeed);
        }
        myText.text = "Oil: " + oil.ToString() + '/' + maxOil.ToString();
    }

    void oilCut(float amount) {
        if(!CarController.Instance.onBroken && oil >= 0)
            oil -= amount;
        if(oil < 0) {
            oil = 0;
            CarController.Instance.onBroken = true;
        }
    }
}
