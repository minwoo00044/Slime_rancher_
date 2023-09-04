using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu]
public class ItemData : ScriptableObject
{
    public string itemName;
    public Sprite itemImage;
    public int itemGold = 10;
    public int itemAmount = 0;

    public bool CompareData(ItemData data)
    {
        if(itemName == data.itemName && itemGold == data.itemGold && itemAmount == data.itemAmount)
        {
            return true;
        }
        return false;
        
    }
}
