using UnityEngine;

[CreateAssetMenu(fileName = "FurnitureData", menuName = "FurnitureData")]
public class furnitureData : ScriptableObject
{
    public string furnitureName;
    public GameObject prefab;   
    public Sprite icon;         
    public int count;           
}
