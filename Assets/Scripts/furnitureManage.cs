using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class furnitureManage : MonoBehaviour
{
    public GameObject otherUI;
    public GameObject furnitureUI;
    public bool isPlacementMode = false;

    public GameObject selectedFurniturePrefab;
    private GameObject currentFurniture;

    private bool isDragging = false;
    private Vector3 offset;

    public Sprite[] appearances;
    private int currentIndex = 0;
    private SpriteRenderer spriteRenderer;

    public Dictionary<string, int> furnitureData = new Dictionary<string, int>();

    [System.Serializable]
    public class FurnitureData
    {
        public string prefabName;
        public Vector3 position;
        public Vector3 scale;
        public int variantIndex;
    }

    void Awake()
    {
        furnitureUI.SetActive(isPlacementMode);
        PopulateBackpack();
        UpdateArrowButtons();
    //     furnitureData["Chair"] = 2;
    //     furnitureData["Table"] = 1;

    //     Debug.Log(furnitureData);
    }
    // void start(){
    //     PopulateBackpack();
    //     UpdateArrowButtons();
    // }

    void Update()
    {
        // if (Input.GetMouseButtonDown(0) && selectedFurniturePrefab != null)
        // {
        //     Vector2 mousePosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        //     currentFurniture = Instantiate(selectedFurniturePrefab, mousePosition, Quaternion.identity);
        // }

        // if (currentFurniture != null)
        // {
        //     currentFurniture.transform.position = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        // }

        // if (Input.GetMouseButtonDown(1)) // 右鍵放置
        // {
        //     currentFurniture = null;
        // }
        
    }


    public GameObject backpackSlotPrefab;
    public Transform backpackGrid;
    public List<Sprite> furnitureIcons;
    public List<int> furnitureCounts; // 新增每個家具的數量
    public Button leftArrowButton;        // 左箭頭按鈕
    public Button rightArrowButton;       // 右箭頭按鈕
    private int currentPage = 0;          // 當前頁碼
    private int iconsPerPage = 5;         // 每頁顯示的圖標數

    // 更新背包顯示
    void PopulateBackpack()
    {
        // 清空當前的Grid
        foreach (Transform child in backpackGrid)
        {
            Destroy(child.gameObject);
        }

        // 計算當前頁面的圖標範圍
        int startIndex = currentPage * iconsPerPage;
        int endIndex = Mathf.Min(startIndex + iconsPerPage, furnitureIcons.Count);

        // 動態生成當前頁面的圖標
        for (int i = startIndex; i < endIndex; i++)
        {
            var slot = Instantiate(backpackSlotPrefab, backpackGrid);
            slot.GetComponent<Image>().sprite = furnitureIcons[i];
        }
    }

    // 更新箭頭按鈕的狀態
    void UpdateArrowButtons()
    {
        // 左箭頭在第1頁時禁用
        leftArrowButton.interactable = currentPage > 0;

        // 右箭頭在最後一頁時禁用
        rightArrowButton.interactable = (currentPage + 1) * iconsPerPage < furnitureIcons.Count;
    }

    // 左箭頭被點擊
    public void OnLeftArrowClick()
    {
        if (currentPage > 0)
        {
            currentPage--;
            PopulateBackpack();
            UpdateArrowButtons();
        }
    }

    // 右箭頭被點擊
    public void OnRightArrowClick()
    {
        if ((currentPage + 1) * iconsPerPage < furnitureIcons.Count)
        {
            currentPage++;
            PopulateBackpack();
            UpdateArrowButtons();
        }
    }


    void OnMouseDown()
    {
        isDragging = true;
        offset = transform.position - GetMouseWorldPosition();
    }

    void OnMouseDrag()
    {
        if (isDragging)
        {
            transform.position = GetMouseWorldPosition() + offset;
        }
    }

    void OnMouseUp()
    {
        isDragging = false;
        // 在這裡可以添加放置邏輯，例如檢查是否能合法放置
    }

    private Vector3 GetMouseWorldPosition()
    {
        Vector3 mousePos = Input.mousePosition;
        mousePos.z = Camera.main.nearClipPlane;
        return Camera.main.ScreenToWorldPoint(mousePos);
    }


    public void PlacementMode(bool isPlacementMode)
    {
        otherUI.SetActive(!isPlacementMode);
        furnitureUI.SetActive(isPlacementMode);
    
    }


    public void RotateFurniture()
    {
        transform.Rotate(0, 0, 180); // 鏡像
    }


    void Start()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();
    }

    public void ChangeAppearance()
    {
        currentIndex = (currentIndex + 1) % appearances.Length;
        spriteRenderer.sprite = appearances[currentIndex];
    }


//     public void SaveFurnitureData()
//     {
//         FurnitureData[] furnitureArray = FindObjectsOfType<Furniture>().Select(f => new FurnitureData
//         {
//             prefabName = f.prefabName,
//             position = f.transform.position,
//             scale = f.transform.localScale,
//             variantIndex = f.variantIndex
//         }).ToArray();

//         string json = JsonUtility.ToJson(furnitureArray);
//         PlayerPrefs.SetString("FurnitureData", json);
//         PlayerPrefs.Save();
//     }

// public void LoadFurnitureData()
// {
//     string json = PlayerPrefs.GetString("FurnitureData", "[]");
//     FurnitureData[] furnitureArray = JsonUtility.FromJson<FurnitureData[]>(json);

//     foreach (var data in furnitureArray)
//     {
//         GameObject prefab = Resources.Load<GameObject>(data.prefabName);
//         var furniture = Instantiate(prefab, data.position, Quaternion.identity);
//         furniture.transform.localScale = data.scale;
//         furniture.GetComponent<Furniture>().SetVariant(data.variantIndex);
//     }
// }
// public void DiscardChanges()
// {
//     foreach (var furniture in FindObjectsOfType<Furniture>())
//     {
//         Destroy(furniture.gameObject);
//     }
//     LoadFurnitureData();
// }


}
