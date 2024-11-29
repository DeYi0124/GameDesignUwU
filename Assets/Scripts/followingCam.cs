using UnityEngine;

public class Follow_player : MonoBehaviour {

    public Transform player;
    private bool cmcs = false;
    
    // Update is called once per frame
    void Awake() {
        cmcs = false;
    }
    public void cmc(bool b) {
        cmcs = b;
    }
    void Update () {
        if(cmcs) return;
        if(player != null)
            transform.position = player.transform.position + new Vector3(0, 1, -5);
        else{
            var newCar = GameObject.FindWithTag("Car");
            player = newCar.transform;
            transform.position = player.transform.position + new Vector3(0, 1, -5);
        }
    }
}