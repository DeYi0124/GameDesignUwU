using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

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
    private List<furnitureData> furnitureDataList; 
    private List<furnitureData> initialFurnitureDataList; 
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

    public FurnitureInventory inventory;
    private void OnEnable()
    {
        // 訂閱事件
        whatsMyPurpose.Instance.OnInventoryChanged += PopulateBackpack;
        furnitureDataList = whatsMyPurpose.Instance.furnitureList;
        if(whatsMyPurpose.Instance.transform.childCount > 0){
            for(int i = 0;i < whatsMyPurpose.Instance.transform.childCount;i++){
                GameObject tmp = Instantiate(whatsMyPurpose.Instance.transform.GetChild(i).gameObject);
                tmp.transform.SetParent(furniturePanel.transform);
                tmp.transform.localPosition = whatsMyPurpose.Instance.transform.GetChild(i).localPosition;
                tmp.transform.localRotation = whatsMyPurpose.Instance.transform.GetChild(i).localRotation;
                tmp.transform.localScale = whatsMyPurpose.Instance.transform.GetChild(i).localScale;
            }
        }

    }

    private void OnDisable()
    {
        // 取消訂閱事件
        whatsMyPurpose.Instance.OnInventoryChanged -= PopulateBackpack;
        furnitureDataList = whatsMyPurpose.Instance.furnitureList;
        // for(int i = 0;i < furniturePanel.transform.childCount;i++){
        //     GameObject tmp = furniturePanel.transform.GetChild(i).gameObject;
        //     whatsMyPurpose.Instance.addPlacedFurniture(tmp,tmp.transform.GetSiblingIndex(),tmp.GetComponent<RectTransform>());
        // }

    }


    void Awake()
    {
        furnitureUI.SetActive(isPlacementMode);
        PopulateBackpack();
        // UpdateArrowButtons();
        //Debug.Log("Furniture list count: " + whatsMyPurpose.Instance.furnitureList.Count);
        if (furnitureDataList == null || furnitureDataList.Count == 0)
        {
            //Debug.Log("No furniture available in furnitureDataList.");
            return;
        }
        else{
            initialFurnitureDataList = new List<furnitureData>();
            foreach (var furnitureData in furnitureDataList)
            {
                // 深拷貝數據，確保是獨立的對象
                initialFurnitureDataList.Add(new furnitureData
                {
                    prefab = furnitureData.prefab,
                    icon = furnitureData.icon,
                    count = furnitureData.count
                });
            }
        }
         
    }



    void Update()
    {
        
        HandleFurnitureDragging();
        if (currentFurniture!=null){
            if (Input.GetKeyDown(KeyCode.W))
            {
                ChangeFurnitureLayer(currentFurniture, 1);
                //Debug.Log("W");
            }
            if (Input.GetKeyDown(KeyCode.S))
            {
                ChangeFurnitureLayer(currentFurniture, -1);
                //Debug.Log("S");
            }
            if (Input.GetKeyDown(KeyCode.E))
            {
                RotateUDFurniture(currentFurniture);
                //Debug.Log("E");
            }
            if (Input.GetKeyDown(KeyCode.Q))
            {
                RotateRLFurniture(currentFurniture);
                //Debug.Log("Q");
            }
      


        }

        
    }

    // 更新背包顯示
    void PopulateBackpack()
    {
        if (furnitureDataList == null || furnitureDataList.Count == 0)
        {
            // //Debug.Log("No furniture available in furnitureDataList.");
            return;
        }

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


        if (furnitureDataList == null || furnitureDataList.Count == 0)
        {
            leftArrowButton.interactable = false;
            rightArrowButton.interactable = false;
            return;
        }
    }

    // 選擇家具
    void SelectFurniture(int index)
    {
        var furniture = furnitureDataList[index];
        if (furniture.count > 0) // 檢查是否還有剩餘家具
        {
            selectedFurniturePrefab = furniture.prefab;
            // //Debug.Log($"選中了家具：{furniture.prefab.name}");
        }
        else
        {
            selectedFurniturePrefab = null;
            // //Debug.Log("該家具數量不足！");
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
            currentFurniture = Instantiate(selectedFurniturePrefab, furniturePanel.transform);
            currentFurniture.transform.localPosition = GetMousePositionInUI();


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
            currentFurniture.transform.localPosition = GetMousePositionInUI();
            if (Input.GetMouseButtonDown(0))
            {
                placedFurnitureStack.Push(currentFurniture);
                // currentFurniture.GetComponent<FurnitureIdentifier>().place();
                currentFurniture = null; // 放置完成，清空拖動家具
                var selectedData = furnitureDataList.Find(f => f.prefab == selectedFurniturePrefab);
                if (selectedData != null && selectedData.count > 0)
                {
                    selectedData.count--;
                    PopulateBackpack();
                }
                selectedFurniturePrefab = null;
            }
        }

}

private Vector2 GetMousePositionInUI()
{
    RectTransformUtility.ScreenPointToLocalPointInRectangle(
        furniturePanel.GetComponent<RectTransform>(),
        Input.mousePosition,
        null, // Screen Space - Overlay 不需要 Camera
        out Vector2 localPoint);
    return localPoint;
}



void ChangeFurnitureLayer(GameObject furniture, int direction)
{
    RectTransform rectTransform = furniture.GetComponent<RectTransform>();
    if (rectTransform != null)
    {
        // 獲取目前的索引
        int currentIndex = rectTransform.GetSiblingIndex();

        // 計算新索引
        int newIndex = Mathf.Clamp(currentIndex + direction, 0, furniturePanel.transform.childCount - 1);

        // 設定新的索引
        rectTransform.SetSiblingIndex(newIndex);

        // //Debug.Log($"家具的層級變更為：{newIndex}");

        // 更新顯示層級文字
        var currentLayerDisplay = currentLayerText.GetComponentInChildren<TextMeshProUGUI>();
        if (currentLayerDisplay != null)
        {
            currentLayerDisplay.text = $"{newIndex}";
        }
        else
        {
            // //Debug.LogWarning("找不到層級顯示文字的組件！");
        }
    }
    else
    {
        //Debug.LogWarning("家具物件缺少 RectTransform 組件，無法更改層級！");
    }
}





 // 回復上一個動作
    public void UndoLastPlacement()
    {
        if (furnitureDataList == null || furnitureDataList.Count == 0)
        {
            //Debug.Log("No furniture available in furnitureDataList.");
            return;
        }

        //Debug.Log($"堆疊狀態：{placedFurnitureStack.Count} 個家具物件");
        if (placedFurnitureStack.Count > 0)
        {
            GameObject lastPlacedFurniture = placedFurnitureStack.Pop(); // 取出最後放置的家具
            if (lastPlacedFurniture == null)
            {
                //Debug.LogWarning("最後放置的家具為 null，無法回復！");
                return;
            }

            var identifier = lastPlacedFurniture.GetComponent<FurnitureIdentifier>();
            if (identifier == null)
            {
                //Debug.LogWarning("家具缺少 FurnitureIdentifier 組件，無法更新數量！");
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
                    //Debug.LogWarning("FurnitureIdentifier.prefab 為 null，無法更新數量！");
                }
            }

            // 刪除家具
            Destroy(lastPlacedFurniture);

            // 更新 UI
            PopulateBackpack();
            }
            else
            {
                //Debug.Log("沒有可以回復的動作！");
            }
        }
    // 全部收回
    public void ClearAllFurniture()
    {
        if (furnitureDataList == null || furnitureDataList.Count == 0)
        {
            //Debug.Log("No furniture available in furnitureDataList.");
            return;
        }
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
        //Debug.Log("全部家具已收回！");
    }


    public void PlacementMode(bool isPlacementMode)
    {
        otherUI.SetActive(!isPlacementMode);
        furnitureUI.SetActive(isPlacementMode);
    
    }

public void RotateRLFurniture(GameObject furniture)
{
    // 取得家具的 RectTransform 組件
    RectTransform rectTransform = furniture.GetComponent<RectTransform>();

    // 進行水平鏡像
    Vector3 localScale = rectTransform.localScale;
    if (rectTransform.localScale.x > 0)
    {
        // 需要翻轉
        localScale.x = -localScale.x;
        rectTransform.localScale = localScale;
    }
    else
    {
        // 可以恢復到正向狀態
        localScale.x = Mathf.Abs(localScale.x);
        rectTransform.localScale = localScale;
    }

}
public void RotateUDFurniture(GameObject furniture)
{
    // 取得家具的 RectTransform 組件
    RectTransform rectTransform = furniture.GetComponent<RectTransform>();

    // 進行水平鏡像
    Vector3 localScale = rectTransform.localScale;
    
    // 翻轉 X 軸的 scale 值，實現鏡像效果
    localScale.y = -localScale.y;
    rectTransform.localScale = localScale;

}

    public void  assignPlacedFurniture(){
        for(int i = 0;i < furniturePanel.transform.childCount;i++){
            GameObject tmp = furniturePanel.transform.GetChild(i).gameObject;
            whatsMyPurpose.Instance.addPlacedFurniture(tmp,tmp.transform.GetSiblingIndex(),tmp.GetComponent<RectTransform>());
        }
    }
    public void getbug()
    {
        //Debug.Log("bug");
        string furnitureName = "bug";
        GameObject prefab = Resources.Load<GameObject>("Prefabs/bug");
        Sprite icon = Resources.Load<Sprite>("Icons/bug");
        if (inventory == null)
        {
            //Debug.LogError("FurnitureInventory is not assigned to testfurniture.");
            return;
        }
        whatsMyPurpose.Instance.AddOrUpdateFurniture(furnitureName, prefab, icon);
    }

}
