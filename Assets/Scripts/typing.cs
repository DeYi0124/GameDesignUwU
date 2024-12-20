using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class typing : MonoBehaviour
{
    public float delay = 0f;
    public float speed = 0.1f;
    public string fullText;
    private string currentText = "";
    void Start()
    {
        StartCoroutine(ShowText());
    }

    IEnumerator ShowText()
    {
        yield return new WaitForSeconds(delay);
        for (int i = 0; i < fullText.Length; i++)
        {
            if(fullText[i] == '<') {
                while(fullText[i] != '>') {
                    currentText += fullText[i];
                    i++;
                }
                currentText += fullText[i];
            }
            else currentText += fullText[i];
            this.GetComponent<TextMeshProUGUI>().text = currentText;
            yield return new WaitForSeconds(speed);
        }
    }
}

