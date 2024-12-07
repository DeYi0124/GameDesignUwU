using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Networking;
using System.IO;
using TMPro;
using System.Linq;
using UnityEngine.SceneManagement;

public class IntroManager : MonoBehaviour
{
    public SpriteRenderer prior;
    public SpriteRenderer next;
    public Sprite[] backGrounds;
    public SpriteRenderer uncle;
    public SpriteRenderer aura;
    public Animator auraAnimator;
    public SpriteRenderer[] students;
    public TextAsset prologue;
    public TextMeshProUGUI narration;
    public AudioSource[] bgm;
    public TextMeshProUGUI timer;
    public Animator[] studentsAnimator;
    public GameObject bike;
    public GameObject canvas;
    public Transform[] outerPoints;
    public Transform[] unclePoint;
    public Transform[] studentPoints;
    public GameObject sayMyName;
    public SpriteRenderer[] sayMyNameSprite;
    public Image penButton;
    public TMP_InputField sayMyNameInputField;
    public GameObject dialogueBox;
    public Image[] dialogueImages;
    public TextMeshProUGUI skip;
    public sayMyNameManager sayMyNameManager;
    public GameObject blackScreenObject;
    private List<string> content;
    private int sceneCount;
    private int bgmCount;
    private bool skipped = false;
    private IEnumerator process;
    void Start(){
        resetSprites();
        process = wholeScene();
        bgmCount = 0;
        sceneCount = 0;
        content = prologue.text.Split('\n').ToList();
        // StartCoroutine(keepCallingGen(outerPoints,studentPoints,10f,1f));
        StartCoroutine(process);
        

    }
    void Update(){
        timer.text = Time.time.ToString("F2");
        // if(skipped)
        //     StopCoroutine(process);
    }
    IEnumerator wholeScene(){
        bgm[0].Play();
        yield return StartCoroutine(TypeSentence(content[0]));
        yield return StartCoroutine(deleteSentence(2f,0.03f));
        yield return StartCoroutine(TypeSentence(content[1]));
        StartCoroutine(transition(backGrounds[sceneCount],backGrounds[sceneCount+1],1.5f));
        yield return StartCoroutine(deleteSentence(3f,0.03f));
        yield return StartCoroutine(TypeSentence(content[2]));
        StartCoroutine(transition(backGrounds[sceneCount],backGrounds[sceneCount+1],1.5f));
        yield return StartCoroutine(deleteSentence(3f,0.03f));
        yield return StartCoroutine(TypeSentence(content[3]));
        StartCoroutine(MusicFadeOut(bgm[bgmCount], 6f));
        bgmCount++;
        StartCoroutine(transition(backGrounds[sceneCount],backGrounds[sceneCount+1],1.5f));
        yield return StartCoroutine(deleteSentence(3f,0.02f));
        StartCoroutine(MusicFadeIn(bgm[bgmCount],10f));
        yield return StartCoroutine(TypeSentence(content[4],0.08f));
        yield return StartCoroutine(deleteSentence(2f,0.03f));
        yield return StartCoroutine(TypeSentence(content[5]));
        StartCoroutine(transition(backGrounds[sceneCount],backGrounds[sceneCount+1],1.5f));
        yield return StartCoroutine(deleteSentence(3f,0.03f));
        for(int i =0;i<students.Length;i++)
            StartCoroutine(fadeIn(students[i],1.3f));
        yield return new WaitForSeconds(1f);
        for(int i =0;i<studentsAnimator.Length;i++)
            studentsAnimator[i].SetBool("isMoving",true);
        yield return StartCoroutine(TypeSentence(content[6]));
        yield return StartCoroutine(deleteSentence(2f,0.03f));
        yield return StartCoroutine(TypeSentence(content[7]));
        StartCoroutine(MusicFadeOut(bgm[bgmCount], 6f));
        bgmCount++;
        for(int i =0;i<students.Length;i++)
            StartCoroutine(fadeOut(students[i],1.3f,1.75f));
        yield return new WaitForSeconds(1.3f);
        for(int i =0;i<studentsAnimator.Length;i++)
            studentsAnimator[i].SetBool("isMoving",false);
        StartCoroutine(transition(backGrounds[sceneCount],backGrounds[sceneCount+1],1.5f));
        yield return StartCoroutine(deleteSentence(2f,0.03f));
        StartCoroutine(MusicFadeIn(bgm[bgmCount],8f));
        yield return new WaitForSeconds(0f);
        StartCoroutine(fadeIn(uncle,2.5f));
        auraAnimator.SetBool("isGlowing",true);
        StartCoroutine(fadeIn(aura,2.7f));
        yield return StartCoroutine(TypeSentence(content[8]));
        StartCoroutine(transition(backGrounds[sceneCount],backGrounds[sceneCount+1],1.5f));
        yield return StartCoroutine(deleteSentence(3.3f,0.015f));
        yield return StartCoroutine(TypeSentence(content[9]));
        StartCoroutine(transition(backGrounds[sceneCount],backGrounds[sceneCount+1],1.5f));
        yield return StartCoroutine(deleteSentence(3.3f,0.015f));
        StartCoroutine(keepCallingGen(unclePoint,outerPoints,13.5f,0.5f));
        yield return StartCoroutine(TypeSentence(content[10]));
        yield return StartCoroutine(deleteSentence(4f,0.02f));
        yield return StartCoroutine(TypeSentence(content[11]));
        StartCoroutine(MusicFadeOut(bgm[bgmCount], 6f));
        bgmCount++;
        yield return StartCoroutine(deleteSentence(3f,0.02f));
        StartCoroutine(fadeOut(uncle,1.7f,4f));
        StartCoroutine(fadeOut(aura,1.7f,3f));
        StartCoroutine(MusicFadeIn(bgm[bgmCount],8f));
        yield return StartCoroutine(TypeSentence(content[12],0.22f));
        StartCoroutine(transition(backGrounds[sceneCount],backGrounds[sceneCount+1],2f));
        yield return StartCoroutine(deleteSentence(2f,0.04f));
        yield return StartCoroutine(TypeSentence(content[13]));
        yield return StartCoroutine(deleteSentence(2f,0.02f));
        for(int i =0;i<students.Length;i++)
            StartCoroutine(fadeIn(students[i],1.3f));
        yield return new WaitForSeconds(1f);
        for(int i =0;i<studentsAnimator.Length;i++)
            studentsAnimator[i].SetBool("isMoving",true);
        StartCoroutine(keepCallingGen(outerPoints,studentPoints,14f,0.25f,1f));
        yield return StartCoroutine(TypeSentence(content[14]));
        yield return StartCoroutine(deleteSentence(2f,0.02f));
        yield return StartCoroutine(TypeSentence(content[15]));
        yield return StartCoroutine(deleteSentence(2f,0.02f));
        yield return StartCoroutine(TypeSentence(content[16]));
        yield return StartCoroutine(deleteSentence(2f,0.02f));
        yield return StartCoroutine(TypeSentence(content[17]));
        StartCoroutine(MusicFadeOut(bgm[bgmCount], 4f));
        bgmCount++;
        yield return new WaitForSeconds(0f);
        StartCoroutine(transition(backGrounds[sceneCount],backGrounds[sceneCount+1],1.5f));
        StartCoroutine(MusicFadeIn(bgm[bgmCount],8f));
        yield return new WaitForSeconds(0f);
        for(int i =0;i<students.Length;i++)
            StartCoroutine(fadeOut(students[i],1.3f));
        yield return StartCoroutine(deleteSentence(2f,0.02f));
        yield return StartCoroutine(TypeSentence(content[18]));
        yield return StartCoroutine(deleteSentence(2f,0.02f));
        yield return StartCoroutine(TypeSentence(content[19],0.09f));
        yield return StartCoroutine(deleteSentence(2f,0.02f));
        yield return StartCoroutine(TypeSentence(content[20],0.15f));
        StartCoroutine(transition(backGrounds[sceneCount],backGrounds[sceneCount+1],2f));
        yield return StartCoroutine(deleteSentence(2f,0.02f));
        StartCoroutine(ImageFadeOut(dialogueImages[0],2.1f));
        StartCoroutine(textFadeOut(skip,1.2f));
        for(int i = 1;i<dialogueImages.Count();i++){
            StartCoroutine(ImageFadeOut(dialogueImages[i],1.2f));
        }
        StartCoroutine(MusicFadeOut(bgm[bgmCount], 4f));
        yield return new WaitForSeconds(0f);
        StartCoroutine(MusicFadeIn(bgm[bgmCount], 6f));
        yield return new WaitForSeconds(0f);
        yield return StartCoroutine(startSayMyName());
    }
    IEnumerator transition(Sprite first,Sprite second,float time){
        prior.sprite = first;
        next.sprite = second;
        prior.color = new Color(1f,1f,1f,1f);
        next.color = new Color(1f,1f,1f,0f);
        yield return new WaitForSeconds(2.3f);
        float interval = time/Time.deltaTime;
        for(int i = 0;i<interval;i++){
            prior.color = new Color(prior.color.r,prior.color.g,prior.color.b,1f-i/interval);
            next.color = new Color(next.color.r,next.color.g,next.color.b,i/interval);
            yield return new WaitForSeconds(Time.deltaTime);
        }
        sceneCount++;
    }
    IEnumerator startSayMyName(){
        sayMyName.SetActive(true);
        yield return new WaitForSeconds(2.5f);
        StartCoroutine(fadeIn(sayMyNameSprite[0],2.3f));
        StartCoroutine(ImageFadeIn(penButton,1.5f,0.8f));
        yield return new WaitForSeconds(1.5f);
        sayMyNameManager.inSayMyName();
        sayMyNameInputField.ActivateInputField();
        // if(skipped)
        //     StopAllCoroutines();
    }
    public void skipFunction(){
        StopAllCoroutines();
        dialogueImages[0].color = new Color(dialogueImages[0].color.r,dialogueImages[0].color.g,dialogueImages[0].color.b,0f);
        skip.color = new Color(skip.color.r,skip.color.g,skip.color.b,0f);
        for(int i = 1;i<dialogueImages.Count();i++){
            dialogueImages[i].color = new Color(dialogueImages[i].color.r,dialogueImages[i].color.g,dialogueImages[i].color.b,0f);
        } 
        narration.text = "";
        uncle.color = new Color(1f,1f,1f,0f);
        aura.color = new Color(1f,1f,1f,0f);
        auraAnimator.SetBool("isGlowing",false);
        prior.color = new Color(1f,1f,1f,1f);
        next.color = new Color(1f,1f,1f,0f);
        for(int i = 0;i<students.Length;i++)
            students[i].color = new Color(1f,1f,1f,0f);
        prior.sprite = backGrounds[10];
        for(int i = 0;i<bgm.Count();i++){
            Debug.Log(bgm[i].name);
            bgm[i].volume = 0f;
        }
        bgm[5].Play();
        bgm[5].volume = 1f;
        StartCoroutine(startSayMyName());
        skipped = true;
    }
    IEnumerator fadeIn(SpriteRenderer spriteRenderer,float time, float delay = 0f){
        yield return new WaitForSeconds(delay);
        spriteRenderer.color = new Color(spriteRenderer.color.r,spriteRenderer.color.g,spriteRenderer.color.b,0f);
        float interval = time/Time.deltaTime;
        for(int i = 0;i<interval;i++){
            spriteRenderer.color = new Color(spriteRenderer.color.r,spriteRenderer.color.g,spriteRenderer.color.b,i/interval);
            yield return new WaitForSeconds(Time.deltaTime);
        }
    }
    IEnumerator fadeOut(SpriteRenderer spriteRenderer,float time, float delay = 0f){
        yield return new WaitForSeconds(delay);
        spriteRenderer.color = new Color(spriteRenderer.color.r,spriteRenderer.color.g,spriteRenderer.color.b,1f);
        float interval = time/Time.deltaTime;
        for(int i = 0;i<interval;i++){
            spriteRenderer.color = new Color(spriteRenderer.color.r,spriteRenderer.color.g,spriteRenderer.color.b,1f-i/interval);
            yield return new WaitForSeconds(Time.deltaTime);
        }
    }
    IEnumerator ImageFadeOut(Image image,float time, float delay = 0f){
        image.color = new Color(image.color.r,image.color.g,image.color.b,image.color.a);
        yield return new WaitForSeconds(delay);
        float originalAlpha = image.color.a;
        float interval = time/Time.deltaTime;
        for(int i = 0;i<interval;i++){
            image.color = new Color(image.color.r,image.color.g,image.color.b,originalAlpha-(i/interval*originalAlpha));
            yield return new WaitForSeconds(Time.deltaTime);
        }
    }

    IEnumerator ImageFadeIn(Image image,float time, float delay = 0f){
        yield return new WaitForSeconds(delay);
        image.color = new Color(image.color.r,image.color.g,image.color.b,0f);
        float interval = time/Time.deltaTime;
        for(int i = 0;i<interval;i++){
            image.color = new Color(image.color.r,image.color.g,image.color.b,i/interval);
            yield return new WaitForSeconds(Time.deltaTime);
        }

    }
    IEnumerator textFadeOut(TextMeshProUGUI textMeshPro,float time, float delay = 0f){
        yield return new WaitForSeconds(delay);
        textMeshPro.color = new Color(textMeshPro.color.r,textMeshPro.color.g,textMeshPro.color.b,1f);
        float interval = time/Time.deltaTime;
        for(int i = 0;i<interval;i++){
            textMeshPro.color = new Color(textMeshPro.color.r,textMeshPro.color.g,textMeshPro.color.b,1f-i/interval);
            yield return new WaitForSeconds(Time.deltaTime);
        }
    }
    private void resetSprites(){
        blackScreenObject.SetActive(false);
        dialogueBox.SetActive(true);
        sayMyName.SetActive(false);
        for(int i = 0;i<students.Length;i++){
            students[i].color = new Color(1f,1f,1f,0f);
        }
        uncle.color = new Color(1f,1f,1f,0f);
        aura.color = new Color(1f,1f,1f,0f);
        auraAnimator.SetBool("isGlowing",false);
        prior.sprite = backGrounds[0];
        prior.color = new Color(1f,1f,1f,1f);
        next.sprite = backGrounds[1];
        next.color = new Color(1f,1f,1f,0f);
        for(int i = 0;i< sayMyNameSprite.Count();i++){
            sayMyNameSprite[i].color = new Color(1f,1f,1f,0f);
        }
        penButton.color = new Color(1f,1f,1f,0f);
    }
    IEnumerator TypeSentence (string fullText, float dialogueSpeed = 0.015f){
        narration.text = "";
        for (int i = 0; i < fullText.Length; i++)
        {
            if(fullText[i] == '<') {
                while(fullText[i] != '>') {
                    narration.text += fullText[i];
                    i++;
                }
                narration.text += fullText[i];
            }
            else narration.text += fullText[i];
            yield return new WaitForSeconds(dialogueSpeed);
        }
    }
    IEnumerator deleteSentence(float delay,float speed = 0.02f){
        yield return new WaitForSeconds(delay);
        for(int i =0; i< narration.text.Length;i++){
            if(narration.text[i] == '<') {
                while(narration.text[i] != '>') {
                    narration.text = narration.text.Remove(i,1).Insert(i," ");
                    i++;
                }
                narration.text = narration.text.Remove(i,1).Insert(i," ");
            }
            else
                narration.text = narration.text.Remove(i,1).Insert(i," ");
            yield return new WaitForSeconds(speed);
        }
    }
    IEnumerator keepCallingGen(Transform[] goals,Transform[] spawns,float duration,float frequency = 1f,float delay = 0f){
        yield return new WaitForSeconds(delay);
        while(duration > 0f){
            Transform goal = goals[Random.Range(0,goals.Length)];
            Transform spawn = spawns[Random.Range(0,spawns.Length)];
            generateBike(goal,spawn);
            duration -= frequency;
            yield return new WaitForSeconds(frequency);
        }
    }
    void generateBike(Transform destination, Transform spawnPoint){
        GameObject newBike = Instantiate(bike,spawnPoint.position,spawnPoint.rotation);
        newBike.transform.SetParent(canvas.transform);
        newBike.GetComponent<flyingBike>().setDestination(destination);
    }
    IEnumerator MusicFadeOut(AudioSource audioSource, float time)
    {
        float intervalMusic = time/Time.deltaTime;
        for(int i = 0;i<intervalMusic;i++){
            audioSource.volume = 1f-i/intervalMusic;
            yield return new WaitForSeconds(Time.deltaTime);
        }
    }
    IEnumerator MusicFadeIn(AudioSource audioSource, float duration) 
    {
        audioSource.volume = 0f;
        audioSource.Play();
        float intervalMusic = duration/0.002f;
        for(int j = 0;j<intervalMusic;j++){
            audioSource.volume = j/intervalMusic;
            yield return new WaitForSeconds(0.002f);
        }
    }

}

