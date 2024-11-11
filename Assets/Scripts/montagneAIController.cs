using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class montagneAIController : MonoBehaviour
{
    public Animator enemyAnimator;
    private int MoveSpeed = 4;
    private int outerDist = 15;
    private int innerDist = 13;
    private bool inside = false;
    private int previousDay = 0;
    void Start()
    {
        enemyAnimator.SetBool("isWalking", false);
    }

    void Update()
    {
        
        if(GameManager.Instance.getTime() != 60){
            Destroy(this.gameObject);
        }
        if(!(GameManager.Instance.pause)){
            Vector3 positionDiff = CarController.Instance.transform.position - transform.position;
            var enemySprite = GameObject.Find("BlockingEnemySprite").GetComponent<SpriteRenderer>();
            if(positionDiff.x == 0)
                return;
            enemySprite.flipY = (positionDiff.x < 0);
            transform.right = positionDiff;

            if (Vector3.Distance(transform.position, CarController.Instance.transform.position) >= outerDist)
            {
                enemyAnimator.SetBool("isSpinning", false);
                enemyAnimator.SetBool("isWalking",true);
                MoveSpeed = 1;
                transform.position += transform.right * MoveSpeed * Time.deltaTime;
            }
            else{
                enemyAnimator.SetBool("isWalking",false);
                enemyAnimator.SetBool("isSpinning",true);
            }
        }

    }
}
