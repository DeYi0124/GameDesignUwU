using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class furniturePanel : MonoBehaviour
{
    public furnitureManage manager;
    // Start is called before the first frame update
    void OnDisable(){
        manager.assignPlacedFurniture();
    }
}
