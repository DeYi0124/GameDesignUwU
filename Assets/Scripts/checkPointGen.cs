using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;
using UnityEngine.SceneManagement;
using TMPro;

public class checkPointGen : MonoBehaviour
{
    public GameObject myPrefab;
    public GameObject Car;
    public GameObject[] bikePts;
    public GameObject mc;
    public static int rng;
    public int rngUpperLimit;

    private int time = 0;
    private int allBike = 0;
    private bool[] alreadyFilled;
    private TextMeshPro bikePerPointText;
    private int bikePerPointInt;
    private bool metCmm = false;
    private bool mcz = false;

    public Animator transition;
    public float transitionTime = 3f;
    public TextMeshProUGUI LoadText;
    public bool isTutor = false;
    void OnEnable()
    {
        SceneManager.sceneLoaded += OnSceneLoaded;
    }
    void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        if(scene.name == "TutorialScene") {
            isTutor = true;
        }
    }
    void OnDisable()
    {
        SceneManager.sceneLoaded -= OnSceneLoaded;
    }

    public void genBike() {
        int tmpBike = Random.Range(0, allBike);
        //Debug.Log(tmpBike);
        if(time % 2 == 0 && !alreadyFilled[tmpBike] && !PauseMenu.isPaused && !GameManager.Instance.pause) {
            GameObject tmpgobj = Instantiate(myPrefab, bikePts[tmpBike].transform.position, Quaternion.identity, transform);
            checkPointHandler tmpcph = tmpgobj.GetComponent<checkPointHandler>();
            int yiedlLevel = GameManager.Instance.yieldLevel;
            int tmprng = Random.Range(getBikePerPointRange(yiedlLevel).Item1, getBikePerPointRange(yiedlLevel).Item2+1);
            tmpcph.setBikePerPoint(tmprng);
            GameObject textGameObject = tmpcph.transform.GetChild(0).gameObject;
            textGameObject.SetActive(true);
            bikePerPointText = textGameObject.GetComponent<TextMeshPro>();
            bikePerPointText.text = tmpcph.getBikePerPoint().ToString();


            tmpcph.id = tmpBike;
            tmpcph.OnReceiving += OnReceiving;
            time = 1;
            alreadyFilled[tmpBike] = true;
        } else {
            time ++;
        }
        if(GameManager.Instance.getTime() >= GameManager.Instance.maxTime){
            for(int i = 0; i < allBike; i++) {
                alreadyFilled[i] = false;
            }
        }
    }
    void Awake() {
        InvokeRepeating("genBike", 1f, 1f);
        allBike = bikePts.Length;
        alreadyFilled = new bool[allBike];
        for(int i = 0; i < allBike; i++) {
            alreadyFilled[i] = false;
        }
        mcz = false;
        isTutor = false;
    }
    
    void Update() {
        if(mcz) {
            //UwU
            mc = GameObject.Find("Da Main Camera");
            float targetSize = 2f;
            float zoomSpeed = 1.5f;
            mc.GetComponent<Camera>().orthographicSize = Mathf.Lerp(mc.GetComponent<Camera>().orthographicSize, targetSize, Time.deltaTime * zoomSpeed);
            // mc.GetComponent<Camera>().orthographicSize = 2f;
        }
    }

     public void FadeIn() {
        StartCoroutine(LoadFadeIn());
    }

     IEnumerator LoadFadeIn() {
        yield return new WaitForSecondsRealtime(3.1415926583f);
        LoadText.text = "Event Encountered!";
        transition.SetTrigger("Start");
        yield return new WaitForSecondsRealtime(transitionTime);
        mc.GetComponent<Follow_player>().cmc(false);
        mcz = false;
        SceneManager.LoadScene("DialogueTemplate");
        transition.SetTrigger("End");
    }

    void OnReceiving(checkPointHandler cph) {
        Debug.Log(isTutor);
        alreadyFilled[cph.id] = false;
        rng = Random.Range(1, rngUpperLimit+1);
        Debug.Log(rng);
        // rng = 3;
        if(isTutor) return;
        if (rng == 16 && !metCmm)
            metCmm = true;
        else if(rng == 16 && metCmm)
            return;
        if(rng <= 16){
            GameManager.Instance.pause = true;
            Car.SetActive(false);
            Car.SetActive(true);
            Car.transform.GetChild(0).GetChild(6).gameObject.SetActive(true);
            mcz = true;
            mc = GameObject.Find("Da Main Camera");
            mc.GetComponent<Follow_player>().cmc(true);
            mc.transform.position -= new Vector3(0, 1.618f, 0);
            // mc.Size = 2f;
            // Time.timeScale = 0;
            GameManager.carPosition = GameObject.FindWithTag("Car").GetComponent<Transform>().position;
            if(rng == 1) {
                CarController.Instance.bike = (CarController.Instance.bike + 5 > CarController.Instance.maxBike)? CarController.Instance.maxBike : CarController.Instance.bike + 5;
            }else if(rng == 2) {
                GameManager.Instance.PR = (GameManager.Instance.PR + 5 > GameManager.Instance.maxPR)? GameManager.Instance.maxPR : GameManager.Instance.PR + 5;
            }
            else if(rng == 4) {
                CarController.Instance.bike -= 1;
            }else if(rng == 5){
                GameManager.Instance.PR = (GameManager.Instance.PR + 5 > GameManager.Instance.maxPR)? GameManager.Instance.maxPR : GameManager.Instance.PR + 5;
            }else if(rng == 6) {
                CarController.Instance.bike = 0;
                GameManager.Instance.PR = (GameManager.Instance.PR + 5 > GameManager.Instance.maxPR)? GameManager.Instance.maxPR : GameManager.Instance.PR + 10;
            }else if(rng == 7) {
                GameManager.Instance.coins = 0;
            }else if(rng == 9) {
                GameManager.Instance.guatiaShow += 2;
            }
            FadeIn();
        }
    }
    public (int, int) getBikePerPointRange(int yieldLevel){
        switch(yieldLevel){
            case 0://1
                return (1, 2);
            case 1://1
                return (1, 3);
            case 2://2
                return (1, 4);
            case 3://3
                return (1, 5);
            case 4://5
                return (2, 4);
            case 5://8
                return (2, 5);
            case 6://13
                return (2, 6);
            case 7://21
                return (3, 6);
            case 8://34
                return (4, 6);
            case 9://55
                return (5, 6);
            case 10:
                return (6, 6);
            default:
                return (0, 0);
        }

    }

}
