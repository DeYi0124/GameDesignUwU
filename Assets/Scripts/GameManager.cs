using System.Collections;
using System.Collections.Generic;
using System;
using UnityEngine;
using Random = UnityEngine.Random;
using UnityEngine.Events;
using UnityEngine.SceneManagement;
using System.IO;
using TMPro;
using UnityEngine.Networking;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance;
    public static Vector3 carPosition;
    public int days = 0;
    public GameObject Car;
    public GameObject vb;
    public GameState State;
    public int bike = 0;
    public int KPI = 5;
    public float oil = 0;
    public float maxOil = 100f;
    public int maxPR = 5;
    public int maxYield = 5;
    public int PR = 1;
    private static int time = 0;
    public int maxTime = 60;
    public int skillLevel = 0;
    public int speedLevel = 0;
    public int oilLevel = 0;
    public int volumeLevel = 0;
    public int yieldLevel = 0;
    public int maxEnt = 100000;
    public int TimeScale = 20;
    public int EnemyLimit = 4;
    public int EnemyCount = 0;
    public String ReasonText;

    public int coins = 0;
    private int credits = 0;
    
    public bool pause = false;
    public static event Action<GameState> OnGameStateChanged;
    public static UnityEvent OnTimeUpdated;

    public Animator transition;
    public float transitionTime = 3f;
    public TextMeshProUGUI LoadText;
    private Vector2 carSpawn;  
    private bool isTutor = false; 
    
    //items
    public int guatiaShow;

    void OnEnable()
    {
        // Debug.Log("OnEnable called");
        SceneManager.sceneLoaded += OnSceneLoaded;
    }
    void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        if(scene.name == "MainScene" || scene.name == "TutorialScene") {
            vb = GameObject.Find("VineBoom");
            vb.SetActive(false);
        }
        if(scene.name == "TutorialScene") {
            isTutor = true;
        }
    }
    void OnDisable()
    {
        // Debug.Log("OnDisable");
        SceneManager.sceneLoaded -= OnSceneLoaded;
    }

    void Awake() {
        Debug.Log("state: " + (Instance == null) + (Instance == this));
        if (Instance != null && Instance != this) {
            Debug.Log("destroying duplicate game manager");
            Destroy(gameObject);
        }
        else 
            Instance = this;
        bike = 0;
        KPI = 10;
        PR = 5;
        days = 0;
        coins = 0;
        credits = 0;
        maxEnt = 100;
        maxTime = 60;
        guatiaShow = 0;
        carSpawn = CarController.Instance.GetPostion();
        //CarController.Instance.SetPosition(carSpawn);
        InvokeRepeating("UpdateTime", 1f, 1f);
        if(FindObjectsOfType<GameManager>().Length > 1) {
            //Debug.Log("DestroyinG" + FindObjectsOfType<GameManager>().Length.ToString());
            this.gameObject.SetActive(false);
            Destroy(this.gameObject);
        } else {
            //Debug.Log("owo");
            DontDestroyOnLoad(gameObject);
            CurrentBike[] currs = FindObjectsOfType<CurrentBike>();
            foreach (CurrentBike curr in currs) {
                //Debug.Log("NMSL");
                curr.OnSave += OnSave;
            }
        }
    }

    void OnSave(CurrentBike currBike) {
        // Debug.Log("user saving");
        bike += CarController.Instance.bike;
        time = (time < maxTime - 5)? time + 5 : maxTime - 1;
        CarController.Instance.bike = 0;
        CarController.Instance.SetPosition(carSpawn);
    }

    public void reset() {
        oil = maxOil;
        CarController.Instance.SetPosition(carSpawn);
        CarController.Instance.bike = 0;
        CarController.Instance.SetInputVector(new Vector2(0,0));
        CarController.Instance.reset();
        CarController.Instance.onBroken = false;
        days ++;
        updateGameState(GameState.Morning);
        bike = 0;
        State = GameState.Morning;
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
            time += TimeScale;
            // int tmpEnt = Random.Range(1, maxEnt+1);
            // if(tmpEnt <= 50) {
            //     Debug.Log("event occurs, ID: " + tmpEnt.ToString());
            // }
            if(PR <= 0) { 
                pause = true;
                time = 0;
                days = 0;
                coins = 0;
                PR = 5;
                ReasonText = "You have been jailed for being a criminal";
                if(isTutor) {
                    SceneManager.LoadScene("MainMenu");
                    return;
                }
                SceneManager.LoadSceneAsync("DeathReportScene");
            }
        }   
    }

    public int getTime() {
        return time;
    }
    
    void Start() {
        updateGameState(GameState.Morning);
        StartCoroutine(preloadImages(16));
    }

    void Update() {
        if(!pause){
            Car.SetActive(true);
        }
        GameStateHandle();
    }

    void LoadingScreen() {
        StartCoroutine(LoadLevel());
    }

    IEnumerator LoadLevel() {
        LoadText.text = "End Of The Day";
        transition.SetTrigger("Start");
        yield return new WaitForSecondsRealtime(transitionTime);
        SceneManager.LoadScene("GameReportScene");  
    }

    void GameStateHandle() {
        if(time > (maxTime/2) && State == GameState.Morning) {
            updateGameState(GameState.Evening);
        } else if(time > maxTime && State == GameState.Evening) {
            updateGameState(GameState.Night);
            if(isTutor) {
                SceneManager.LoadScene("MainMenu");
                return;
            }
            if(bike >= KPI) {
                updateMoney(bike*5, 0);
                PR += (bike - KPI);
                SceneManager.LoadScene("GameReportScene");  
            } else {
                days = 0;
                coins = 0;
                PR = 5;
                ReasonText = "You have failed to complete your daily task.";
                SceneManager.LoadScene("DeathReportScene");
            }
        }
    }

    public void updateGameState(GameState newState) {
        State = newState;
        switch(State) {
            case GameState.Morning:
                pause = false;
                time = 0;
                oil = maxOil;
                // bike = 99999;
                //Debug.Log("goodMorning");
                KPI = 5+10*(days);
                // Debug.Log(days);
                // KPI = 0;
                EnemyLimit = 4 + days;
                maxTime = 60*((days / 3) + 1);
                break;
            case GameState.Evening:
                break;
            case GameState.Night:
                //Debug.Log("NIGHT");
                pause = true;
                vb.SetActive(false);
                // Debug.Log(days);
                break;
            case GameState.Shop:
                break;
            case GameState.Lose:
                break;
            default:
                // throw new ArgumentOutOfRangeException(nameof(newState), newState, null);
                // Debug.Log("UwU");
                break;
        }
        //OnGameStateChanged(newState)?.Invoke();
    }
    private IEnumerator preloadImages(int maxID){
        List<int> charNum = new List<int>();
        charNum.Add(1);
        charNum.Add(1);
        charNum.Add(6);
        charNum.Add(1);
        charNum.Add(1);
        charNum.Add(1);
        charNum.Add(1);
        charNum.Add(1);
        charNum.Add(1);
        charNum.Add(1);
        charNum.Add(1);
        charNum.Add(1);
        charNum.Add(1);
        charNum.Add(1);
        charNum.Add(2);
        charNum.Add(1);
        for(int id = 1;id <= maxID;id++){
            for(int characterID = 1;characterID < charNum[id-1];characterID++){
                // Debug.Log("Loading..."+id.ToString()+" "+characterID.ToString());
                string charPath = Application.streamingAssetsPath + "/Character/" + id.ToString() +'/'+ characterID.ToString() + ".png";
                if (charPath.Contains("://") || charPath.Contains(":///"))
                {
                    byte[] imgData;
                    Texture2D tex = new Texture2D(2, 2);
                    UnityWebRequest www = UnityWebRequest.Get(charPath);
                    yield return www.SendWebRequest();
                    imgData = www.downloadHandler.data;
                }
                string bgPath = Application.streamingAssetsPath + "/BackGround/" + id.ToString() + ".png";
                if (bgPath.Contains("://") || bgPath.Contains(":///"))
                {
                    byte[] imgData;
                    Texture2D tex = new Texture2D(2, 2);
                    UnityWebRequest www = UnityWebRequest.Get(bgPath);
                    yield return www.SendWebRequest();
                    imgData = www.downloadHandler.data;
                }
            }
        }
        // Debug.Log("All loaded");

    }
}

public enum GameState {
    Morning, 
    Evening, 
    Night, 
    Shop,
    Lose
}
