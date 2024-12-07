using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class testManager : MonoBehaviour
{
    public SpriteRenderer prior;
    public SpriteRenderer next;
    public TextMeshProUGUI text;
    public TextAsset bla;
    void Start(){
        text.text = bla.text;
        StartCoroutine(transition(prior.sprite,next.sprite,3f));

    }
    IEnumerator transition(Sprite first,Sprite second,float time){
        prior.color = new Color(1f,1f,1f,1f);
        next.color = new Color(1f,1f,1f,0f);
        yield return new WaitForSeconds(3f);
        prior.sprite = first;
        next.sprite = second;
        float interval = time/Time.deltaTime;
        for(int i = 0;i<interval;i++){
            prior.color = new Color(prior.color.r,prior.color.g,prior.color.b,1f-i/interval);
            next.color = new Color(next.color.r,next.color.g,next.color.b,i/interval);
            yield return new WaitForSeconds(Time.deltaTime);
        }
    }
}
