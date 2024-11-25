using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class audioManager : MonoBehaviour
{
    [SerializeField] AudioSource musicSource;

    public AudioClip GCOS;
    public AudioClip defaultBGM;
    public GameObject[] bgm;
    public GameObject DialogueManager;
    void Start(){
        Debug.Log(idToBGMID(checkPointGen.rng));
        musicSource = bgm[idToBGMID(checkPointGen.rng)].GetComponent<AudioSource>();
        musicSource.Play();
    }
    private int idToBGMID(int id){
        if(id == 3) 
            return 1;
        else if( id == 9 || id == 11)
            return 2;
        else if (id == 7)
            return 3;
        else if (id ==6)
            return 4;
        return 0;
    }
}
