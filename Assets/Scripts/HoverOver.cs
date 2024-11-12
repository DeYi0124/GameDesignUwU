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
        explanationText.Add(0, "Increase the capacity of your beautiful ShueiYuan truck");
        explanationText.Add(1, "Increase the speed of your beautiful ShueiYuan truck, this upgrade may make the truck harder to drive, so be careful!");
        explanationText.Add(2, "Increase the size of the oil tank of your beautiful ShueiYuan truck");
        explanationText.Add(3, "Increase the efficiency of your beautiful ShueiYuan truck, shortening the time required to take bikes");
        explanationText.Add(4, "Increase the average number of bikes per point");
        // explanation.SetActive(true);
        text.text = explanationText[explanationID];
    }
    
    public void OnPointerExit(PointerEventData eventData){

        explanation.SetActive(false);
    }

}
