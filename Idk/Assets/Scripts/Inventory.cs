using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Inventory : MonoBehaviour
{
    [SerializeField] Inventory_Window inventory_Window;
    public List<Items> items = new List<Items>();

    // Start is called before the first frame update
    void Start()
    {
        GlobalEventManager.OnItemTake += AddItem;
    }

    void AddItem(Items item)
    {
        Items RedrawItem = new Items() ;
        bool isNew = true;
        foreach (var item_ in items)
        {
            if (item_.name == item.name)
            {
                item_.amount += 1;
                isNew = false;
                RedrawItem = item_;
                break;
               
            }
        }
        if (isNew)
        {
            items.Add(item);
            RedrawItem = item;
        }
        inventory_Window.ReDraw(RedrawItem);
        Debug.Log(items.Count);
    }
}
