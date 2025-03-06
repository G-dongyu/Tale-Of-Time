using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InventorySample : MonoBehaviour
{
    public InventoryManager inventoryManager;
    public ItemBase item;
    // Start is called before the first frame update
    void Start()
    {
        inventoryManager.RemoveItem(item);
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
