using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using System.Linq;
using System;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class CmmManager : MonoBehaviour
{
    public TextMeshProUGUI answer;
    public TextMeshProUGUI hint;
    public TextMeshProUGUI console;
    public TextMeshProUGUI submitText;
    public TextMeshProUGUI fakeText;
    public TMP_InputField realCodeText;
    public TMP_InputField inputField;
    public GameObject consoleGameObject;
    public GameObject exitButtonObject;
    public GameObject submitButton;
    public GameObject realCode;
    private string submition;
    private bool submitted = false;
    private Dictionary<int,string> hints = new Dictionary<int,string>();
    private Dictionary<int,string> consoleLog = new Dictionary<int,string>();
    private List<(int, int)> RedArray = new List<(int,int)>(){(55, 27), (101, 37), (87, 34), (108, 54), (122, 61), (43, 55), (118, 28), (106, 32), (58, 41), (33, 65)};
    private List<string> lines = new List<string>();
    private int errorID = 0;
    private bool success = false;
    private bool realCodeTyped = false;
    private Dictionary<string,string> wordsToColorDict = new Dictionary<string,string>();
    private List<string> colorList = new List<string>();
    // Start is called before the first frame update
    void makeItRed(int start, int size) {
        if(size <= 0) return;
        if(start < 0) return;
        Debug.Log(hint.text);
        //hint.text.Trim((char)8203);
        Debug.Log(hint.textInfo.characterInfo.Count());
        for(int i = 0; i < size; i++) {
            hint.textInfo.characterInfo[i+start].color = Color.red;
        }
    }
    void Start()
    {
        inputField.ActivateInputField();
        inputField.customCaretColor = true;
        inputField.caretColor = Color.white;
        consoleGameObject.SetActive(true);
        submitButton.SetActive(true);
        realCode.SetActive(false);
        exitButtonObject.SetActive(false);
        submitted = false;
        success = false;
        realCodeTyped = false;
        hints = new Dictionary<int,string>();
        consoleLog = new Dictionary<int,string>();
        lines = new List<string>();
        setWordsToColorDict();
        hints.Add(-1, "Although being creative is quite impressive. In C-- we do not tolerate creativeity, FOLLOW THE FUCKING RULES AND LISTEN TO WHAT I SAID.");
        hints.Add(0,"Looks like you forgot to teach the code how to speak english, no wonder the code is not working. Try including <color=#65da3f><english.h></color> in the first line.");
        hints.Add(1,"In C--, using semicolon is a grave sin. Just looking at the <b>;</b> makes me wanna throw up, try using the beautiful colon <color=#fe5cf5><b>:</b></color> instead.");
        hints.Add(2,"When variables and functions are declared in C--, they're always very excited, to express their excitement, an exclamation mark <color=#f5c144>!</color> is added after the declaration. For example: <color=#f5c144>int! </color>a <color=#38dacc>=</color> <color=#f3a025>0</color>:");
        hints.Add(3,"I designed C-- for everyone to learn, that's why we spell out all the operators in case someone forgets what they do. Try replacing the <color=#38dacc> = </color>with the word <color=#38dacc>equals</color> and <color=#38dacc>+</color> with the word <color=#38dacc>plus</color>.");
        hints.Add(4,"In C--, because I'm a cute discord kitten, initialization value for every variable needs to be <color=#f3a025>UwU</color>. For example: <color=#f5c144>int! </color>a <color=#38dacc>equals</color> <color=#f3a025>UwU</color>:");
        hints.Add(5,"In C--, everyone deserves a warm home, including the main function. Instead of 0, main function should return <color=#65da3f>\"Home\"</color>, try changing the <color=#f3a025>0</color> to <color=#65da3f>\"Home\"</color>");
        hints.Add(6,"Oh did you forget your C classes, <color=#65da3f>\"Home\"</color> is a char array, so now the main function is not an int anymore. Try <color=#f5c114>const</color> <color=#f5c114><color=#f5c114><color=#f5c114>char</color>*</color>!</color> <color=#4482f5>main</color>() instead");
        hints.Add(7,"Oh my god, you did it!, you're now a C-- pro. Let's check if you code work by inputting two integer into the console, for example: 3 4");
        hints.Add(8,"Here is a very basic c code, now prove your worth by translating it into the glorious C-- language.");
        consoleLog.Add(-1,"");
        consoleLog.Add(0,"");
        consoleLog.Add(1,"");
        consoleLog.Add(2,"");
        consoleLog.Add(3,"");
        consoleLog.Add(4,"");
        consoleLog.Add(5,"");
        consoleLog.Add(6,"");
        consoleLog.Add(7,"");
        consoleLog.Add(8,"");
        hint.text = hints[8];
        console.text = consoleLog[8];
        createFakeText();
        //makeItRed(RedArray[9].Item1, RedArray[9].Item2);
    }
    // Update is called once per frame
    void Update()
    {
        // createFakeText();
        if (submitted){
            if(!success) {
                judge(submition);
                hint.text = hints[errorID];
                console.text = findLine(errorID);
            }
            if(success && realCodeTyped){
                string[] nums = submition.Trim().Split(' ');
                Debug.Log(nums.Count());
                if(nums.Count() != 2) {
                    realCodeTyped = false;
                    hint.text = "Are you stupid or something? Exactly 2 integers are required. 2! DOS!";
                    return;
                }
                int finalAns = 0;
                try {
                    finalAns = Int32.Parse(nums[0])+Int32.Parse(nums[1]);
                } catch (Exception e){
                    realCodeTyped = false;
                    hint.text = "You must input two integer, can't you read the objective sign above?";
                    return;
                }
                realCodeText.text = submition +'\n'+ finalAns.ToString();
                hint.text = "Looks like your code is working, you can now leave with my bike and invaluabe C-- skill.";
                exitButtonObject.SetActive(true);
                realCodeText.readOnly = true;
                submitButton.SetActive(false);
            }
            if(errorID == 7)
                success = true;
            submitted = false;
        }
    }
    public void presentFakeText(string bla){
        fakeText.text = bla;
        createFakeText();
    }
    public void readAnswer(string ans){
        lines = ans.Split("\n").ToList();
        submition = ans.Replace("\n","").Replace(" ","");
    }
    public void readRealCodeAnswer(string ans){
        Debug.Log(ans);
        realCodeTyped = true;
        submition = ans;
    }
    public void sumbit(){
        submitted = true;
        if(success){
            realCode.SetActive(true);
            consoleGameObject.SetActive(false);
        }
    }
    public void exitButton(){
        GameManager.Instance.pause = false;
        SceneManager.LoadSceneAsync("MainScene");

    }
    private void judge(string ans){
        for(int i = 7;i >= 0;i--){
            if(ans == getTemplate(i) && i >= errorID){
                errorID = i;
            }
        } 
        return ;
    }

    private string findLine(int ID){
        int lineNum = 0;
        consoleLog[-1] = "幹，哪個腦麻亂動我的code，你他媽給我重來。";
        consoleLog[0] = "編譯錯誤:第1行，錯誤訊息: 幹我看不懂英文啦，include是三小意思";
        consoleLog[1] = "Compilation error: line lineNum, error message: What the fuck is this? why are you using the disgusting semicolon?";
        consoleLog[2] = "Compilation error: line lineNum, error message: Even amateur C-- programmers know that you need exclamation marks after declaration.";
        consoleLog[3] = "Compilation error: line lineNum, error message: If we don't spell out all the operators, idiots like you might forget what they do. Be grateful that we are so considerate";
        consoleLog[4] = "Compilation error: line lineNum, error message: UwU";
        consoleLog[5] = "Compilation error: line lineNum, error message: You need a warm home, but you don't have one. Try changing the 0 to \"Home\"";
        consoleLog[6] = "Compilation error: line lineNum, error message: You are such a disgrace to the CSIE department and to yourself, you should be using const char*! main(){ instead";
        consoleLog[7] = "You finally did it!, you can press submit again to try out the code you just wrote.";
        if (ID == 0){
            if(submition == getTemplate(0)){
                hint.text = hints[0];
                //makeItRed(RedArray[1].Item1, RedArray[1].Item2);
                return consoleLog[0];
            } else {
                inputField.text = "#include<stdio.h>\nint main(){\n  int a = 0;\n  int b = 0;\n  int sum = 0;\n  scanf(\"%d %d\",&a,&b);\n  sum = a + b;\n  printf(\"%d\",sum);\n  return 0;\n}";
                hint.text = hints[-1];
                //makeItRed(RedArray[0].Item1, RedArray[0].Item2);
                return consoleLog[-1];
            }
        }
        else if(ID == 1){
            for(int i = 0;i < lines.Count;i++){
                if(lines[i].Contains(";")){
                    lineNum = i+1;
                    break;
                }
            }
            consoleLog[1] = consoleLog[1].Replace("lineNum",lineNum.ToString());
            if(lineNum == 0) {
                consoleLog[ID] = "Something went wrong, you must have fucked something up, just like usual. But don't worry. We can start all over again, make sure you don't fuck up AGAIN.";
                hint.text = hints[-1];
                inputField.text = "#include<stdio.h>\nint main(){\n  int a = 0;\n  int b = 0;\n  int sum = 0;\n  scanf(\"%d %d\",&a,&b);\n  sum = a + b;\n  printf(\"%d\",sum);\n  return 0;\n}";
                errorID = 0;
            }
            return consoleLog[1];
        }
        else if (ID == 2){
            for(int i = 0;i < lines.Count;i++){
                if(lines[i].Contains("int ")){
                    lineNum = i+1;
                    break;
                }
            }
            consoleLog[2] = consoleLog[2].Replace("lineNum",lineNum.ToString());
            if(lineNum == 0) {
                consoleLog[ID] = "Something went wrong, you must have fucked something up, just like usual. But don't worry. We can start all over again, make sure you don't fuck up AGAIN.";
                hint.text = hints[-1];
                inputField.text = "#include<stdio.h>\nint main(){\n  int a = 0;\n  int b = 0;\n  int sum = 0;\n  scanf(\"%d %d\",&a,&b);\n  sum = a + b;\n  printf(\"%d\",sum);\n  return 0;\n}";
                errorID = 0;
            }
            return consoleLog[2];
        }
        else if (ID == 3){
            for(int i = 0;i < lines.Count;i++){
                if(lines[i].Contains("=") || lines[i].Contains("+")){
                    lineNum = i+1;
                    break;
                }
            }
            consoleLog[3] = consoleLog[3].Replace("lineNum",lineNum.ToString());
            if(lineNum == 0) {
                consoleLog[ID] = "Something went wrong, you must have fucked something up, just like usual. But don't worry. We can start all over again, make sure you don't fuck up AGAIN.";
                hint.text = hints[-1];
                inputField.text = "#include<stdio.h>\nint main(){\n  int a = 0;\n  int b = 0;\n  int sum = 0;\n  scanf(\"%d %d\",&a,&b);\n  sum = a + b;\n  printf(\"%d\",sum);\n  return 0;\n}";
                errorID = 0;
            }
            return consoleLog[3];
        }
        else if (ID == 4){
            for(int i = 0;i < lines.Count;i++){
                if((lines[i].Contains("equals 0") || lines[i].Contains("equals0")) && !lines[i].Contains("return")){
                    lineNum = i+1;
                    break;
                }
            }
            consoleLog[4] = consoleLog[4].Replace("lineNum",lineNum.ToString());
            if(lineNum == 0) {
                consoleLog[ID] = "Something went wrong, you must have fucked something up, just like usual. But don't worry. We can start all over again, make sure you don't fuck up AGAIN.";
                hint.text = hints[-1];
                inputField.text = "#include<stdio.h>\nint main(){\n  int a = 0;\n  int b = 0;\n  int sum = 0;\n  scanf(\"%d %d\",&a,&b);\n  sum = a + b;\n  printf(\"%d\",sum);\n  return 0;\n}";
                errorID = 0;
            }
            return consoleLog[4];
        }
        else if (ID == 5){
            for(int i = 0;i < lines.Count;i++){
                if(lines[i].Contains("return")){
                    lineNum = i+1;
                    break;
                }
            }
            consoleLog[5] = consoleLog[5].Replace("lineNum",lineNum.ToString());
            if(lineNum == 0) {
                consoleLog[ID] = "Something went wrong, you must have fucked something up, just like usual. But don't worry. We can start all over again, make sure you don't fuck up AGAIN.";
                hint.text = hints[-1];
                inputField.text = "#include<stdio.h>\nint main(){\n  int a = 0;\n  int b = 0;\n  int sum = 0;\n  scanf(\"%d %d\",&a,&b);\n  sum = a + b;\n  printf(\"%d\",sum);\n  return 0;\n}";
                errorID = 0;
            }
            return consoleLog[5];
        }
        else if (ID == 6){
            for(int i = 0;i < lines.Count;i++){
                if(lines[i].Contains("main")){
                    lineNum = i+1;
                    break;
                }
            }
            consoleLog[6] = consoleLog[6].Replace("lineNum",lineNum.ToString());
            if(lineNum == 0) {
                consoleLog[ID] = "Something went wrong, you must have fucked something up, just like usual. But don't worry. We can start all over again, make sure you don't fuck up AGAIN.";
                hint.text = hints[-1];
                inputField.text = "#include<stdio.h>\nint main(){\n  int a = 0;\n  int b = 0;\n  int sum = 0;\n  scanf(\"%d %d\",&a,&b);\n  sum = a + b;\n  printf(\"%d\",sum);\n  return 0;\n}";
                errorID = 0;
            }
            return consoleLog[6];
        }
        else if (ID == 7){
            inputField.readOnly = true;
            submitButton.GetComponent<Image>().color = Color.green;
            submitText.text = "test";
            return consoleLog[7];

        }
        return "";
    }
    private void setWordsToColorDict(){
        wordsToColorDict = new Dictionary<string, string> ();
        wordsToColorDict.Add("=", "<color=#38dacc>=</color>");
        wordsToColorDict.Add("int!", "<color=#f5c144>int!</color>");
        wordsToColorDict.Add(":", "<color=#fe5cf5>:</color>");
        wordsToColorDict.Add("#include", "<color=#E058DA>#include</color>");
        wordsToColorDict.Add("main", "<color=#4482f5>main</color>");
        wordsToColorDict.Add("<stdio.h>", "<color=#65da3f><stdio.h></color>");
        wordsToColorDict.Add("<english.h>", "<color=#65da3f><english.h></color>");
        wordsToColorDict.Add("+", "<color=#38dacc>+</color>");
        wordsToColorDict.Add("equals", "<color=#38dacc>equals</color>");
        wordsToColorDict.Add("plus", "<color=#38dacc>plus</color>");
        wordsToColorDict.Add("UwU", "<color=#f3a025>UwU</color>");
        wordsToColorDict.Add("\"Home\"", "<color=#65da3f>\"Home\"</color>");
        wordsToColorDict.Add("\"", "<color=#65da3f>\"</color>");
        wordsToColorDict.Add("%d", "<color=#65da3f>%d</color>");
        wordsToColorDict.Add("scanf", "<color=#b9400a>scanf</color>");
        wordsToColorDict.Add("printf", "<color=#b9400a>printf</color>");
        wordsToColorDict.Add("return", "<color=#8d10c3>return</color>");
        wordsToColorDict.Add("const", "<color=#f5c114>const</color>");
        wordsToColorDict.Add("char*!", "<color=#f5c114>char*!</color>");
        colorList.Add("<color=#f5c114>");
        colorList.Add("<color=#E058DA>");
        colorList.Add("<color=#4482f5>");
        colorList.Add("<color=#65da3f>");
        colorList.Add("<color=#38dacc>");
        colorList.Add("<color=#f3a025>");
        colorList.Add("<color=#b9400a>");
        colorList.Add("<color=#8d10c3>");
        colorList.Add("<color=#f5c144>");
        colorList.Add("<color=#fe5cf5>");
    }
    private void createFakeText(){
        if(fakeText.text == null) return;
        for(int i = 0;i< colorList.Count;i++){
            if(fakeText.text.Contains(colorList[i])){
                fakeText.text = fakeText.text.Replace(colorList[i],"");
            }
        }
        fakeText.text = fakeText.text.Replace("</color>","");
        for(int i = 0;i< wordsToColorDict.Count;i++){
            if(fakeText.text.Contains(wordsToColorDict.ElementAt(i).Key)){
                // if(wordsToColorDict.ElementAt(i).Key == "="){
                //     fakeText.text = fakeText.text.Replace("=#","&&&&&");
                //     fakeText.text = fakeText.text.Replace(wordsToColorDict.ElementAt(i).Key,wordsToColorDict.ElementAt(i).Value);
                //     fakeText.text = fakeText.text.Replace("&&&&&","=#");
                // }else
                fakeText.text = fakeText.text.Replace(wordsToColorDict.ElementAt(i).Key,wordsToColorDict.ElementAt(i).Value);
            }
        }
    }
    
    private string getTemplate(int version){
        switch (version){
            case 0:
                return "#include<stdio.h>intmain(){inta=0;intb=0;intsum=0;scanf(\"%d%d\",&a,&b);sum=a+b;printf(\"%d\",sum);return0;}";
            case 1:
                return "#include<english.h>#include<stdio.h>intmain(){inta=0;intb=0;intsum=0;scanf(\"%d%d\",&a,&b);sum=a+b;printf(\"%d\",sum);return0;}";
            case 2:
                return "#include<english.h>#include<stdio.h>intmain(){inta=0:intb=0:intsum=0:scanf(\"%d%d\",&a,&b):sum=a+b:printf(\"%d\",sum):return0:}";
            case 3:
                return "#include<english.h>#include<stdio.h>int!main(){int!a=0:int!b=0:int!sum=0:scanf(\"%d%d\",&a,&b):sum=a+b:printf(\"%d\",sum):return0:}";
            case 4:
                return "#include<english.h>#include<stdio.h>int!main(){int!aequals0:int!bequals0:int!sumequals0:scanf(\"%d%d\",&a,&b):sumequalsaplusb:printf(\"%d\",sum):return0:}";
            case 5:
                return "#include<english.h>#include<stdio.h>int!main(){int!aequalsUwU:int!bequalsUwU:int!sumequalsUwU:scanf(\"%d%d\",&a,&b):sumequalsaplusb:printf(\"%d\",sum):return0:}";
            case 6:
                return "#include<english.h>#include<stdio.h>int!main(){int!aequalsUwU:int!bequalsUwU:int!sumequalsUwU:scanf(\"%d%d\",&a,&b):sumequalsaplusb:printf(\"%d\",sum):return\"Home\":}";
            case 7:
                return "#include<english.h>#include<stdio.h>constchar*!main(){int!aequalsUwU:int!bequalsUwU:int!sumequalsUwU:scanf(\"%d%d\",&a,&b):sumequalsaplusb:printf(\"%d\",sum):return\"Home\":}";
            default:
                return "";
        }
    }
}
