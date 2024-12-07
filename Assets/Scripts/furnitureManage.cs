using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

[System.Serializable]
    public class FurnitureData
    {
        public GameObject prefab;   // 家具對應的Prefab
        public Sprite icon;         // 家具的圖示
        public int count;           // 家具的數量
    }

public class furnitureManage : MonoBehaviour
{
    //切換介面
    public GameObject otherUI;
    public GameObject furnitureUI;
    public bool isPlacementMode = false;

    //拖拉物品
    private GameObject selectedFurniturePrefab;
    private GameObject currentFurniture;
    
    //家具列表
    public List<FurnitureData> furnitureDataList; 
    private List<FurnitureData> initialFurnitureDataList; 
    //家具欄
    public GameObject backpackSlotPrefab;   
    public Transform backpackGrid;
    public Button leftArrowButton;
    public Button rightArrowButton;
    private int currentPage = 0;
    private int iconsPerPage = 5; 
    private Stack<GameObject> placedFurnitureStack = new Stack<GameObject>();
    //前後層
    public GameObject furniturePanel;
    public GameObject currentLayerText;


    void Awake()
    {
        furnitureUI.SetActive(isPlacementMode);
        PopulateBackpack();
        UpdateArrowButtons();
        initialFurnitureDataList = new List<FurnitureData>();
        foreach (var furnitureData in furnitureDataList)
        {
            // 深拷貝數據，確保是獨立的對象
            initialFurnitureDataList.Add(new FurnitureData
            {
                prefab = furnitureData.prefab,
                icon = furnitureData.icon,
                count = furnitureData.count
            });
        }
    }


    void Update()
    {
        
        HandleFurnitureDragging();
        if (currentFurniture!=null){
            if (Input.GetKeyDown(KeyCode.W))
            {
                ChangeFurnitureLayer(currentFurniture, 1);
                Debug.Log("W");
            }
            if (Input.GetKeyDown(KeyCode.S))
            {
                ChangeFurnitureLayer(currentFurniture, -1);
                Debug.Log("S");
            }
        }
        
    }

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
        int endIndex = Mathf.Min(startIndex + iconsPerPage, furnitureDataList.Count);
        
        // 動態生成當前頁面的圖標
        for (int i = startIndex; i < endIndex; i++)
        {
            var furniture = furnitureDataList[i];
            var slot = Instantiate(backpackSlotPrefab, backpackGrid);
            var imageComponent = slot.GetComponent<Image>();
            if (imageComponent != null)
            {
                imageComponent.sprite = furniture.icon;
            }
            var textComponent = slot.GetComponentInChildren<TextMeshProUGUI>();
            if (textComponent != null)
            {
                textComponent.text = furniture.count.ToString();
            }
            var button = slot.GetComponent<Button>();
            if (button != null)
            {
                button.interactable = furnitureDataList[i].count > 0;
                int index = i; // 本地保存迴圈變數（避免閉包問題）
                button.onClick.AddListener(() =>
                {
                    SelectFurniture(index);
                });
            }

        }
        
    }

    // 更新箭頭按鈕的狀態
    void UpdateArrowButtons()
    {
        // 左箭頭在第1頁時禁用
        leftArrowButton.interactable = currentPage > 0;

        // 右箭頭在最後一頁時禁用
        rightArrowButton.interactable = (currentPage + 1) * iconsPerPage < furnitureDataList.Count;
    }

    // 選擇家具
    void SelectFurniture(int index)
    {
        var furniture = furnitureDataList[index];
        if (furniture.count > 0) // 檢查是否還有剩餘家具
        {
            selectedFurniturePrefab = furniture.prefab;
            Debug.Log($"選中了家具：{furniture.prefab.name}");
        }
        else
        {
            selectedFurniturePrefab = null;
            Debug.Log("該家具數量不足！");
        }
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
        if ((currentPage + 1) * iconsPerPage < furnitureDataList.Count)
        {
            currentPage++;
            PopulateBackpack();
            UpdateArrowButtons();
        }
    }


    // 處理家具拖拉與放置
   void HandleFurnitureDragging()
    {
        // 確保已選擇家具
        if (selectedFurniturePrefab == null) return;

        // 如果有按家具，然後手上沒家具的話，生成家具
        if (selectedFurniturePrefab != null && currentFurniture == null)
        {
            Vector2 mousePosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            currentFurniture = Instantiate(selectedFurniturePrefab, mousePosition, Quaternion.identity);
            currentFurniture.transform.SetParent(furniturePanel.transform, false);

            // 確保生成家具有 FurnitureIdentifier 並正確設置 prefab
            var identifier = currentFurniture.GetComponent<FurnitureIdentifier>();
            if (identifier == null)
            {
                identifier = currentFurniture.AddComponent<FurnitureIdentifier>();
            }
            identifier.prefab = selectedFurniturePrefab;

            placedFurnitureStack.Push(currentFurniture);
        }

        // 拖動中的家具跟隨滑鼠移動
        if (currentFurniture != null)
        {
            Vector2 mousePosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            currentFurniture.transform.position = new Vector3(mousePosition.x, mousePosition.y, 0);
            // 左鍵放置家具
            if (Input.GetMouseButtonDown(0))
            {
                placedFurnitureStack.Push(currentFurniture); // 記錄放置的家具
                currentFurniture = null; // 放置完成，清空拖動家具
                var selectedData = furnitureDataList.Find(f => f.prefab == selectedFurniturePrefab);
                // Debug.Log(selectedFurniturePrefab);
                // Debug.Log(selectedData.count);
                if (selectedData != null && selectedData.count > 0)
                    {
                        selectedData.count--;
                        PopulateBackpack();
                    }
                selectedFurniturePrefab = null;
            }
        }
}


    // 改變家具的層級
    void ChangeFurnitureLayer(GameObject furniture, int direction)
    {
        SpriteRenderer sr = furniture.GetComponent<SpriteRenderer>();
        if (sr != null)
        {
            sr.sortingOrder += direction;  // 調整層級
            Debug.Log("layer change");
            var currentLayerDisplay = currentLayerText.GetComponentInChildren<TextMeshProUGUI>();
            if (currentLayerDisplay != null)
            {
                currentLayerDisplay.text = $"{sr.sortingOrder}";
            }
            else{
                Debug.Log("no display");
            }
        }
    }




 // 回復上一個動作
    public void UndoLastPlacement()
    {
        Debug.Log($"堆疊狀態：{placedFurnitureStack.Count} 個家具物件");
        if (placedFurnitureStack.Count > 0)
        {
            GameObject lastPlacedFurniture = placedFurnitureStack.Pop(); // 取出最後放置的家具
            if (lastPlacedFurniture == null)
            {
                Debug.LogWarning("最後放置的家具為 null，無法回復！");
                return;
            }

            var identifier = lastPlacedFurniture.GetComponent<FurnitureIdentifier>();
            if (identifier == null)
            {
                Debug.LogWarning("家具缺少 FurnitureIdentifier 組件，無法更新數量！");
            }
            else
            {
                var prefab = identifier.prefab;
                if (prefab != null)
                {
                    // 更新數量
                    var selectedData = furnitureDataList.Find(f => f.prefab == prefab);
                    if (selectedData != null)
                    {
                        selectedData.count++;
                    }
                }
                else
                {
                    Debug.LogWarning("FurnitureIdentifier.prefab 為 null，無法更新數量！");
                }
            }

            // 刪除家具
            Destroy(lastPlacedFurniture);

            // 更新 UI
            PopulateBackpack();
            }
            else
            {
                Debug.Log("沒有可以回復的動作！");
            }
        }
    // 全部收回
    public void ClearAllFurniture()
    {
        while (placedFurnitureStack.Count > 0)
        {
            GameObject furniture = placedFurnitureStack.Pop();
            Destroy(furniture); // 刪除每一件家具
        }

            // 將數量重設為初始數據
        for (int i = 0; i < furnitureDataList.Count; i++)
        {
            furnitureDataList[i].count = initialFurnitureDataList[i].count;
        }

        PopulateBackpack();
        Debug.Log("全部家具已收回！");
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

}
