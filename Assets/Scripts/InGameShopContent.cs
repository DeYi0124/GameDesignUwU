using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class InGameShopContent : MonoBehaviour
{
    public void upGradeSkill() {
        GameManager.Instance.skill ++;
    }
    public void upGradeSpeed() {
        GameManager.Instance.skill ++;
    }
    public void upGradeVolume() {
        GameManager.Instance.skill ++;
    }
    public void upGradeOil() {
        GameManager.Instance.skill ++;
    }
    public void Exit() {
        SceneManager.LoadScene("GameReportScene");
    }
}
