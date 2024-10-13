using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class levelPanel : MonoBehaviour
{
    // Start is called before the first frame update
    public TextMeshProUGUI volume;
    public TextMeshProUGUI fuel;
    public TextMeshProUGUI speed;
    public TextMeshProUGUI skill;
    void Update()
    {
        volume.text = "volume level: " + GameManager.Instance.volumeLevel;
        fuel.text = "fuel level: " + GameManager.Instance.oilLevel;
        speed.text = "speed level: " + GameManager.Instance.speedLevel;
        skill.text = "skill level: " + GameManager.Instance.skillLevel;
    }
}
