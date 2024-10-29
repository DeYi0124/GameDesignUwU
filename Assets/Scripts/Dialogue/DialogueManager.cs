using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Networking;
using System.IO;
using TMPro;
using System.Linq;
using UnityEngine.SceneManagement;


public class DialogueManager : MonoBehaviour
{

    public TextMeshProUGUI nameText;
    public TextMeshProUGUI dialogueText;
    public Animator dialogueBoxAnimator;
    public Animator character1Animator;
    public Animator character2Animator;
    public Animator character3Animator;
    public Animator character4Animator;
    public Animator character5Animator;
    public Animator character6Animator;
    public int id;
    public Dialogue dialogue;
    private List<string> dialogueContent;
    private Queue<string> sentences;
    private Queue<string> names;
    private string[] speakerID;
    private int currentSpeaker = 0;
    private int previousSpeaker = 0;
    private bool OddFirstPerson = true;
    private bool EvenFirstPerson = true;
    private float dialogueSpeed = 0.025f;
    private bool doneReading = false;


    // Start is called before the first frame update
    void Start()
    {
        Vector3 farFarAway = new Vector3(1000,1000,0);
        var car = GameObject.FindWithTag("Car").GetComponent<Transform>();
        car.position = farFarAway;

        id = checkPointGen.rng;
        dialogue = new Dialogue();
        dialogue.content = new List<string>();
        dialogueContent = new List<string>();
        sentences = new Queue<string>();
        names = new Queue<string>();
        speakerID = new string[7];
        StartCoroutine(LoadBackGround());
        List<int> charNum = new List<int>();
        charNum.Add(2);
        charNum.Add(2);
        charNum.Add(6);
        LoadCharacters(charNum[id-1]);
        dialogueBoxAnimator.SetBool("IsOpen", false);
        for(int i = 1;i<7;i++){
            TalkingCurrentSpeaker(i,false);
            InSceneCurrentSpeaker(i,false);
        }
        StartCoroutine(ReadDialogueFile());
    }


    IEnumerator ReadDialogueFile(){
        string path = Application.streamingAssetsPath + "/Dialogue/" + id.ToString() + ".txt";
        if (path.Contains("://") || path.Contains(":///"))
        {
            UnityWebRequest www = UnityWebRequest.Get(path);
            www.downloadHandler = new DownloadHandlerBuffer();
            yield return www.SendWebRequest();
            string tmpDialogueContent = www.downloadHandler.text;
            dialogueContent = tmpDialogueContent.Split('\n').ToList();
            doneReading = true;
        }
        else
        {   
            yield return null;
            dialogueContent = File.ReadAllLines(path).ToList();
            doneReading = true;
        }
        TriggerDialogue();
        
    }
    public void TriggerDialogue(){
        for( int i = 0;i < dialogueContent.Count; i++){
            dialogue.content.Add(dialogueContent[i]);
        }
        StartDialogue(dialogue);
    }
    public void StartDialogue (Dialogue dialogue){
        
        dialogueBoxAnimator.SetBool("IsOpen", true);
        names.Clear();
        sentences.Clear();
        bool isName = true;
        if(doneReading){
            speakerID = dialogue.content[0].Trim().Split(' ');
            dialogue.content.RemoveAt(0);
            foreach(string sentence in dialogue.content){
                if(isName){
                    names.Enqueue(sentence);
                    isName = false;
                }
                else{
                    sentences.Enqueue(sentence);
                    isName = true;
                }
            }

            DisplayNextSentence();
        }

    }

    public void DisplayNextSentence(){

        if (sentences.Count == 0 || names.Count == 0){
            EndDialogue();
            return;
        }
        for(int i = 0;i< speakerID.Length;i++){
            if(names.Peek() == speakerID[i]){
                previousSpeaker = currentSpeaker;
                currentSpeaker = (i+1);
                break;
            }
            else if(i == speakerID.Length-1){
                previousSpeaker = currentSpeaker;
                currentSpeaker = 0;
            }
        }

        nameText.text = names.Dequeue();

        if((currentSpeaker == 1 || currentSpeaker == 3|| currentSpeaker == 5)&& OddFirstPerson){
            InSceneCurrentSpeaker(currentSpeaker,true);
            OddFirstPerson = false;
        }
        if((currentSpeaker == 2 || currentSpeaker == 4|| currentSpeaker == 6)&& EvenFirstPerson){
            InSceneCurrentSpeaker(currentSpeaker,true);
            EvenFirstPerson = false;
        }
        TalkingCurrentSpeaker(KickSpeaker(currentSpeaker),false);
        InSceneCurrentSpeaker(KickSpeaker(currentSpeaker),false);
        InSceneCurrentSpeaker(currentSpeaker,true);
        TalkingCurrentSpeaker(previousSpeaker,false);
        TalkingCurrentSpeaker(currentSpeaker,true);
        string sentence = sentences.Dequeue();
        StopAllCoroutines();
        StartCoroutine(TypeSentence(sentence));

    }
    IEnumerator TypeSentence (string sentence){
        dialogueText.text = "";
        foreach(char letter in sentence.ToCharArray()){
            dialogueText.text += letter;
            yield return new WaitForSeconds(dialogueSpeed);
        }

    }

    void EndDialogue(){
        dialogueBoxAnimator.SetBool("IsOpen", false);
        for (int i = 1;i< 7;i++){
            TalkingCurrentSpeaker(i,false);
            InSceneCurrentSpeaker(i,false);
        }
        SceneManager.LoadScene("RPS game");
    }

    private IEnumerator LoadBackGround(){
        GameObject BackGround = GameObject.Find("BackGround");
        SpriteRenderer bgRenderer = BackGround.GetComponent<SpriteRenderer>();
        string bgPath = Application.streamingAssetsPath + "/BackGround/" + id.ToString() + ".png";
        if (bgPath.Contains("://") || bgPath.Contains(":///"))
        {
            byte[] imgData;
            Texture2D tex = new Texture2D(2, 2);
            UnityWebRequest www = UnityWebRequest.Get(bgPath);
            yield return www.SendWebRequest();
            imgData = www.downloadHandler.data;
            tex.LoadImage(imgData);
            Vector2 pivot = new Vector2(0.5f, 0.5f);
            Sprite sprite = Sprite.Create(tex, new Rect(0.0f, 0.0f, tex.width, tex.height), pivot, 100.0f);
            bgRenderer.sprite = sprite;
        }
        else
        {
            yield return null;
            bgRenderer.sprite = LoadImageFile(bgPath);
        }
        
        List<Vector3> ScaleList = new List<Vector3>();
        ScaleList.Add(new Vector3(2,1,0));
        ScaleList.Add(new Vector3(8,8,0));
        ScaleList.Add(new Vector3(9,9,0));
        RectTransform transform = BackGround.GetComponent<RectTransform>();
        transform.localScale = ScaleList[id-1];
    }
    private void LoadCharacters(int num){
        for (int i = 1; i<= num; i++){
            StartCoroutine(LoadCharacter(i));
        }
    }
    private IEnumerator LoadCharacter(int characterID){
        string characterObject = "character" + characterID.ToString();
        GameObject Character = GameObject.Find(characterObject);
        SpriteRenderer charRenderer = Character.GetComponent<SpriteRenderer>();
        string charPath = Application.streamingAssetsPath + "/Character/" + id.ToString() +'/'+ characterID.ToString() + ".png";
        if (charPath.Contains("://") || charPath.Contains(":///"))
        {
            byte[] imgData;
            Texture2D tex = new Texture2D(2, 2);
            UnityWebRequest www = UnityWebRequest.Get(charPath);
            yield return www.SendWebRequest();
            imgData = www.downloadHandler.data;
            tex.LoadImage(imgData);
            Vector2 pivot = new Vector2(0.5f, 0.5f);
            Sprite sprite = Sprite.Create(tex, new Rect(0.0f, 0.0f, tex.width, tex.height), pivot, 100.0f);
            charRenderer.sprite = sprite;
        }
        else
        {
            yield return null;
            charRenderer.sprite = LoadImageFile(charPath);
        }
        List<List<Vector3>> ScaleList = new List<List<Vector3>>();
        ScaleList.Add(new List<Vector3>());
        ScaleList[0].Add(new Vector3(0,0,0));
        ScaleList[0].Add(new Vector3(0,0,0));
        ScaleList.Add(new List<Vector3>());
        ScaleList[1].Add(new Vector3(15,15,0));
        ScaleList[1].Add(new Vector3(15,15,0));
        ScaleList.Add(new List<Vector3>());
        ScaleList[2].Add(new Vector3(15,15,0));
        ScaleList[2].Add(new Vector3(15,15,0));
        ScaleList[2].Add(new Vector3(15,15,0));
        ScaleList[2].Add(new Vector3(15,15,0));
        ScaleList[2].Add(new Vector3(30,30,0));
        ScaleList[2].Add(new Vector3(15,15,0));

        RectTransform transform = Character.GetComponent<RectTransform>();
        transform.localScale = ScaleList[id-1][characterID-1];

    }

    private Sprite LoadImageFile(string path){
        byte[] imgData;
        Texture2D tex = new Texture2D(2, 2);
        imgData = File.ReadAllBytes(path);
        tex.LoadImage(imgData);
        Vector2 pivot = new Vector2(0.5f, 0.5f);
        Sprite sprite = Sprite.Create(tex, new Rect(0.0f, 0.0f, tex.width, tex.height), pivot, 100.0f);
        return sprite;
    }
    private void TalkingCurrentSpeaker(int currentSpeaker, bool flag){
        switch (currentSpeaker){
            case 1:
                character1Animator.SetBool("IsTalking",flag);
                break;
            case 2:
                character2Animator.SetBool("IsTalking",flag);
                break;
            case 3:
                character3Animator.SetBool("IsTalking",flag);
                break;
            case 4:
                character4Animator.SetBool("IsTalking",flag);
                break;
            case 5:
                character5Animator.SetBool("IsTalking",flag);
                break;
            case 6:
                character6Animator.SetBool("IsTalking",flag);
                break;
            default:
                break;
        }
    }
    private void InSceneCurrentSpeaker(int currentSpeaker, bool flag){
        switch (currentSpeaker){
            case 1:
                character1Animator.SetBool("InScene",flag);
                break;
            case 2:
                character2Animator.SetBool("InScene",flag);
                break;
            case 3:
                character3Animator.SetBool("InScene",flag);
                break;
            case 4:
                character4Animator.SetBool("InScene",flag);
                break;
            case 5:
                character5Animator.SetBool("InScene",flag);
                break;
            case 6:
                character6Animator.SetBool("InScene",flag);
                break;
            default:
                break;
        }
    }
    private int KickSpeaker(int current){
        switch(current){
            case 1:
                if(character1Animator.GetBool("InScene"))
                    return 0;
                else if(character3Animator.GetBool("InScene"))
                    return 3;
                else if(character5Animator.GetBool("InScene"))
                    return 5;
                else
                    return 0;
            case 2:
                if(character2Animator.GetBool("InScene"))
                    return 0;
                else if(character4Animator.GetBool("InScene"))
                    return 4;
                else if(character6Animator.GetBool("InScene"))
                    return 6;
                else
                    return 0;
            case 3:
                if(character3Animator.GetBool("InScene"))
                    return 0;
                else if(character1Animator.GetBool("InScene"))
                    return 1;
                else if(character5Animator.GetBool("InScene"))
                    return 5;
                else
                    return 0;
            case 4:
                if(character4Animator.GetBool("InScene"))
                    return 0;
                else if(character2Animator.GetBool("InScene"))
                    return 2;
                else if(character6Animator.GetBool("InScene"))
                    return 6;
                else
                    return 0;
            case 5:
                if(character5Animator.GetBool("InScene"))
                    return 0;
                else if(character1Animator.GetBool("InScene"))
                    return 1;
                else if(character3Animator.GetBool("InScene"))
                    return 3;
                else
                    return 0;
            case 6:
                if(character6Animator.GetBool("InScene"))
                    return 0;
                else if(character2Animator.GetBool("InScene"))
                    return 2;
                else if(character4Animator.GetBool("InScene"))
                    return 4;
                else
                    return 0;
            default:
                return 0;
        }
    }
}
