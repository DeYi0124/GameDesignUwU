using UnityEngine;

public class Follow_player : MonoBehaviour {

    public Transform player;
    
    // Update is called once per frame
    // void Awake() {
    //     player = GetComponent<Car>();
    // }
    void Update () {
        if(player != null)
            transform.position = player.transform.position + new Vector3(0, 1, -5);
        else{
            var newCar = GameObject.FindWithTag("Car");
            player = newCar.transform;
            transform.position = player.transform.position + new Vector3(0, 1, -5);
        }
    }
}