using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Card : MonoBehaviour
{
    private SpriteRenderer rend;

    [SerializeField]
    private Sprite[] faceSprites;
    [SerializeField]
    private Sprite backSprite;
    private int cardID;

    private bool coroutineAllowed, facedUp;

    // Start is called before the first frame update
    void Start()
    {
        rend = GetComponent<SpriteRenderer>();
        rend.sprite = backSprite;
        coroutineAllowed = true;
        facedUp = false;
    }
    
    void Update(){
        if(Time.time >= 2f){
            if (coroutineAllowed)
            {
                // StartCoroutine(RotateCard());
            }
        }
    }
    public void setCardID(int id){
        cardID = id;
    }
    public void rotateCard(){
        StartCoroutine(RotateCard());
    }
    private IEnumerator RotateCard()
    {
        coroutineAllowed = false;

        if (!facedUp)
        {
            for (float i = 0f; i <= 180f; i += 10f)
            {
                transform.rotation = Quaternion.Euler(0f, i, 0f);
                if (i == 90f)
                {
                    rend.sprite = faceSprites[cardID];
                    rend.flipX = true;

                }
                yield return new WaitForSeconds(0.01f);
            }
        }

        else if (facedUp)
        {
            for (float i = 180f; i >= 0f; i -= 10f)
            {
                transform.rotation = Quaternion.Euler(0f, i, 0f);
                if (i == 90f)
                {
                    rend.sprite = backSprite;
                }
                yield return new WaitForSeconds(0.01f);
            }
        }

        coroutineAllowed = true;

        facedUp = !facedUp;
    }
}

