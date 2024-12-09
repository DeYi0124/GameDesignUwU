using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using TMPro;

public class InGameShopContent : MonoBehaviour
{
    public TextMeshProUGUI maxBiketext;
    public TextMeshProUGUI maxOiltext;
    public TextMeshProUGUI maxSpeedtext;
    public TextMeshProUGUI maxSkilltext;
    public TextMeshProUGUI IWILLNOTYIELD;

    private int maxLevel = 20;
    private int[] level2cost = {1, 1, 2, 3, 5, 8, 13, 21, 34, 55, 89, 144, 233, 377, 610, 987, 1597, 2584, 4181, 6765, -1};
    private string[] level2costString = {"1", "1", "2", "3", "5", "8", "13", "21", "34", "55", "89", "144", "233", "377", "610", "987", "1597", "2584", "4181", "6765", "MAX"};
    void Start() {
        maxBiketext.text = "Bike\ncost: " +level2costString[GameManager.Instance.volumeLevel];
        maxOiltext.text = "Oil\ncost: " +level2costString[GameManager.Instance.oilLevel];
        maxSpeedtext.text = "Speed\ncost: " +level2costString[GameManager.Instance.speedLevel];
        maxSkilltext.text = "Skill\ncost: " +level2costString[GameManager.Instance.skillLevel];
        IWILLNOTYIELD.text = "Yield\ncost: " +level2costString[GameManager.Instance.yieldLevel];;
    }
    public void upGradeSkill() {
        if(GameManager.Instance.skillLevel >= maxLevel) {
            maxSkilltext.text = "Skill\ncost: MAX";
            Debug.Log("max level detected");
            return;
        }
        if (GameManager.Instance.coins >= level2cost[GameManager.Instance.skillLevel]) {
            GameManager.Instance.coins -= level2cost[GameManager.Instance.skillLevel];
            GameManager.Instance.skillLevel ++;
            maxSkilltext.text = "Skill\ncost: " +level2cost[GameManager.Instance.skillLevel];
        } else {
            Debug.Log("poor detected");
            return;
        }
    }
    public void upGradeSpeed() {
        if(GameManager.Instance.speedLevel >= maxLevel) {
            maxSpeedtext.text = "Speed\ncost: MAX";
            Debug.Log("max level detected");
            return;
        }
        if (GameManager.Instance.coins >= level2cost[GameManager.Instance.speedLevel]) {
            GameManager.Instance.coins -= level2cost[GameManager.Instance.speedLevel];
            GameManager.Instance.speedLevel ++;
            maxSpeedtext.text = "Speed\ncost: " +level2cost[GameManager.Instance.speedLevel];
            CarController.Instance.maxSpeed += 26;
        } else {
            Debug.Log("poor detected");
            return;
        }
    }
    public void upGradeVolume() {
        if(GameManager.Instance.volumeLevel >= maxLevel) {
            Debug.Log("max level detected");
            maxBiketext.text = "Bike\ncost: MAX";
            return;
        }
        if (GameManager.Instance.coins >= level2cost[GameManager.Instance.volumeLevel]) {
            GameManager.Instance.coins -= level2cost[GameManager.Instance.volumeLevel];
            GameManager.Instance.volumeLevel ++;
            maxBiketext.text = "Bike\ncost: " +level2cost[GameManager.Instance.volumeLevel];
            CarController.Instance.maxBike += 5;
        } else {
            Debug.Log("poor detected");
            return;
        }
    }
    public void upGradeOil() {
        if(GameManager.Instance.oilLevel >= maxLevel) {
            Debug.Log("max level detected");
            maxOiltext.text = "Oil\ncost: MAX";
            return;
        }
        if (GameManager.Instance.coins >= level2cost[GameManager.Instance.oilLevel]) {
            GameManager.Instance.coins -= level2cost[GameManager.Instance.oilLevel];
            GameManager.Instance.oilLevel ++;
            maxOiltext.text = "Oil\ncost: " +level2cost[GameManager.Instance.oilLevel];
            GameManager.Instance.maxOil += 25;
        } else {
            Debug.Log("poor detected");
            return;
        }
    }
    public void upGradeYield() {
        if(GameManager.Instance.yieldLevel >= maxLevel) {
            Debug.Log("max level detected");
            IWILLNOTYIELD.text = "Yield\ncost: MAX";
            return;
        }
        if (GameManager.Instance.coins >= level2cost[GameManager.Instance.yieldLevel]) {
            GameManager.Instance.coins -= level2cost[GameManager.Instance.yieldLevel];
            GameManager.Instance.yieldLevel ++;
            IWILLNOTYIELD.text = "Yield\ncost: " +level2cost[GameManager.Instance.yieldLevel];
            GameManager.Instance.maxYield+= 1;
        } else {
            Debug.Log("poor detected");
            return;
    }
}    public void Exit() {
        SceneManager.LoadScene("GameReportScene");
    }
}
