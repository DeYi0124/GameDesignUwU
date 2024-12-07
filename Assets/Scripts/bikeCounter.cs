using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class bikeCounter : MonoBehaviour
{
    // Update is called once per frame
    public TextMeshProUGUI myText;
    void Update()
    {
        myText.text = "Bike Counter: " + GameManager.Instance.bike.ToString() + "/" + GameManager.Instance.KPI.ToString();
        if(GameManager.Instance.bike >= GameManager.Instance.KPI) {
            myText.color = Color.green;
        }else {
            myText.color = Color.red;
        }
    }
}
