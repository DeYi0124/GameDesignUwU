using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Networking;
using System.IO;

public class DialogueManager : MonoBehaviour
{

    public Text nameText;
    public Text dialogueText;
    public Animator dialogueBoxAnimator;
    public Animator character1Animator;
    private Queue<string> sentences;
    private Queue<string> names;
    public int id;

    // Start is called before the first frame update
    void Start()
    {  
        LoadBackGround();
        List<int> charNum = new List<int>();
        charNum.Add(2);
        charNum.Add(2);
        LoadCharacters(charNum[id-1]);
        sentences = new Queue<string>();
        names = new Queue<string>();
        dialogueBoxAnimator.SetBool("IsOpen", false);
        character1Animator.SetBool("InScene",false);
    }


    public void StartDialogue (Dialogue dialogue){
        //Debug.Log("starting conversation with "+ dialogue.name);
        dialogueBoxAnimator.SetBool("IsOpen", true);
        character1Animator.SetBool("InScene",true);
        names.Clear();
        sentences.Clear();
        bool isName = true;
        foreach(string sentence in dialogue.content){
            //Debug.Log(sentence);
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

    public void DisplayNextSentence(){ 
        if (sentences.Count == 0 || names.Count == 0){
            EndDialogue();
            return;
        }
        nameText.text = names.Dequeue();
        string sentence = sentences.Dequeue();
        //Debug.Log(sentence);
        //dialogueText.text = sentence;
        StopAllCoroutines();
        StartCoroutine(TypeSentence(sentence));

    }
    IEnumerator TypeSentence (string sentence){
        dialogueText.text = "";
        foreach(char letter in sentence.ToCharArray()){
            dialogueText.text += letter;
            yield return null;
            yield return null;
            yield return null;
            yield return null;
            yield return null;
            yield return null;
        }

    }
    
    void EndDialogue(){
        dialogueBoxAnimator.SetBool("IsOpen", false);
        //Debug.Log("end of conversation.");
    }
    
    private void LoadBackGround(){
        GameObject BackGround = GameObject.Find("BackGround");
        SpriteRenderer bgRenderer = BackGround.GetComponent<SpriteRenderer>();
        string bgPath = Application.streamingAssetsPath + "/BackGround/" + id.ToString() + ".png";
        bgRenderer.sprite = LoadImageFile(bgPath);
        List<Vector3> ScaleList = new List<Vector3>();
        ScaleList.Add(new Vector3(2,1,0));
        ScaleList.Add(new Vector3(8,8,0)); 
        RectTransform transform = BackGround.GetComponent<RectTransform>();
        transform.localScale = ScaleList[id-1];
    }
    private void LoadCharacters(int num){
        for (int i = 1; i<= num; i++){
            LoadCharacter(i);
        }
    }
    private void LoadCharacter(int characterID){
        string characterObject = "character" + characterID.ToString();
        GameObject Character = GameObject.Find(characterObject);
        SpriteRenderer charRenderer = Character.GetComponent<SpriteRenderer>();
        string charPath = Application.streamingAssetsPath + "/Character/" + id.ToString() +'/'+ characterID.ToString() + ".png";
        charRenderer.sprite = LoadImageFile(charPath);
        List<List<Vector3>> ScaleList = new List<List<Vector3>>();
        ScaleList.Add(new List<Vector3>());
        ScaleList[0].Add(new Vector3(0,0,0));
        ScaleList[0].Add(new Vector3(0,0,0));
        ScaleList.Add(new List<Vector3>());
        ScaleList[1].Add(new Vector3(15,15,0));
        ScaleList[1].Add(new Vector3(15,15,0));

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
        //Debug.Log(id.ToString()+"sprite");
        return sprite;


    }
}
