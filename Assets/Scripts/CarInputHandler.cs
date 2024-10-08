using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class carInputHandler : MonoBehaviour
{
    CarController m_carController;

    void Awake()
    {
        GameObject obj = GameObject.Find("Car");
        Debug.Log(obj);
        m_carController = obj.GetComponent<CarController>();
    }
    // Start is called before the first frame update
    void Start()
    {
        // GameObject obj = GameObject.Find("Car");
        // Debug.Log(obj);
        // m_carController = obj.GetComponent<CarController>();
    }

    // Update is called once per frame
    void Update()
    {
        
        Vector2 inputVector = Vector2.zero;
        inputVector.x = Input.GetAxis("Horizontal");
        inputVector.y = Input.GetAxis("Vertical");
        m_carController.SetInputVector(inputVector);
    }
}
