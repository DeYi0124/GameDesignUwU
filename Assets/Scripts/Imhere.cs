using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Imhere : MonoBehaviour
{
    // Start is called before the first frame update
    void Awake() {
        //Debug.Log(GameManager.Instance);
    }
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if(!GameManager.Instance.vb)
            GameManager.Instance.vb = this.gameObject;
    }
}
