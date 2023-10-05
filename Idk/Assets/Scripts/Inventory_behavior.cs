using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class Inventory_behavior : MonoBehaviour
{
    RectTransform rt;
    private void Start()
    {
        rt = GetComponent<RectTransform>();
    }
    
    public void ShowInventory() 
    {
        rt.localPosition = new Vector3(rt.localPosition.x, 0, rt.localPosition.y);
    }

    public void CloseInventory()
    {
        rt.localPosition = new Vector3(rt.localPosition.x, 500, rt.localPosition.y);
    }
}
