using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;
using UnityEngine.UI;
public class AIController : MonoBehaviour
{

    public Animator enemyAnimator;
    public AudioSource WAAAA;
    private int MoveSpeed = 4;
    private int MaxDist = 10;
    private int MinDist = 6;
    private bool hit = false;
    void Start()
    {
        enemyAnimator.SetBool("isWalking", false);
    }
    void OnCollisionEnter2D(Collision2D collision2D) {
        if(!hit && collision2D.gameObject.tag == "Car") {
            hit = true;
            GameManager.Instance.kills += 1;
            if(GameManager.Instance.PR < GameManager.Instance.maxPR)
                GameManager.Instance.PR -= GameManager.Instance.PRfanumTax[0];
            else
                GameManager.Instance.PR -= GameManager.Instance.PRfanumTax[GameManager.Instance.PRlevel];
            GameManager.Instance.EnemyCount -= 1;
            StartCoroutine(wait());
        }
    }

    IEnumerator wait(float seconds = 1.5f) {
        WAAAA.Play();
        // Debug.Log("Killed");
        yield return new WaitForSeconds(seconds);
        Destroy(this.gameObject);
    }
    void Update()
    {
        if(GameManager.Instance.getTime() > GameManager.Instance.maxTime) {
            StartCoroutine(wait(1f));
        }
        if (!(GameManager.Instance.pause)){
            Vector3 positionDiff = CarController.Instance.transform.position - transform.position;
            var enemySprite = GameObject.Find("ChasingEnemySprite").GetComponent<SpriteRenderer>();
            if(positionDiff.x == 0)
                return;
            enemySprite.flipY = (positionDiff.x < 0);
            transform.right = positionDiff;
            enemyAnimator.SetBool("isSprinting",false);
            enemyAnimator.SetBool("isWalking", false);
            if (Vector3.Distance(transform.position, CarController.Instance.transform.position) >= MinDist)
            {
                enemyAnimator.SetBool("isSprinting",false);
                enemyAnimator.SetBool("isWalking",true);
                MoveSpeed = 1;
                transform.position += transform.right * MoveSpeed * Time.deltaTime;
                if (Vector3.Distance(transform.position, CarController.Instance.transform.position) <= MaxDist)
                {
                    enemyAnimator.SetBool("isSprinting",true);
                    MoveSpeed = 2;// - Vector3.Distance(transform.position, CarController.Instance.transform.position);
                    transform.position += transform.right * MoveSpeed * Time.deltaTime;
                        //Here Call any function U want Like Shoot at here or something
                }

            }
        }
    }
}
