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


public class whatsMyPurpose : MonoBehaviour
{
    public static whatsMyPurpose Instance;
    public List<furnitureData> furnitureList = new List<furnitureData>();
    public event Action OnInventoryChanged;
    public List<GameObject> placedFurniture = new List<GameObject>();
    private string playerName = "";
    // Start is called before the first frame update
    void Awake(){
        if (Instance != null && Instance != this) {
            Destroy(gameObject);
        }
        else
            Instance = this;
        DontDestroyOnLoad(gameObject);
    }
    public void readPlayerNameFromScript(){
        playerName = sayMyNameManager.playerName;
        Debug.Log(playerName);
    }
    public string getPlayerName(){
        return playerName;
    }
    public void addPlacedFurniture(GameObject obj,int layerIndex,RectTransform rectTransform){
        GameObject temp = Instantiate(obj);
        temp.transform.localScale = rectTransform.localScale;
        temp.transform.localPosition = rectTransform.localPosition;
        temp.transform.localRotation = rectTransform.localRotation;
        temp.transform.SetParent(this.gameObject.transform);
        temp.transform.SetSiblingIndex(layerIndex);

    }
    public void AddOrUpdateFurniture(string furnitureName, GameObject prefab, Sprite icon)
    {
        if (string.IsNullOrEmpty(furnitureName))
        {
            return;
        }
        if (prefab == null || icon == null)
        {
            return;
        }

        furnitureData existingFurniture = furnitureList.Find(f => f.furnitureName == furnitureName);

        if (existingFurniture != null)
        {
            existingFurniture.count++;
        }
        else
        {
            // 如果不存在，新增一個新的家具
            furnitureData newFurniture = ScriptableObject.CreateInstance<furnitureData>();
            newFurniture.furnitureName = furnitureName;
            newFurniture.prefab = prefab;
            newFurniture.icon = icon;
            newFurniture.count = 1; // 初始數量設為 1
            furnitureList.Add(newFurniture);
        }
        OnInventoryChanged?.Invoke();
    }


}

