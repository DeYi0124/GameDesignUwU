using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameReport : MonoBehaviour
{
    public void ContinueGame() {
        SceneManager.LoadSceneAsync("GameScene");
        GameManager.Instance.reset();
    }
    public void EnterShop() {
        SceneManager.LoadSceneAsync("Shop");
    }
}
