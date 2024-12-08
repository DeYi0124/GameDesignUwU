using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FurnitureIdentifier : MonoBehaviour
{
    public GameObject prefab;
    public RectTransform position;
    public int layerIndex;
    public bool isPlaced;
    void Start(){
        position = this.GetComponent<RectTransform>();
    }
    void Update()
    {

    }
    public void place(){
        layerIndex = position.GetSiblingIndex();
        whatsMyPurpose.Instance.addPlacedFurniture(this.gameObject,layerIndex,position);
    
    }

}
