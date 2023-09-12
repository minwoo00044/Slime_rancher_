using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

[CreateAssetMenu(fileName = "New ItemData", menuName = "Item Data")]
public class ItemData : ScriptableObject
{
    public string itemName;
    public Sprite itemImage;
    public GameObject itemPrefab;
    public int itemPrice = 10;
    public int itemQuantity = 0;
    public bool isBig = false;

    
    int soldQuantity = 0;

    public ItemType itemType;
    public enum ItemType
    {
        Slime,
        Fruit,
        Vegetable,
        Animal,
        Gem
    }


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
        StatusUIManager.instance.gold += itemPrice;
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
