using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using TMPro;

public class checkPointHandler : MonoBehaviour
{
    private bool isWorking = false;
    private float progress = 1f;
    private SpriteRenderer m_SpriteRenderer;
    private TextMeshPro m_TextMeshPro;
    private int bikePerPoint;
    public event Action<checkPointHandler> OnReceiving;
    public int id;
    
    //private float timer = 0;

    void OnTriggerEnter2D(Collider2D collider2D) {
        if(progress != 0 && collider2D.CompareTag("Car"))
            isWorking = true;
    }

    void OnTriggerExit2D(Collider2D collider2D) {
        if(progress != 0 && collider2D.CompareTag("Car"))
            isWorking = false;
    }

    public float getProgress() {
        return progress;
    }

    void Awake()
    {
        m_SpriteRenderer = GetComponent<SpriteRenderer>();
        m_SpriteRenderer.color = new Color (1, 0, 0, progress);
        GameObject tmp = this.gameObject.transform.GetChild(0).gameObject; 
        tmp.SetActive(true);
        tmp.GetComponent<TextMeshPro>().text = bikePerPoint.ToString();


    }

    void Update()
    {
        // Gradually increase the red paint height
        //timer += Time.deltaTime;
　　    //Debug.Log(timer);
        if(isWorking && CarController.Instance.bike < CarController.Instance.maxBike) {
            progress -= ((GameManager.Instance.skillLevel+1)*Time.deltaTime);
            progress = Mathf.Clamp01(progress);  
            m_SpriteRenderer.color = new Color (1, 0, 0, progress);
            if (progress == 0) {
                CarController.Instance.bike+= bikePerPoint;
                progress = -1;
                OnReceiving?.Invoke(this);
                Destroy(transform.gameObject);
            }
        }
        if(GameManager.Instance.getTime() > 60) {
            StartCoroutine(wait(3));
        }
    }
    public void setBikePerPoint(int i){
        bikePerPoint = i;
    }
    public int getBikePerPoint(){
        return bikePerPoint;
    }
    IEnumerator wait(float seconds = 0.5f) {
        yield return new WaitForSeconds(seconds);
        Destroy(this.gameObject);
    }
}
