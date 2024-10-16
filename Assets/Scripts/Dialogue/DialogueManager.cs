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
    public Animator animator;
    private Queue<string> sentences;
    public int id;

    // Start is called before the first frame update
    void Start()
    {  
        Debug.Log(id);
        GameObject BackGround = GameObject.Find("BackGround");
        SpriteRenderer renderer = BackGround.GetComponent<SpriteRenderer>();
        renderer.sprite = LoadImageFile(id);
        List<Vector3> ScaleList = new List<Vector3>();
        ScaleList.Add(new Vector3(2,1,0));
        ScaleList.Add(new Vector3(2,1,0)); 
        RectTransform transform = BackGround.GetComponent<RectTransform>();
        transform.localScale = ScaleList[id-1];
        sentences = new Queue<string>();
        animator.SetBool("IsOpen", false);
    }


    public void StartDialogue (Dialogue dialogue){
        //Debug.Log("starting conversation with "+ dialogue.name);
        animator.SetBool("IsOpen", true);
        nameText.text = dialogue.name;
        
        sentences.Clear();

        foreach(string sentence in dialogue.sentences){
            sentences.Enqueue(sentence);
        }

        DisplayNextSentence();
    }

    public void DisplayNextSentence(){
        if (sentences.Count == 0){
            EndDialogue();
            return;
        }
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
        animator.SetBool("IsOpen", false);
        //Debug.Log("end of conversation.");
    }
    
    private Sprite LoadImageFile(int id){
        string path = Application.streamingAssetsPath + "/BackGround/" + id.ToString() + ".png";
        byte[] imgData;
        Texture2D tex = new Texture2D(2, 2);
        imgData = File.ReadAllBytes(path);
        tex.LoadImage(imgData);
        Vector2 pivot = new Vector2(0.5f, 0.5f);
        Sprite sprite = Sprite.Create(tex, new Rect(0.0f, 0.0f, tex.width, tex.height), pivot, 100.0f);
        Debug.Log(id.ToString()+"sprite");
        return sprite;


    }
}
