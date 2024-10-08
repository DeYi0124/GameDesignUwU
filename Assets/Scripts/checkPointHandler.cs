using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class checkPointHandler : MonoBehaviour
{
    bool isWorking = false;
    float progress = 1f;
    SpriteRenderer m_SpriteRenderer;
    
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
    }

    void Update()
    {
        // Gradually increase the red paint height
        //timer += Time.deltaTime;
　　    //Debug.Log(timer);
        if(isWorking) {
            progress -= Time.deltaTime;
            progress = Mathf.Clamp01(progress);  
            m_SpriteRenderer.color = new Color (1, 0, 0, progress);
        }
        
    }
}
