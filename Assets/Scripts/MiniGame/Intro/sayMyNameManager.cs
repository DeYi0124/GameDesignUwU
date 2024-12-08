using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using UnityEngine.SceneManagement;
using System.Text.RegularExpressions;

public class sayMyNameManager : MonoBehaviour
{
    public TextMeshProUGUI walterText;
    static public string playerName = "";
    public GameObject inputField;
    public TextMeshProUGUI inputText;
    public GameObject whatsMyPurpose;
    public AudioSource youAreGoddamnRight;
    public GameObject speechBubble;
    public SpriteRenderer blackScreen;
    public IntroManager introManager;
    public whatsMyPurpose whatsMyPurposeScript;
    private bool submitted = false;
    private bool enteredName = false;
    private string currentText = "";
    private int errorID = 0;
    private bool changed = false;
    private bool validName = true;
    void Start()
    {
        speechBubble.GetComponent<SpriteRenderer>().color = new Color(1,1,1,0);
        walterText.color = new Color(walterText.color.r,walterText.color.g,walterText.color.b,0);
        speechBubble.SetActive(false);
        introManager.blackScreenObject.SetActive(true);
        blackScreen.color = new Color(0,0,0,0);
    }
    void Update(){
        if(Regex.IsMatch(playerName, @"^[a-zA-Z0-9_+=/!?-`' ]+$") && playerName.Length > 0 && playerName.Length <= 13) validName = true;
        else validName = false;
        if(errorID != 1 && !Regex.IsMatch(playerName, @"^[a-zA-Z0-9_+=/!?-`' ]+$") && playerName.Length > 0){
            StartCoroutine(changeText("English only, thank you.",walterText));
            errorID = 1;
            validName = false;
            changed = true;
            return;
        }
        if(playerName.Length > 13 && errorID != 2){
            StartCoroutine(changeText("Keep your name under 12 characters please.",walterText));
            errorID = 2;
            validName = false;
            changed = true;
            return;
        }

        if(validName && changed && errorID != 3){
            errorID = 3;
            StartCoroutine(textFadeOut(walterText,0.5f,1.3f));
            StartCoroutine(fadeOut(speechBubble.GetComponent<SpriteRenderer>(),0.6f,1.5f));
        }
        if(validName && submitted && !enteredName){
            enteredName = true;
            inputText.color = Color.red;
            inputField.GetComponent<TMP_InputField>().readOnly = true;
            youAreGoddamnRight.Play();
            whatsMyPurposeScript.readPlayerNameFromScript();
            StartCoroutine(textFadeOut(walterText,1.3f,0.5f));
            StartCoroutine(fadeOut(speechBubble.GetComponent<SpriteRenderer>(),0.8f,0.8f));
            StartCoroutine(fadeIn(blackScreen,5f,2f));
            StartCoroutine(changeScene());
        }
    }

    public void sumbit(){
        if(playerName.Length == 0) return;
        submitted = true;
    }
    public void readName(string input){
        // playerName = input;
        // submitted = true;
    }
    public void onValueChanged(string cock){
        playerName = cock;
    }
    public string getPlayerName() {
        return playerName;
    }
    public void inSayMyName(){
        StartCoroutine(prepWork());
    }
    IEnumerator prepWork(){
        speechBubble.SetActive(true);
        StartCoroutine(fadeIn(speechBubble.GetComponent<SpriteRenderer>(),1.2f,1f));
        walterText.text = "Sign your name here, then you'll become a SheiYuen Intern.";
        yield return StartCoroutine(textFadeIn(walterText,1.2f,1f));
    }
    IEnumerator changeScene(){
        introManager.turnOffLastBGM();
        yield return new WaitForSeconds(3);
        SceneManager.LoadScene("MainMenu");
    }
    IEnumerator changeText(string content,TextMeshProUGUI textMeshPro){
        if(speechBubble.GetComponent<SpriteRenderer>().color.a < 0.2f){
            StartCoroutine(fadeIn(speechBubble.GetComponent<SpriteRenderer>(),1.2f));
        }
        yield return StartCoroutine(textFadeOut(walterText,0.3f));
        walterText.text = content;
        StartCoroutine(textFadeIn(walterText,0.3f));

    }
    IEnumerator fadeIn(SpriteRenderer spriteRenderer,float time, float delay = 0f){
        yield return new WaitForSeconds(delay);
        float originalAlpha = spriteRenderer.color.a;
        spriteRenderer.color = new Color(spriteRenderer.color.r,spriteRenderer.color.g,spriteRenderer.color.b,originalAlpha);
        float interval = time/Time.deltaTime;
        for(int i = 0;i<interval;i++){
            spriteRenderer.color = new Color(spriteRenderer.color.r,spriteRenderer.color.g,spriteRenderer.color.b,i/interval);
            yield return new WaitForSeconds(Time.deltaTime);
        }
    } 
    IEnumerator textFadeIn(TextMeshProUGUI textMeshPro,float time, float delay = 0f){
        yield return new WaitForSeconds(delay);
        float originalAlpha = textMeshPro.color.a;
        textMeshPro.color = new Color(textMeshPro.color.r,textMeshPro.color.g,textMeshPro.color.b,originalAlpha);
        float interval = time/Time.deltaTime;
        for(int i = 0;i<interval;i++){
            textMeshPro.color = new Color(textMeshPro.color.r,textMeshPro.color.g,textMeshPro.color.b,i/interval);
            yield return new WaitForSeconds(Time.deltaTime);
        }
    }

    IEnumerator fadeOut(SpriteRenderer spriteRenderer,float time, float delay = 0f){
        yield return new WaitForSeconds(delay);
        float originalAlpha = spriteRenderer.color.a;
        spriteRenderer.color = new Color(spriteRenderer.color.r,spriteRenderer.color.g,spriteRenderer.color.b,originalAlpha);
        float interval = time/Time.deltaTime;
        for(int i = 0;i<interval;i++){
            if(spriteRenderer.color.a > 0f){
                spriteRenderer.color = new Color(spriteRenderer.color.r,spriteRenderer.color.g,spriteRenderer.color.b,originalAlpha-(i/interval*originalAlpha));
            }
            yield return new WaitForSeconds(Time.deltaTime);
        }
    }
    IEnumerator textFadeOut(TextMeshProUGUI textMeshPro,float time, float delay = 0f){
        yield return new WaitForSeconds(delay);
        float originalAlpha = textMeshPro.color.a;
        textMeshPro.color = new Color(textMeshPro.color.r,textMeshPro.color.g,textMeshPro.color.b,originalAlpha);
        float interval = time/Time.deltaTime;
        for(int i = 0;i<interval;i++){
            if(textMeshPro.color.a > 0f){
                textMeshPro.color = new Color(textMeshPro.color.r,textMeshPro.color.g,textMeshPro.color.b,originalAlpha-(i/interval*originalAlpha));
            }
            yield return new WaitForSeconds(Time.deltaTime);
        }
    }
}
    


