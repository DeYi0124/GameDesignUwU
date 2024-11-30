using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using UnityEngine.SceneManagement;

public class AfterTyping : MonoBehaviour
{
    public float delay = 0f;
    public float speed = 0.1f;
    public string fullText;
    private string currentText = "";
    static public string playerName = "";
    private bool submitted = false;
    public TextMeshProUGUI thing;
    public GameObject inputField;
    public TextMeshProUGUI inputText;
    public GameObject submitButton;
    public GameObject whatsMyPurpose;
    public AudioSource audioSource;
    public AudioSource epicAFBGM;
    void Start()
    {
        StartCoroutine(ShowText());
        submitted = false;
    }
    void Awake(){


    }
    void Update(){
        if(submitted){
            inputText.color = Color.red;
        }
    }
    IEnumerator ShowText()
    {
        yield return new WaitForSeconds(delay);
        inputField.SetActive(true);
        submitButton.SetActive(true);
        this.GetComponent<Animator>().enabled = true;
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
            thing.text = currentText;
            yield return new WaitForSeconds(speed);
        }
    }

    IEnumerator HideText(){
        submitButton.GetComponent<Button>().interactable = false;
        inputField.GetComponent<TMP_InputField>().readOnly = true;
        submitted = true;
        for (int i = fullText.Length-1; i >= 0; i--)
        {
            currentText = currentText.Substring(0,i);
            thing.text = currentText;
            yield return new WaitForSeconds(speed);
        }
        whatsMyPurpose.GetComponent<whatsMyPurpose>().readPlayerNameFromScript();
        fullText = "It is the story of";
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
            thing.text = currentText;
            yield return new WaitForSeconds(speed);
        }
        yield return new WaitForSeconds(5f);
        SceneManager.LoadScene("MainMenu");
        // this.GetComponent<Animator>().SetTrigger("leave");
    }

    public void sumbit(){
        if(playerName.Length == 0 || playerName.Length > 10) return;
        StartCoroutine(HideText());
        audioSource.Play();
    }
    public void readName(string input){
        playerName = input;
    }
    public string getPlayerName() {
        return playerName;
    }
    IEnumerator changeScene(){
        yield return new WaitForSeconds(2);
        SceneManager.LoadScene("MainMenu");
    }
}
    

