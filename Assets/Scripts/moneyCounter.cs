using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class moneyCounter : MonoBehaviour
{
    // Update is called once per frame
    public TextMeshProUGUI coins;
    public TextMeshProUGUI credits;
    void Update()
    {
        coins.text = "Coins: " + GameManager.Instance.GetMoney().Item1.ToString();
        credits.text = "Credits: " + GameManager.Instance.GetMoney().Item2.ToString();
    }
}
