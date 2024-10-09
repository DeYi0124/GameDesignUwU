using System.Collections;
using System.Collections.Generic;
using UnityEngine;
//using UnityEngine.Object;
using System;
using TMPro;

public class CurrentBike : MonoBehaviour
{
    public TextMeshProUGUI myText;
    public Transform textMeshTransform;
    CarController carController;
    public event Action<CurrentBike> OnReceiving;
    public event Action<CurrentBike> OnSave;
    void OnTriggerExit2D(Collider2D collider2D) {
        if(collider2D.CompareTag("CheckPoints")) {
            checkPointHandler checkpoint = collider2D.GetComponent<checkPointHandler>();
            if (checkpoint.getProgress() == 0) {
                if(carController.bike < carController.maxBike)
                    carController.bike++;
                myText.text = carController.bike.ToString() + "/" + carController.maxBike.ToString();
                Destroy(checkpoint.transform.gameObject);
            }
        }
    }

    void OnTriggerEnter2D(Collider2D collider2D) {
        if(collider2D.CompareTag("save")) {
            OnSave?.Invoke(this);
            myText.text = carController.bike.ToString() + "/" + carController.maxBike.ToString();
        }
    }
    void Awake() {
        carController = GetComponentInParent<CarController>();
    }
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        myText.transform.rotation = Camera.main.transform.rotation;
    }
}
