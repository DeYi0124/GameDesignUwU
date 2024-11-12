using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using TMPro;

public class HoverOver : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
{   

    public int explanationID;
    public GameObject explanation;
    public TextMeshProUGUI text;
    private Dictionary<int, string> explanationText = new Dictionary<int, string>();
    
    public void OnPointerEnter(PointerEventData eventData){
        Dictionary<int, string> explanationText = new Dictionary<int, string>();
        explanationText.Add(0, "This is the first explanation");
        explanationText.Add(1, "This is the second explanation");
        explanationText.Add(2, "This is the third explanation");
        explanationText.Add(3, "This is the fourth explanation");
        explanationText.Add(4, "This is the fifth explanation");
        explanation.SetActive(true);
        text.text = explanationText[explanationID];
    }
    
    public void OnPointerExit(PointerEventData eventData){

        explanation.SetActive(false);
    }

}
