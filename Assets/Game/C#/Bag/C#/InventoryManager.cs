using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InventoryManager : MonoBehaviour
{
    public InventoryBase inventory;
    public InventoryUIBase uiManager;

    public void AddItem(ItemBase item)
    {
        bool isHave = false;
        foreach (var itemPlate in inventory.allItemPlates)
        {
            if (itemPlate.item == item)
            {
                itemPlate.count++;
                isHave = true;
            }
        }

        if (!isHave)
            inventory.allItemPlates.Add(new ItemPlate(item));
        
        //uiManager.RefreshUI(inventory);
    }

    public void RemoveItem(ItemBase item)
    {
        foreach (var itemPlate in inventory.allItemPlates)
        {
            if (itemPlate.item == item)
                itemPlate.count--;
            
            if (itemPlate.count <= 0)
            {
                inventory.allItemPlates.Remove(itemPlate);
                return;
            }
        }
        
        //uiManager.RefreshUI(inventory);
    }
}