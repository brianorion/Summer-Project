using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Inventory : MonoBehaviour
{
    // this is a list of items that a player will have. 
    public List<Item> items = new List<Item>();

    public Item GetItem()
    {
        if(items.Count != 0)
        {
            return items[0];
        }
        return null;
    }


}
