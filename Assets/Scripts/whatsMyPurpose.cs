using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Networking;
using System.IO;
using TMPro;
using System.Linq;
using UnityEngine.SceneManagement;
using System;


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

    IEnumerator fadeIn(SpriteRenderer spriteRenderer,float time, float delay = 0f){
        yield return new WaitForSeconds(delay);
        spriteRenderer.color = new Color(spriteRenderer.color.r,spriteRenderer.color.g,spriteRenderer.color.b,0f);
        float interval = time/Time.deltaTime;
        for(int i = 0;i<interval;i++){
            spriteRenderer.color = new Color(spriteRenderer.color.r,spriteRenderer.color.g,spriteRenderer.color.b,i/interval);
            yield return new WaitForSeconds(Time.deltaTime);
        }
    }
    IEnumerator fadeOut(SpriteRenderer spriteRenderer,float time, float delay = 0f){
        spriteRenderer.color = new Color(spriteRenderer.color.r,spriteRenderer.color.g,spriteRenderer.color.b,spriteRenderer.color.a);
        yield return new WaitForSeconds(delay);
        float originalAlpha = spriteRenderer.color.a;
        float interval = time/Time.deltaTime;
        for(int i = 0;i<interval;i++){
            spriteRenderer.color = new Color(spriteRenderer.color.r,spriteRenderer.color.g,spriteRenderer.color.b,originalAlpha-(i/interval*originalAlpha));
            yield return new WaitForSeconds(Time.deltaTime);
        }
    }
    IEnumerator ImageFadeOut(Image image,float time, float delay = 0f){
        image.color = new Color(image.color.r,image.color.g,image.color.b,image.color.a);
        yield return new WaitForSeconds(delay);
        float originalAlpha = image.color.a;
        float interval = time/Time.deltaTime;
        for(int i = 0;i<interval;i++){
            image.color = new Color(image.color.r,image.color.g,image.color.b,originalAlpha-(i/interval*originalAlpha));
            yield return new WaitForSeconds(Time.deltaTime);
        }
    }

    IEnumerator ImageFadeIn(Image image,float time, float delay = 0f){
        yield return new WaitForSeconds(delay);
        image.color = new Color(image.color.r,image.color.g,image.color.b,0f);
        float interval = time/Time.deltaTime;
        for(int i = 0;i<interval;i++){
            image.color = new Color(image.color.r,image.color.g,image.color.b,i/interval);
            yield return new WaitForSeconds(Time.deltaTime);
        }

    }
    IEnumerator textFadeOut(TextMeshProUGUI textMeshPro,float time, float delay = 0f){
        yield return new WaitForSeconds(delay);
        float originalAlpha = textMeshPro.color.a;
        textMeshPro.color = new Color(textMeshPro.color.r,textMeshPro.color.g,textMeshPro.color.b,originalAlpha);
        float interval = time/Time.deltaTime;
        for(int i = 0;i<interval;i++){
            if(textMeshPro.color.a > 0f){
                textMeshPro.color = new Color(textMeshPro.color.r,textMeshPro.color.g,textMeshPro.color.b,originalAlpha-(i/interval*originalAlpha));
            }
            yield return new WaitForSeconds(Time.deltaTime);
        }
    }
    IEnumerator textFadeIn(TextMeshProUGUI textMeshPro,float time, float delay = 0f){
        yield return new WaitForSeconds(delay);
        float originalAlpha = textMeshPro.color.a;
        textMeshPro.color = new Color(textMeshPro.color.r,textMeshPro.color.g,textMeshPro.color.b,originalAlpha);
        float interval = time/Time.deltaTime;
        for(int i = 0;i<interval;i++){
            textMeshPro.color = new Color(textMeshPro.color.r,textMeshPro.color.g,textMeshPro.color.b,i/interval);
            yield return new WaitForSeconds(Time.deltaTime);
        }
    }


}

