using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using TMPro;

public class CosmeticShopContent : MonoBehaviour
{
    void Start() {

    }
    public void Exit() {
        SceneManager.LoadScene("GameReportScene");
    }
}
