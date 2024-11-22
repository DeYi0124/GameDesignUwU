using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using Random = UnityEngine.Random;
using System.Linq;
using System;

public class Enemy : MonoBehaviour
{
    public Sprite[] enemySprite;
    public GameObject ManagerObject;
    public DaManager manager;
    public GameObject car;
    public Animator enemySpriteAnimator;
    public Animator enemyTextAnimator;

    private Vector3 path;
    private string wordText = "";
    private float speed = 0.1f;
    private bool didDamage = false;
    private string cock = "";
    // Start is called before the first frame update
    void Start()
    {
    }
    void Awake(){
        ManagerObject = GameObject.Find("DaManager");
        manager = ManagerObject.GetComponent<DaManager>();
        Debug.Log(manager.getWordCount());
        wordText = manager.getWord(Random.Range(0,manager.getWordCount()));
        car = GameObject.Find("car");
        this.transform.GetChild(0).GetComponent<SpriteRenderer>().sprite = enemySprite[Random.Range(0,enemySprite.Length)];
        this.transform.GetChild(1).GetChild(0).GetComponent<TextMeshProUGUI>().text = wordText;
        path = car.transform.position - this.transform.position;

    }
    // Update is called once per frame
    void Update()
    {
        if(manager.getTime() >= 20)
            speed = 0.125f;
        else if (manager.getTime() >= 10)
            speed = 0.1f;
        else if (manager.getTime() >= 0)
            speed = 0.075f;
        this.transform.position += path * speed * Time.deltaTime;
        if (this.transform.localPosition.y < -200 && !didDamage){
            manager.minusHP();
            didDamage = true;
            Destroy(this.gameObject);
        }
        if (manager.getSubmission() == wordText){
            selfDestruct();
            manager.TypedWord.color = Color.green;
            StartCoroutine(waitAndClear(0.5f));
        }
    }
    private void selfDestruct(){
        enemySpriteAnimator.SetBool("isDestroyed", true);
        enemyTextAnimator.SetBool("isDestroyed", true);
    }
    IEnumerator waitAndClear(float time){
        yield return new WaitForSeconds(time);
        manager.clearTextBox();
        manager.TypedWord.color = Color.white;
        Destroy(this.gameObject);
    }

}
