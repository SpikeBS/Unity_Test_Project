using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

public class Inventory_Window : MonoBehaviour
{
    [SerializeField] Inventory targetInventory;
    SpriteRenderer[] cells;
    [SerializeField] GameObject icon_prefab;
    SpriteRenderer icon_sprite;
    Text icon_name;
    Text icon_amount;
    // Start is called before the first frame update
    void Start()
    {
        cells = GetComponentsInChildren<SpriteRenderer>();
        icon_sprite = icon_prefab.GetComponentInChildren<SpriteRenderer>();
        icon_name = icon_prefab.GetComponentsInChildren<Text>()[0];
        icon_amount = icon_prefab.GetComponentsInChildren<Text>()[1];
    }

    public void ReDraw(Items item) 
    {
        bool isFound = false;
        foreach (var cell in cells)
        {
            if (cell.transform.childCount > 0)
            Debug.Log(cell.transform.GetChild(0).GetComponentsInChildren<Text>()[0].text);
            if (cell.transform.childCount > 0 && item.name == cell.transform.GetChild(0).GetComponentsInChildren<Text>()[0].text)
            {
                Debug.Log(item.amount.ToString());
                cell.transform.GetChild(0).GetComponentsInChildren<Text>()[1].text = item.amount.ToString();
                isFound = true;
                Debug.Log("Stack!");
                break;
            }
        }

        if (!isFound)
        {
            icon_sprite.sprite = item.icon;
            icon_name.text = item.name;
            icon_amount.text = item.amount.ToString();
            foreach (var cell in cells)
            {
                // if (cell.)
                if (cell.transform.childCount == 0)
                {
                    icon_prefab.GetComponent<RectTransform>().position = new Vector3(0, 0, 0);
                    Instantiate(icon_prefab, cell.transform);
                    break;
                }
            }
        }


       
    }
}
