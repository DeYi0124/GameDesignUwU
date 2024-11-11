using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
public class AIController : MonoBehaviour
{

    public Animator enemyAnimator;
    private int MoveSpeed = 4;
    private int MaxDist = 10;
    private int MinDist = 2;

    void Start()
    {
        enemyAnimator.SetBool("isWalking", false);
    }

    void Update()
    {
        if (!(GameManager.Instance.pause)){ 
            Vector3 positionDiff = CarController.Instance.transform.position - transform.position;
            bool flipped = false;
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