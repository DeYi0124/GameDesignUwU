using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class DialogueManager : MonoBehaviour
{

    public Text nameText;
    public Text dialogueText;
    public Animator animator;
    private Queue<string> sentences;

    // Start is called before the first frame update
    void Start()
    {  
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
    
}
