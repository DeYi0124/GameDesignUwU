using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Networking;
using System.IO;
using TMPro;
using System.Linq;
using UnityEngine.SceneManagement;

public class flyingBike : MonoBehaviour
{
    public Sprite[] bikes;
    private Transform destination;
    private Transform origin;
    private Vector3 positionDiff;
    // Start is called before the first frame update
    void Start()
    {
        origin = this.transform;
        
    }
    void Awake(){
        this.GetComponent<SpriteRenderer>().sprite = bikes[Random.Range(0,bikes.Length)];

    }
    public void setDestination(Transform dest){
        destination = dest;
        positionDiff = destination.position - transform.position;
    }
    // Update is called once per frame
    void Update()
    {
        this.transform.right = positionDiff;
        this.GetComponent<SpriteRenderer>().flipY = (positionDiff.x < 0);
        this.GetComponent<SpriteRenderer>().flipX = true;
        this.transform.position = Vector3.MoveTowards(this.transform.position, destination.position, 0.01f);
        if(this.transform.position == destination.position){
            Destroy(this.gameObject);
        }
    }
}
