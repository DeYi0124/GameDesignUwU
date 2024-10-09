using System.Collections;
using System.Collections.Generic;
using System;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance;
    public GameState State;
    public int bike = 0;
    public int KPI = 10;
    private static int time = 0;

    private int coins = 0;
    private int credits = 0;
    
    public bool pause = false;
    public static event Action<GameState> OnGameStateChanged;
    public static UnityEvent OnTimeUpdated;
    void Awake() {
        if (Instance != null && Instance != this) {
            Destroy(gameObject);
        }
        else 
            Instance = this;
        CurrentBike[] currs = FindObjectsOfType<CurrentBike>();
        foreach (CurrentBike curr in currs) {
            curr.OnSave += OnSave;
        }
        bike = 0;
        KPI = 10;
        coins = 0;
        credits = 0;
        pause = false;
        InvokeRepeating("UpdateTime", 1f, 1f);
        if(FindObjectsOfType<GameManager>().Length > 1) {
            //Debug.Log("DestroyinG" + FindObjectsOfType<GameManager>().Length.ToString());
            this.gameObject.SetActive(false);
            Destroy(this.gameObject);
        } else {
            //Debug.Log("owo");
            DontDestroyOnLoad(gameObject);
        }
    }

    void OnSave(CurrentBike currBike) {
        pause = true;
        if (CarController.Instance.bike > 0) {
            bike += CarController.Instance.bike;
        }
    }

    public void reset() {
        pause = false;
        //Debug.Log("RESETTING");
        updateGameState(GameState.Morning);
        OilBar.Instance.oil = OilBar.Instance.maxOil;
        //Debug.Log("FILLING");
        //Debug.Log((OilBar.Instance.oil));
        CarController.Instance.bike = 0;
        KPI = (int)OilBar.Instance.oil;
        State = GameState.Morning;
        pause = false;
    }

    public (int, int) GetMoney() {
        return (coins, credits);
    }

    public void updateMoney(int c1, int c2) {
        coins += c1;
        credits += c2;
    }

    void UpdateTime() {
        if(!pause) {
            time += 1;
            //Debug.Log(time);
        }
    }

    public int getTime() {
        return time;
    }
    
    void Start() {
        updateGameState(GameState.Morning);
    }

    void Update() {
        GameStateHandle();
    }

    void GameStateHandle() {
        if(time > 30 && State == GameState.Morning) {
            updateGameState(GameState.Evening);
        } else if(time > 60 && State == GameState.Evening) {
            updateGameState(GameState.Night);
            SceneManager.LoadSceneAsync("GameReportScene");
        }
    }

    public void updateGameState(GameState newState) {
        State = newState;
        switch(State) {
            case GameState.Morning:
                pause = false;
                //Debug.Log("goodMorning");
                break;
            case GameState.Evening:
                break;
            case GameState.Night:
                //Debug.Log("NIGHT");
                pause = true;
                time = 0;
                break;
            case GameState.Shop:
                break;
            case GameState.Lose:
                break;
            default:
                // throw new ArgumentOutOfRangeException(nameof(newState), newState, null);
                Debug.Log("UwU");
                break;
        }
        //OnGameStateChanged(newState)?.Invoke();
    }
}

public enum GameState {
    Morning, 
    Evening, 
    Night, 
    Shop,
    Lose
}