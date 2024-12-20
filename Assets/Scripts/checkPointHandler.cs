using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using TMPro;
using Random = UnityEngine.Random;

public class checkPointHandler : MonoBehaviour
{
    private bool isWorking = false;
    private float progress = 1f;
    private SpriteRenderer m_SpriteRenderer;
    private TextMeshPro m_TextMeshPro;
    private int bikePerPoint;
    private float MoveSpeed = 4f;
    private int MaxDist = 0;
    private int MinDist = 11;


    public event Action<checkPointHandler> OnReceiving;
    public int id;
    public Transform customPivot;
    public Sprite[] bikeSprite;


    //private float timer = 0;

    void OnTriggerEnter2D(Collider2D collider2D) {
        if(progress != 0 && collider2D.CompareTag("CarCollider"))
            isWorking = true;
    }

    void OnTriggerExit2D(Collider2D collider2D) {
        if(progress != 0 && collider2D.CompareTag("CarCollider"))
            isWorking = false;
    }

    public float getProgress() {
        return progress;
    }

    void Awake()
    {
        m_SpriteRenderer = GetComponent<SpriteRenderer>();
        int rng = Random.Range(0,bikeSprite.Length);
        m_SpriteRenderer.sprite = bikeSprite[rng];
        m_SpriteRenderer.color = new Color (255, 255, 255, progress);
        GameObject tmp = this.gameObject.transform.GetChild(0).gameObject;
        tmp.SetActive(true);
        tmp.GetComponent<TextMeshPro>().text = bikePerPoint.ToString();


    }

    void Update()
    {
        if(GameManager.Instance.PR >= 100 && !GameManager.Instance.pause) {
            MinDist = GameManager.Instance.PRaura[GameManager.Instance.PRlevel];
            MoveSpeed = GameManager.Instance.PRspeed[GameManager.Instance.PRlevel];
            Vector3 positionDiff = CarController.Instance.transform.position - transform.position;
            var enemySprite = this.gameObject.GetComponent<SpriteRenderer>();
            GameObject textObject = this.gameObject.transform.GetChild(0).gameObject;

            if (Vector3.Distance(transform.position, CarController.Instance.transform.position) < MinDist)
            {
                if(positionDiff.x == 0)
                    return;
                Vector3 positionForLHS = new Vector3(-0.2f,0.8f,0);
                Vector3 scaleForLHS = new Vector3(-0.4f,-0.4f,0);
                Vector3 positionForRHS = new Vector3(0.2f,-0.8f,0);
                Vector3 scaleForRHS = new Vector3(0.4f,0.4f,0);
                if (positionDiff.x < 0){
                    textObject.transform.localPosition = positionForLHS;
                    textObject.GetComponent<RectTransform>().localScale = scaleForLHS;
                }
                else{
                    textObject.transform.localPosition = positionForRHS;
                    textObject.GetComponent<RectTransform>().localScale = scaleForRHS;
                }
                enemySprite.flipY = (positionDiff.x < 0);
                enemySprite.flipX = (positionDiff.x < 0);

                transform.right = positionDiff;
                transform.position += transform.right * MoveSpeed * Time.deltaTime;
            }
        }

        if(!GameManager.Instance.pause && isWorking && CarController.Instance.bike < CarController.Instance.maxBike) {
            progress -= (0.33f*(GameManager.Instance.skillLevel+1)*Time.deltaTime);
            progress = Mathf.Clamp01(progress);
            m_SpriteRenderer.color = new Color (255, 255, 255, progress);
            if (progress == 0) {
                CarController.Instance.bike+= bikePerPoint;
                progress = -1;
                OnReceiving?.Invoke(this);
                Destroy(transform.gameObject);
            }
        }
        if(GameManager.Instance.getTime() > GameManager.Instance.maxTime) {
            StartCoroutine(wait(1));
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
