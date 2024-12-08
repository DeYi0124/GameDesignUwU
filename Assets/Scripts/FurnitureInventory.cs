using System;
using System.Collections.Generic;
using UnityEngine;

public class FurnitureInventory : MonoBehaviour
{
    public List<furnitureData> furnitureList = new List<furnitureData>(); // 儲存所有家具的列表

    public event Action OnInventoryChanged;

    // 添加或更新家具的方法
    public void AddOrUpdateFurniture(string furnitureName, GameObject prefab, Sprite icon)
    {
        if (string.IsNullOrEmpty(furnitureName))
        {
            Debug.LogError("Furniture name is invalid.");
            return;
        }
        if (prefab == null || icon == null)
        {
            Debug.LogError($"Cannot add {furnitureName}: Prefab or Icon is missing.");
            return;
        }

        // 檢查家具是否已經存在
        furnitureData existingFurniture = furnitureList.Find(f => f.furnitureName == furnitureName);

        if (existingFurniture != null)
        {
            // 如果已存在，增加數量
            existingFurniture.count++;
            Debug.Log($"Updated furniture '{furnitureName}' count to {existingFurniture.count}.");
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
            Debug.Log($"Added new furniture '{furnitureName}' with count {newFurniture.count}.");
        }

        // 觸發庫存改變事件
        OnInventoryChanged?.Invoke();
    }

    // 獲取家具清單的方法
    internal List<furnitureData> GetFurnitureList()
    {
        return furnitureList;
    }
}
