using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class testfurniture : MonoBehaviour
{
    public FurnitureInventory inventory;

    

    // Update is called once per frame
    public void getbug(bool istouch)
    {
        string furnitureName = "bug";
        GameObject prefab = Resources.Load<GameObject>("Prefabs/bug");
        Sprite icon = Resources.Load<Sprite>("Icons/bug");
if (inventory == null)
{
    Debug.LogError("FurnitureInventory is not assigned to testfurniture.");
    return;
}
        inventory.AddOrUpdateFurniture(furnitureName, prefab, icon);
    }
}
