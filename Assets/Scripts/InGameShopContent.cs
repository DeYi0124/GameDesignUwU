using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class InGameShopContent : MonoBehaviour
{
    private int maxLevel = 10;
    private int[] level2cost = {1, 1, 2, 3, 5, 8, 13, 21, 34, 55};
    public void upGradeSkill() {
        if(GameManager.Instance.skillLevel >= maxLevel - 1) {
            Debug.Log("max level detected");
            return;
        }
        if (GameManager.Instance.coins >= level2cost[GameManager.Instance.skillLevel]) {
            GameManager.Instance.coins -= level2cost[GameManager.Instance.skillLevel];
            GameManager.Instance.skillLevel ++;
        } else {
            Debug.Log("poor detected");
            return;
        }
    }
    public void upGradeSpeed() {
        if(GameManager.Instance.speedLevel >= maxLevel - 1) {
            Debug.Log("max level detected");
            return;
        }
        if (GameManager.Instance.coins >= level2cost[GameManager.Instance.speedLevel]) {
            GameManager.Instance.coins -= level2cost[GameManager.Instance.speedLevel];
            GameManager.Instance.speedLevel ++;
            CarController.Instance.maxSpeed += 26;
        } else {
            Debug.Log("poor detected");
            return;
        }
    }
    public void upGradeVolume() {
        if(GameManager.Instance.volumeLevel >= maxLevel - 1) {
            Debug.Log("max level detected");
            return;
        }
        if (GameManager.Instance.coins >= level2cost[GameManager.Instance.volumeLevel]) {
            GameManager.Instance.coins -= level2cost[GameManager.Instance.volumeLevel];
            GameManager.Instance.volumeLevel ++;
            CarController.Instance.maxBike += 10;
        } else {
            Debug.Log("poor detected");
            return;
        }
    }
    public void upGradeOil() {
        if(GameManager.Instance.oilLevel >= maxLevel - 1) {
            Debug.Log("max level detected");
            return;
        }
        if (GameManager.Instance.coins >= level2cost[GameManager.Instance.oilLevel]) {
            GameManager.Instance.coins -= level2cost[GameManager.Instance.oilLevel];
            GameManager.Instance.oilLevel ++;
            GameManager.Instance.maxOil += 50;
        } else {
            Debug.Log("poor detected");
            return;
        }
    }
    public void Exit() {
        SceneManager.LoadScene("GameReportScene");
    }
}
