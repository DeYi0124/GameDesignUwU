using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using Random = UnityEngine.Random;
using System.Linq;
using System;


public class diezYMediaManager : MonoBehaviour
{
    public GameObject Canvas;
    public Transform deck;
    public Transform[] playerOnePosition;
    public Transform[] playerTwoPosition;
    public GameObject card;
    public TextMeshProUGUI result;
    public TextMeshProUGUI playerOnePoint;
    public TextMeshProUGUI playerTwoPoint;
    private int cardCountPlayerOne = 0;
    private int cardCountPlayerTwo = 0;
    private int pointPlayerOne = 0;
    private int pointPlayerTwo= 0;
    private List<GameObject> totalCard = new List<GameObject>();
    private List<int> deckCard = new List<int>();
    private List<int> playerOneCard = new List<int>();
    private List<int> playerTwoCard = new List<int>();
    private bool lost = false;
    private bool won = false;
    private bool isHouseTurn = false;
    private bool houseIsDone = false;
    // Start is called before the first frame update
    void Start()
    {
        for(int i = 0;i<52;i++){
            deckCard.Add(i);
        }
        playerOnePoint.text = "?";
        playerTwoPoint.text = "?";
        generateCard(1,playerOnePosition[cardCountPlayerOne]);
        generateCard(2,playerTwoPosition[cardCountPlayerTwo]);
    }
    void Update(){
    }
    private void generateCard(int whoseCard, Transform whereToGo){
        int tmpRng = Random.Range(0, deckCard.Count);
        int rng = deckCard[tmpRng];
        deckCard.Remove(rng);
        totalCard.Add(Instantiate(card, deck.position, deck.rotation));
        totalCard[cardCountPlayerOne+cardCountPlayerTwo].GetComponent<Card>().setCardID(rng);
        totalCard[cardCountPlayerOne+cardCountPlayerTwo].transform.SetParent(Canvas.transform);
        totalCard[cardCountPlayerOne+cardCountPlayerTwo].transform.localScale = new Vector3(15,15,0);
        if(cardCountPlayerOne+cardCountPlayerTwo == 1)
            StartCoroutine(moveCardWithoutFlipping(totalCard[cardCountPlayerOne+cardCountPlayerTwo], whereToGo));
        else
            StartCoroutine(moveCard(totalCard[cardCountPlayerOne+cardCountPlayerTwo], whereToGo));
        int point = rng%13+1;
        if (whoseCard == 1){
            cardCountPlayerOne++;
            playerOneCard.Add(point);
        }
        else{
            cardCountPlayerTwo++;
            playerTwoCard.Add(point);
        }


    }
    public void call(){
        generateCard(1,playerOnePosition[cardCountPlayerOne]);
        if(calculatePoint(playerOneCard)>21){
            lost = true;
        }
    }
    public void pass(){
        totalCard[1].GetComponent<Card>().rotateCard();
        StartCoroutine(houseTurn());
        isHouseTurn = true;
    }
    private IEnumerator houseTurn(){
        for(int i = 0;i<4;i++){
            if(calculatePoint(playerTwoCard)<17){
                generateCard(2,playerTwoPosition[cardCountPlayerTwo]);
                yield return new WaitForSeconds(2f);
            }
            else{
                houseIsDone = true;
                StartCoroutine(judge());
                break;
            }
        }
    }

    private int calculatePoint(List<int> cardList){
        int point = 0;
        int aceCount = 0;
        for(int i = 0;i<cardList.Count;i++){
            if(cardList[i]>10)
                point+=10;
            else if(cardList[i]==1){
                aceCount++;
                point+=11;
            }
            else{
                point+=cardList[i];
            }
        }
        for(int i = 0;i<aceCount;i++){
            if(point>21){
                point-=10;
            }
        }
        return point;
    }
    IEnumerator moveCard(GameObject card, Transform destination){
        Vector3 direction = destination.position - card.transform.position;
        float ETA = 0.3f;
        for (int i = 0; i < 300; i++){
            card.transform.Translate(direction / 300f);
            yield return new WaitForSeconds(ETA/300f);
        }
        card.GetComponent<Card>().rotateCard();
        StartCoroutine(judge());
    }
    IEnumerator judge(){
        if(calculatePoint(playerOneCard)>21){
            yield return new WaitForSeconds(0.5f);
            lost = true;
        }
        else if(calculatePoint(playerTwoCard)>21 || playerOneCard.Count == 5 || calculatePoint(playerOneCard) == 21){
            yield return new WaitForSeconds(0.5f);
            won = true;
        }
        else if(isHouseTurn && houseIsDone && calculatePoint(playerOneCard)>calculatePoint(playerTwoCard)){
            yield return new WaitForSeconds(0.5f);
            won = true;
        }
        else if(isHouseTurn && calculatePoint(playerOneCard)<=calculatePoint(playerTwoCard) || playerTwoCard.Count == 5){
            yield return new WaitForSeconds(0.5f);
            lost = true;
        }

        pointPlayerOne = calculatePoint(playerOneCard);
        playerOnePoint.text = pointPlayerOne.ToString();
        pointPlayerTwo = calculatePoint(playerTwoCard);
        playerTwoPoint.text = "?";
        if(isHouseTurn)
            playerTwoPoint.text = pointPlayerTwo.ToString();
                if(lost || won){
            if(lost){
                result.color = Color.red;
                result.text = "YOU LOST";
            }
            else{
                result.color = Color.green;
                result.text = "YOU WON";
            }
        }



    }
    IEnumerator moveCardWithoutFlipping(GameObject card, Transform destination){
        Vector3 direction = destination.position - card.transform.position;
        float d = Vector3.Distance(destination.position,card.transform.position);
        float ETA = 0.3f;
        for (int i = 0; i < 300; i++){
            card.transform.Translate(direction / 300f);
            yield return new WaitForSeconds(ETA/300f);
        }
        Debug.Log(Time.time);
    }
}
