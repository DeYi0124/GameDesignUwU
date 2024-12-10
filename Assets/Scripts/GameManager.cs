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
    public int totalPlaytime = 0;
    public GameObject Car;
    public GameObject vb;
    public GameState State;
    public GameObject ll;
    public int bike = 0;
    public int KPI = 5;
    public float oil = 0;
    public float maxOil = 100f;
    public int maxPR = 5;
    public int PRlevel = 0;
    public float[] PRspeed = {0.2f, 0.4f, 1.6f, 10.8f, 174.8f};
    public int[] PRaura = {3, 9, 22, 45, 1000};
    public int[] PRrizz = {5, 4, 3, 2, 1};
    public int[] PRfanumTax = {1, 4, 9, 16, 25};
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
    public int kills = 0;
    public String ReasonText;
    public bool[] events = new bool[100];

    public int coins = 0;
    public bool firstTime = false;
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
        SceneManager.sceneLoaded += OnSceneLoaded;
    }
    void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        if(firstTime == false) {
            firstTime = true;
        } else {
            Debug.Log("Fading out");
            ll.GetComponent<LevelLoader>().FadeOut();
        }

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
        SceneManager.sceneLoaded -= OnSceneLoaded;
    }

    void Awake() {
        if (Instance != null && Instance != this) {
            Destroy(gameObject);
        }
        else
            Instance = this;
        bike = 0;
        firstTime = false;
        KPI = 10;
        PR = 5;
        days = 0;
        coins = 0;
        kills = 0;
        credits = 0;
        maxEnt = 100;
        maxTime = 60;
        PRlevel = -1;
        guatiaShow = 0;
        for(int i = 0; i < 100; i++) {
            events[i] = false;
        }
        carSpawn = CarController.Instance.GetPostion();
        InvokeRepeating("UpdateTime", 1f, 1f);
        if(FindObjectsOfType<GameManager>().Length > 1) {
            this.gameObject.SetActive(false);
            Destroy(this.gameObject);
        } else {
            DontDestroyOnLoad(gameObject);
            CurrentBike[] currs = FindObjectsOfType<CurrentBike>();
            foreach (CurrentBike curr in currs) {
                curr.OnSave += OnSave;
            }
        }
    }

    void OnSave(CurrentBike currBike) {
        bike += CarController.Instance.bike;
        time = (time < maxTime - 5)? time + 5 : maxTime - 1;
        CarController.Instance.bike = 0;
        CarController.Instance.onBroken = false;
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
        kills = 0;
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
        StartCoroutine(preloadImages(24));
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
                updateMoney(-KPI*kills, 0);
                if(PRlevel == -1)
                    PR += (bike - KPI)/5*PRrizz[0];
                else
                    PR += (bike - KPI)/5*PRrizz[PRlevel];
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
                kills = 0;
                oil = maxOil;
                KPI = 5+10*(days);
                EnemyLimit = 4 + days;
                maxTime = 60*((days / 3) + 1);
                for(int i = 0; i < 100; i++) {
                    events[i] = false;
                }
                break;
            case GameState.Evening:
                break;
            case GameState.Night:
                pause = true;
                vb.SetActive(false);
                break;
            case GameState.Shop:
                break;
            case GameState.Lose:
                break;
            default:
                break;
        }
    }
    private IEnumerator preloadImages(int maxID){
        Dictionary<int,int> charNum = new Dictionary<int,int>();
        charNum.Add(1,1);
        charNum.Add(2,1);
        charNum.Add(3,6);
        charNum.Add(4,1);
        charNum.Add(5,1);
        charNum.Add(6,1);
        charNum.Add(7,1);
        charNum.Add(8,1);
        charNum.Add(9,1);
        charNum.Add(10,1);
        charNum.Add(11,1);
        charNum.Add(12,1);
        charNum.Add(13,1);
        charNum.Add(14,1);
        charNum.Add(15,2);
        charNum.Add(16,1);
        charNum.Add(17,1);
        charNum.Add(18,1);
        charNum.Add(19,1);
        charNum.Add(20,1);
        charNum.Add(21,4);
        charNum.Add(22,1);
        charNum.Add(23,2);
        charNum.Add(24,1);
        for(int id = 1;id <= maxID;id++){
            for(int characterID = 1;characterID < charNum[id];characterID++){
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
