using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;
using System.Linq;

public class DialogueTrigger : MonoBehaviour
{
    public int id;
    public Dialogue dialogue;

    List<string> ReadDialogueFile(int id){
        string path = Application.streamingAssetsPath + "/Dialogue/" + id.ToString() + ".txt";
        List<string> DialogueContent = File.ReadAllLines(path).ToList();
        return DialogueContent;
    }
    public void TriggerDialogue(){
        List<string> dialogueContent = ReadDialogueFile(id);
        for( int i = 0;i < dialogueContent.Count; i++){
            dialogue.content.Add(dialogueContent[i]);
        }

        
        FindObjectOfType<DialogueManager>().StartDialogue(dialogue);
    }
}