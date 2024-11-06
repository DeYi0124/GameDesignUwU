using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
public class AIController : MonoBehaviour
{

    int MoveSpeed = 4;
    int MaxDist = 10;
    int MinDist = 2;
    void Start()
    {

    }

    void Update()
    {
        // transform.LookAt(Player);
        transform.right = CarController.Instance.transform.position - transform.position;

        if (Vector3.Distance(transform.position, CarController.Instance.transform.position) >= MinDist)
        {

            MoveSpeed = 4;
            transform.position += transform.right * MoveSpeed * Time.deltaTime;
            if (Vector3.Distance(transform.position, CarController.Instance.transform.position) <= MaxDist)
            {
                MoveSpeed = 8;// - Vector3.Distance(transform.position, CarController.Instance.transform.position);
                transform.position += transform.right * MoveSpeed * Time.deltaTime;

                //Here Call any function U want Like Shoot at here or something
            }

        }
    }
}
