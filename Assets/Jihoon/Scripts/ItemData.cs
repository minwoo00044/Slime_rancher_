using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

[CreateAssetMenu]
public class ItemData : ScriptableObject
{
    public string itemName;
    public Sprite itemImage;
    public int itemPrice = 10;
    public int itemQuantity = 0;
    int soldQuantity = 0;

    public bool CompareData(ItemData data)
    {
        if(itemName == data.itemName && itemPrice == data.itemPrice && itemQuantity == data.itemQuantity)
        {
            return true;
        }
        return false;
    }

    public void SellItem()
    {
        if(itemQuantity >0)
        {
            itemQuantity--;
        }
        soldQuantity++;
        Debug.Log("판매성공");
    }

    public void PriceChange()
    {
        if(soldQuantity >= 20)
        {
            itemPrice = (int)Mathf.Floor(itemPrice * 0.9f);
        }
    }
}
