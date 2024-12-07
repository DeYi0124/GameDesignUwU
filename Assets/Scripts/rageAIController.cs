using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;
using UnityEngine.SceneManagement;

public class rageAIController : MonoBehaviour
{
   public Animator enemyAnimator;
    public AudioSource[] WAAAA;
    public GameObject vb;
    private float MoveSpeed = 4f;
    private int MaxDist = 10;
    private int MinDist = 20;
    private bool isRaged = false;
    private bool hit = false;
 
    void Start()
    {
        enemyAnimator.SetBool("isWalking", false);
        vb = GameManager.Instance.vb;
        isRaged = false;
    }
    void OnCollisionEnter2D(Collision2D collision2D) {
        if(!hit && collision2D.gameObject.tag == "Car") {
            GameManager.Instance.EnemyCount -= 1;
            if(CarController.Instance.bike > 0) {
                CarController.Instance.bike -= 1;
            }
            if(GameManager.Instance.PR < GameManager.Instance.maxPR)
                GameManager.Instance.PR -= GameManager.Instance.PRfanumTax[0];
            else
                GameManager.Instance.PR -= GameManager.Instance.PRfanumTax[GameManager.Instance.PRlevel];

            GameManager.Instance.kills += 1;
            StartCoroutine(wait(0.5f,true));
        }
    }

    IEnumerator wait(float seconds = 0.5f, bool killed = false) {
        if(killed)
            vb.SetActive(true);
        yield return new WaitForSeconds(seconds);
        vb.SetActive(false);
        Destroy(this.gameObject);
    }

    IEnumerator yell(float seconds = 5f) {
        isRaged = true;
        int audioSoundRng = Random.Range(0, WAAAA.Length);
        WAAAA[audioSoundRng].Play();
        yield return new WaitForSeconds(seconds);
        isRaged = false;
    }

    void Update()
    {
        if(!vb) {
            vb = GameManager.Instance.vb;
        }
        if(GameManager.Instance.getTime() > GameManager.Instance.maxTime) {
            StartCoroutine(wait(1f,false));
        }
        if (!(GameManager.Instance.pause)){
            Vector3 positionDiff = CarController.Instance.transform.position - transform.position;
            var enemySprite = GameObject.Find("BlockingEnemySprite").GetComponent<SpriteRenderer>();
            if(positionDiff.x == 0)
                return;
            enemySprite.flipY = (positionDiff.x < 0);
            transform.right = positionDiff;
            enemyAnimator.SetBool("isSprinting",false);
            enemyAnimator.SetBool("isWalking", false);
            if (Vector3.Distance(transform.position, CarController.Instance.transform.position) < MinDist)
            {
                enemyAnimator.SetBool("isSprinting",false);
                enemyAnimator.SetBool("isWalking",true);
                MoveSpeed = 0.2f;
                transform.position += transform.right * MoveSpeed * Time.deltaTime;
                if (Vector3.Distance(transform.position, CarController.Instance.transform.position) <= MaxDist)
                {
                    if(!isRaged){
                        StartCoroutine(yell());
                    }
                    enemyAnimator.SetBool("isSprinting",true);
                    MoveSpeed = 0.8f;// - Vector3.Distance(transform.position, CarController.Instance.transform.position);
                    transform.position += transform.right * MoveSpeed * Time.deltaTime;
                        //Here Call any function U want Like Shoot at here or something
                }

            }
        }
    }
}
