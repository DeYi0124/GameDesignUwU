using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Networking;
using System.IO;
using TMPro;
using System.Linq;
using UnityEngine.SceneManagement;


public class DialogueManager : MonoBehaviour
{
    public GameObject Car;
    public GameObject ll;
    public TextMeshProUGUI nameText;
    public TextMeshProUGUI dialogueText;
    public Animator dialogueBoxAnimator;
    public Animator character1Animator;
    public Animator character2Animator;
    public Animator character3Animator;
    public Animator character4Animator;
    public Animator character5Animator;
    public Animator character6Animator;
    public Animator transition;
    public float transitionTime = 3f;
    public TextMeshProUGUI LoadText;
    public int id;
    public GameObject continueButton, optionA, optionB, optionC;
    public Dialogue dialogue;
    public GameObject nameBG;
    private List<string> dialogueContent;
    private Dictionary<int,int> charNum;
    private Queue<string> sentences;
    private string sentence = "";
    private Queue<string> sentencesOptionA;
    private Queue<string> sentencesOptionB;
    private Queue<string> sentencesOptionC;
    private Queue<string> names;
    private Queue<string> namesOptionA;
    private Queue<string> namesOptionB;
    private Queue<string> namesOptionC;
    private string[] speakerID;
    private int currentSpeaker = 0;
    private int previousSpeaker = 0;
    private bool OddFirstPerson = true;
    private bool EvenFirstPerson = true;
    private float dialogueSpeed = 0.025f;
    private bool hasOption = false;
    private Dictionary<int,string> nextSceneDict;
    private Dictionary<string,Queue<string>> namesDict;
    private Dictionary<string,Queue<string>> sentencesDict;
    private string currentOption;
    private Dictionary<string,string> optionsContent;
    private bool isTyping = false;

    // Start is called before the first frame update
    void Start()
    {
        Time.timeScale = 1f;
        Car = GameObject.FindWithTag("Car");
        isTyping = false;
        id = checkPointGen.rng;
        // id = 11;
        dialogue = new Dialogue();
        dialogue.content = new List<string>();
        dialogueContent = new List<string>();
        sentences = new Queue<string>();
        sentencesOptionA = new Queue<string>();
        sentencesOptionB = new Queue<string>();
        sentencesOptionC = new Queue<string>();
        names = new Queue<string>();
        namesOptionA = new Queue<string>();
        namesOptionB = new Queue<string>();
        namesOptionC = new Queue<string>();
        speakerID = new string[7];
        nextSceneDict = new Dictionary<int,string>();
        nextSceneDict.Add(3,"RPS game");
        nextSceneDict.Add(10,"memGame");
        nextSceneDict.Add(12,"lockGame");
        nextSceneDict.Add(13,"addtion game");
        nextSceneDict.Add(14,"Cmm");
        nextSceneDict.Add(15,"typingGame");
        nextSceneDict.Add(16, "puzzleGame");
        nextSceneDict.Add(21, "10.30");
        namesDict = new Dictionary<string,Queue<string>>();
        namesDict.Add("default",names);
        namesDict.Add("A",namesOptionA);
        namesDict.Add("B",namesOptionB);
        namesDict.Add("C",namesOptionC);
        sentencesDict = new Dictionary<string,Queue<string>>();
        sentencesDict.Add("default",sentences);
        sentencesDict.Add("A",sentencesOptionA);
        sentencesDict.Add("B",sentencesOptionB);
        sentencesDict.Add("C",sentencesOptionC);
        currentOption = "default";
        optionsContent = new Dictionary<string,string>();
        hasOption = false;
        GameManager.Instance.pause = true;
        Car.SetActive(false);
        Car.transform.GetChild(0).GetChild(6).gameObject.SetActive(false);
        charNum = new Dictionary<int,int>();
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

        CarController.Instance.resetMomentum();
        dialogueBoxAnimator.SetBool("IsOpen", false);
        for(int i = 1;i<7;i++){
            TalkingCurrentSpeaker(i,false);
            InSceneCurrentSpeaker(i,false);
        }
        StartCoroutine(PrepWork());
    }

    void Update(){
        if(Input.GetKeyDown("space")){
            if(isTyping) {
                StopAllCoroutines();
                isTyping = false;
                dialogueText.text = sentence;
            } else {
                if(continueButton.activeSelf){
                    DisplayNextSentence();
                }
            }
        }
    }

    IEnumerator PrepWork(){
        yield return StartCoroutine(LoadBackGround());
        if(id == 11) {
            if(GameManager.Instance.guatiaShow > 0) {
                yield return StartCoroutine(ReadDialogueFile(id));
            } else {
                yield return StartCoroutine(ReadDialogueFile(-id));
            }
        }else {
            yield return StartCoroutine(ReadDialogueFile(id));
        }
        for(int i = 1;i <= charNum[id];i++){
            yield return StartCoroutine(LoadCharacter(i));
        }
        TriggerDialogue();
    }
    IEnumerator ReadDialogueFile(int tmpId){
        string path = Application.streamingAssetsPath + "/Dialogue/" + tmpId.ToString() + ".txt";
        if (path.Contains("://") || path.Contains(":///"))
        {
            UnityWebRequest www = UnityWebRequest.Get(path);
            www.downloadHandler = new DownloadHandlerBuffer();
            yield return www.SendWebRequest();
            string tmpDialogueContent = www.downloadHandler.text;
            dialogueContent = tmpDialogueContent.Split('\n').ToList();
        }
        else
        {
            yield return null;
            dialogueContent = File.ReadAllLines(path).ToList();
        }

    }
    public void TriggerDialogue(){
        for( int i = 0;i < dialogueContent.Count; i++){
            dialogue.content.Add(dialogueContent[i]);
        }

        speakerID = dialogue.content[0].Trim().Split(' ');
        dialogue.content.RemoveAt(0);
        if(speakerID[speakerID.Count()-1] == "*")
            StartDialogueWithOptions(dialogue);
        else
            StartDialogue(dialogue);
    }
    public void StartDialogueWithOptions(Dialogue dialogue){
        dialogueBoxAnimator.SetBool("IsOpen", true);
        names.Clear();
        namesOptionA.Clear();
        namesOptionB.Clear();
        namesOptionC.Clear();
        sentences.Clear();
        sentencesOptionA.Clear();
        sentencesOptionB.Clear();
        sentencesOptionC.Clear();
        string option = " ";
        bool isName = true;
        bool loadingOptions = false;
        foreach(string sentence in dialogue.content){
            if(loadingOptions){
                if(sentence.Trim() == "A"){
                    option = "A";
                    continue;
                }
                else if(sentence.Trim() == "B"){
                    option = "B";
                    continue;
                }
                else if(sentence.Trim() == "C"){
                    option = "C";
                    continue;
                }
                if(isName){
                    namesDict[option].Enqueue(sentence);
                    isName = false;
                }
                else if(!isName){
                    sentencesDict[option].Enqueue(sentence);
                    isName = true;
                }


            }
            else if(isName){
                if(sentence.Contains(" || ")){
                    hasOption  = true;
                    names.Enqueue(sentence.Split(" || ")[0]);
                    string[] tmpOptionsContent = sentence.Split(" || ")[1].Split('/');
                    string[] options = {"A","B","C"};
                    for(int i = 0;i< tmpOptionsContent.Length;i++){
                        optionsContent.Add(options[i],tmpOptionsContent[i]);
                    }
                    loadingOptions = true;
                }
                else{
                    names.Enqueue(sentence);
                    isName = false;
                }
            }
            else{
                sentences.Enqueue(sentence);
                isName = true;
            }
        }
        //Debug.Log(sentencesOptionA.Count + " " + sentencesOptionB.Count + " " + sentencesOptionC.Count);
        DisplayNextSentence();
    }
    public void StartDialogue (Dialogue dialogue){

        dialogueBoxAnimator.SetBool("IsOpen", true);
        names.Clear();
        sentences.Clear();
        bool isName = true;
        foreach(string sentence in dialogue.content){
            if(isName){
                names.Enqueue(sentence);
                isName = false;
            }
            else{
                sentences.Enqueue(sentence);
                isName = true;
            }
        }

        DisplayNextSentence();


    }
    public void setToOptionA(){
        currentOption = "A";
        if(id == 17) {
            CarController.Instance.bike = (CarController.Instance.bike + 5 > CarController.Instance.maxBike) ? CarController.Instance.maxBike : CarController.Instance.bike + 5;
        } else if(id == 18) {
            CarController.Instance.bike = (CarController.Instance.bike + 5 > CarController.Instance.maxBike) ? CarController.Instance.maxBike : CarController.Instance.bike + 5;
        } else if(id == 19) {
            //nothing
        } else if(id == 20) {
            //nothing
        }
        DisplayNextSentence();
        continueButton.SetActive(true);
        turnOffOption();
    }
    public void setToOptionB(){
        currentOption = "B";
        if(id == 17) {
            CarController.Instance.bike = (CarController.Instance.bike + 3 > CarController.Instance.maxBike) ? CarController.Instance.maxBike : CarController.Instance.bike + 3;
        } else if(id == 18) {
            //nothing
        } else if(id == 19) {
            CarController.Instance.bike = (CarController.Instance.bike + 3 > CarController.Instance.maxBike) ? CarController.Instance.maxBike : CarController.Instance.bike + 3;
            GameManager.Instance.PR += 10;
        } else if(id == 20) {
            //nothing
        }
        DisplayNextSentence();
        continueButton.SetActive(true);
        turnOffOption();
    }
    public void setToOptionC(){
        currentOption = "C";
        if(id == 17) {
            CarController.Instance.bike = (CarController.Instance.bike + 2 > CarController.Instance.maxBike) ? CarController.Instance.maxBike : CarController.Instance.bike + 2;
        } else if(id == 18) {
            //nothing
        } else if(id == 19) {
            //nothing
        } else if(id == 20) {
            CarController.Instance.bike = (CarController.Instance.bike + 1 > CarController.Instance.maxBike) ? CarController.Instance.maxBike : CarController.Instance.bike + 1;
            GameManager.Instance.PR += 20;
        }
        DisplayNextSentence();
        continueButton.SetActive(true);
        turnOffOption();
    }
    private void turnOffOption(){
        optionA.transform.parent.gameObject.SetActive(false);
        optionB.transform.parent.gameObject.SetActive(false);
        optionC.transform.parent.gameObject.SetActive(false);

    }

    public void DisplayNextSentence(){

        if (sentencesDict[currentOption].Count == 0 || namesDict[currentOption].Count == 0){
            EndDialogue();
            return;
        }
        if(hasOption && currentOption == "default" && (sentencesDict[currentOption].Count == 1 || namesDict[currentOption].Count == 1)){
            string[] options = {"A","B","C"};
            continueButton.SetActive(false);
            GameObject[] optionsButton = {optionA,optionB,optionC};
            for(int i = 0;i < optionsContent.Count;i++){
                optionsButton[i].transform.parent.gameObject.SetActive(true);
                optionsButton[i].GetComponentInChildren<TextMeshProUGUI>().text = optionsContent[options[i]];
            }
        }
        for(int i = 0;i< speakerID.Length;i++){
            if(namesDict[currentOption].Peek() == speakerID[i]){
                previousSpeaker = currentSpeaker;
                currentSpeaker = (i+1);
                break;
            }
            else if(i == speakerID.Length-1){
                previousSpeaker = currentSpeaker;
                currentSpeaker = 0;
            }
        }
        if(namesDict[currentOption].Peek() == " "){
            nameBG.SetActive(false);
            nameText.text = namesDict[currentOption].Dequeue();
        }
        else if(namesDict[currentOption].Peek() == "Me"){
            nameText.text = (whatsMyPurpose.Instance != null)? whatsMyPurpose.Instance.getPlayerName() : "testName";
            nameBG.SetActive(true);
            namesDict[currentOption].Dequeue();
        }
        else{
            nameBG.SetActive(true);
            nameText.text = namesDict[currentOption].Dequeue();
        }

        if((currentSpeaker == 1 || currentSpeaker == 3|| currentSpeaker == 5)&& OddFirstPerson){
            InSceneCurrentSpeaker(currentSpeaker,true);
            OddFirstPerson = false;
        }
        if((currentSpeaker == 2 || currentSpeaker == 4|| currentSpeaker == 6)&& EvenFirstPerson){
            InSceneCurrentSpeaker(currentSpeaker,true);
            EvenFirstPerson = false;
        }
        TalkingCurrentSpeaker(KickSpeaker(currentSpeaker),false);
        InSceneCurrentSpeaker(KickSpeaker(currentSpeaker),false);
        InSceneCurrentSpeaker(currentSpeaker,true);
        TalkingCurrentSpeaker(previousSpeaker,false);
        TalkingCurrentSpeaker(currentSpeaker,true);
        sentence = sentencesDict[currentOption].Dequeue();
        StopAllCoroutines();
        StartCoroutine(TypeSentence(sentence));

    }
    IEnumerator TypeSentence (string fullText){
        isTyping = true;
        dialogueText.text = "";
        for (int i = 0; i < fullText.Length; i++)
        {
            if(fullText[i] == '<') {
                while(fullText[i] != '>') {
                    dialogueText.text += fullText[i];
                    i++;
                }
                dialogueText.text += fullText[i];
            }
            else dialogueText.text += fullText[i];
            yield return new WaitForSeconds(dialogueSpeed);
        }
        isTyping = false;
    }

    void EndDialogue(){
        dialogueBoxAnimator.SetBool("IsOpen", false);
        for (int i = 1;i< 7;i++){
            TalkingCurrentSpeaker(i,false);
            InSceneCurrentSpeaker(i,false);
        }
        if(nextSceneDict.ContainsKey(id)){
            SceneManager.LoadScene(nextSceneDict[id]);
        }
        else {
            GameManager.Instance.pause = false;
            SceneManager.LoadScene("MainScene");
        }
    }

    private IEnumerator LoadBackGround(){
        GameObject BackGround = GameObject.Find("BackGround");
        SpriteRenderer bgRenderer = BackGround.GetComponent<SpriteRenderer>();
        string bgPath = Application.streamingAssetsPath + "/BackGround/" + id.ToString() + ".png";
        if (bgPath.Contains("://") || bgPath.Contains(":///"))
        {
            byte[] imgData;
            Texture2D tex = new Texture2D(2, 2);
            UnityWebRequest www = UnityWebRequest.Get(bgPath);
            yield return www.SendWebRequest();
            imgData = www.downloadHandler.data;
            tex.LoadImage(imgData);
            Vector2 pivot = new Vector2(0.5f, 0.5f);
            Sprite sprite = Sprite.Create(tex, new Rect(0.0f, 0.0f, tex.width, tex.height), pivot, 100.0f);
            bgRenderer.sprite = sprite;
        }
        else
        {
            yield return null;
            bgRenderer.sprite = LoadImageFile(bgPath);
        }
        Dictionary<int,Vector3> ScaleDict = new Dictionary<int,Vector3>();
        ScaleDict.Add(1,new Vector3(5,5,0));
        ScaleDict.Add(2,new Vector3(8,8,0));
        ScaleDict.Add(3,new Vector3(9,9,0));
        ScaleDict.Add(4,new Vector3(6,6,0));
        ScaleDict.Add(5,new Vector3(8,8,0));
        ScaleDict.Add(6,new Vector3(8,8,0));
        ScaleDict.Add(7,new Vector3(8,8,0));
        ScaleDict.Add(8,new Vector3(8,8,0));
        ScaleDict.Add(9,new Vector3(1.4f,1.4f,0));
        ScaleDict.Add(10, new Vector3(1.4f,1.4f,0));
        ScaleDict.Add(11, new Vector3(1.4f,1.4f,0));
        ScaleDict.Add(14, new Vector3(1.2f,1.2f,0));
        ScaleDict.Add(13, new Vector3(2.5f,2.3f,0));
        ScaleDict.Add(12, new Vector3(1.5f,0.9f,0));
        ScaleDict.Add(15, new Vector3(2,2,0));
        ScaleDict.Add(16, new Vector3(2,2,0));
        ScaleDict.Add(17, new Vector3(2,2,0));
        ScaleDict.Add(18, new Vector3(2,2,0));
        ScaleDict.Add(19, new Vector3(2,2,0));
        ScaleDict.Add(20, new Vector3(2,2,0));
        ScaleDict.Add(21, new Vector3(2,2,0));
        ScaleDict.Add(22, new Vector3(2,2,0));
        ScaleDict.Add(23, new Vector3(8,8,0));
        ScaleDict.Add(24, new Vector3(8,8,0));
        RectTransform transform = BackGround.GetComponent<RectTransform>();
        transform.localScale = ScaleDict[id];
    }

    private IEnumerator LoadCharacter(int characterID){
        string characterObject = "character" + characterID.ToString();
        GameObject Character = GameObject.Find(characterObject);
        SpriteRenderer charRenderer = Character.GetComponent<SpriteRenderer>();
        string charPath = Application.streamingAssetsPath + "/Character/" + id.ToString() +'/'+ characterID.ToString() + ".png";
        if (charPath.Contains("://") || charPath.Contains(":///"))
        {
            byte[] imgData;
            Texture2D tex = new Texture2D(2, 2);
            UnityWebRequest www = UnityWebRequest.Get(charPath);
            yield return www.SendWebRequest();
            imgData = www.downloadHandler.data;
            tex.LoadImage(imgData);
            Vector2 pivot = new Vector2(0.5f, 0.5f);
            Sprite sprite = Sprite.Create(tex, new Rect(0.0f, 0.0f, tex.width, tex.height), pivot, 100.0f);
            charRenderer.sprite = sprite;
        }
        else
        {
            yield return null;
            charRenderer.sprite = LoadImageFile(charPath);
        }
        Dictionary<int,List<Vector3>> ScaleDict = new Dictionary<int,List<Vector3>>();
        ScaleDict.Add(1,new List<Vector3>());
        ScaleDict[1].Add(new Vector3(0,0,0));
        ScaleDict[1].Add(new Vector3(0,0,0));
        ScaleDict.Add(2,new List<Vector3>());
        ScaleDict[2].Add(new Vector3(35,35,0));
        ScaleDict[2].Add(new Vector3(35,35,0));
        ScaleDict.Add(3,new List<Vector3>());
        ScaleDict[3].Add(new Vector3(35,35,0));
        ScaleDict[3].Add(new Vector3(35,35,0));
        ScaleDict[3].Add(new Vector3(35,35,0));
        ScaleDict[3].Add(new Vector3(35,35,0));
        ScaleDict[3].Add(new Vector3(70,70,0));
        ScaleDict[3].Add(new Vector3(35,35,0));
        ScaleDict.Add(4,new List<Vector3>());
        ScaleDict[4].Add(new Vector3(15,15,0));
        ScaleDict.Add(5,new List<Vector3>());
        ScaleDict[5].Add(new Vector3(80,80,0));
        ScaleDict.Add(6,new List<Vector3>());
        ScaleDict[6].Add(new Vector3(250,250,0));
        ScaleDict.Add(7,new List<Vector3>());
        ScaleDict[7].Add(new Vector3(125,125,0));
        ScaleDict.Add(8,new List<Vector3>());
        ScaleDict[8].Add(new Vector3(50,50,0));
        ScaleDict.Add(9,new List<Vector3>());
        ScaleDict[9].Add(new Vector3(80,80,0));
        ScaleDict.Add(10,new List<Vector3>());
        ScaleDict[10].Add(new Vector3(250,250,0));
        ScaleDict.Add(11,new List<Vector3>());
        ScaleDict[11].Add(new Vector3(80,80,0));
        ScaleDict.Add(14,new List<Vector3>());
        ScaleDict[14].Add(new Vector3(110,110,0));
        ScaleDict.Add(13,new List<Vector3>());
        ScaleDict[13].Add(new Vector3(50,50,0));
        ScaleDict.Add(12,new List<Vector3>());
        ScaleDict[12].Add(new Vector3(15,15,0));
        ScaleDict.Add(15,new List<Vector3>());
        ScaleDict[15].Add(new Vector3(100,100,0));
        ScaleDict[15].Add(new Vector3(100,100,0));
        ScaleDict.Add(16,new List<Vector3>());
        ScaleDict[16].Add(new Vector3(110,110,0));
        ScaleDict.Add(17,new List<Vector3>());
        ScaleDict[17].Add(new Vector3(110,110,0));
        ScaleDict.Add(18,new List<Vector3>());
        ScaleDict[18].Add(new Vector3(110,110,0));
        ScaleDict.Add(19,new List<Vector3>());
        ScaleDict[19].Add(new Vector3(110,110,0));
        ScaleDict.Add(20,new List<Vector3>());
        ScaleDict[20].Add(new Vector3(110,110,0));
        ScaleDict.Add(21,new List<Vector3>());
        ScaleDict[21].Add(new Vector3(110,110,0));
        ScaleDict[21].Add(new Vector3(110,110,0));
        ScaleDict[21].Add(new Vector3(150,150,0));
        ScaleDict[21].Add(new Vector3(110,110,0));
        ScaleDict.Add(22,new List<Vector3>());
        ScaleDict[22].Add(new Vector3(100,100,0));
        ScaleDict.Add(23,new List<Vector3>());
        ScaleDict[23].Add(new Vector3(75,75,0));
        ScaleDict[23].Add(new Vector3(105,105,0));
        ScaleDict.Add(24,new List<Vector3>());
        ScaleDict[24].Add(new Vector3(105,105,0));
        RectTransform transform = Character.GetComponent<RectTransform>();
        transform.localScale = ScaleDict[id][characterID-1];

    }

    private Sprite LoadImageFile(string path){
        byte[] imgData;
        Texture2D tex = new Texture2D(2, 2);
        imgData = File.ReadAllBytes(path);
        tex.LoadImage(imgData);
        Vector2 pivot = new Vector2(0.5f, 0.5f);
        Sprite sprite = Sprite.Create(tex, new Rect(0.0f, 0.0f, tex.width, tex.height), pivot, 100.0f);
        return sprite;
    }
    public int getID(){
        return id;
    }
    private void TalkingCurrentSpeaker(int currentSpeaker, bool flag){
        switch (currentSpeaker){
            case 1:
                character1Animator.SetBool("IsTalking",flag);
                break;
            case 2:
                character2Animator.SetBool("IsTalking",flag);
                break;
            case 3:
                character3Animator.SetBool("IsTalking",flag);
                break;
            case 4:
                character4Animator.SetBool("IsTalking",flag);
                break;
            case 5:
                character5Animator.SetBool("IsTalking",flag);
                break;
            case 6:
                character6Animator.SetBool("IsTalking",flag);
                break;
            default:
                break;
        }
    }
    private void InSceneCurrentSpeaker(int currentSpeaker, bool flag){
        switch (currentSpeaker){
            case 1:
                character1Animator.SetBool("InScene",flag);
                break;
            case 2:
                character2Animator.SetBool("InScene",flag);
                break;
            case 3:
                character3Animator.SetBool("InScene",flag);
                break;
            case 4:
                character4Animator.SetBool("InScene",flag);
                break;
            case 5:
                character5Animator.SetBool("InScene",flag);
                break;
            case 6:
                character6Animator.SetBool("InScene",flag);
                break;
            default:
                break;
        }
    }
    private int KickSpeaker(int current){
        switch(current){
            case 1:
                if(character1Animator.GetBool("InScene"))
                    return 0;
                else if(character3Animator.GetBool("InScene"))
                    return 3;
                else if(character5Animator.GetBool("InScene"))
                    return 5;
                else
                    return 0;
            case 2:
                if(character2Animator.GetBool("InScene"))
                    return 0;
                else if(character4Animator.GetBool("InScene"))
                    return 4;
                else if(character6Animator.GetBool("InScene"))
                    return 6;
                else
                    return 0;
            case 3:
                if(character3Animator.GetBool("InScene"))
                    return 0;
                else if(character1Animator.GetBool("InScene"))
                    return 1;
                else if(character5Animator.GetBool("InScene"))
                    return 5;
                else
                    return 0;
            case 4:
                if(character4Animator.GetBool("InScene"))
                    return 0;
                else if(character2Animator.GetBool("InScene"))
                    return 2;
                else if(character6Animator.GetBool("InScene"))
                    return 6;
                else
                    return 0;
            case 5:
                if(character5Animator.GetBool("InScene"))
                    return 0;
                else if(character1Animator.GetBool("InScene"))
                    return 1;
                else if(character3Animator.GetBool("InScene"))
                    return 3;
                else
                    return 0;
            case 6:
                if(character6Animator.GetBool("InScene"))
                    return 0;
                else if(character2Animator.GetBool("InScene"))
                    return 2;
                else if(character4Animator.GetBool("InScene"))
                    return 4;
                else
                    return 0;
            default:
                return 0;
        }
    }
}
