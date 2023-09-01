using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SlotItem : MonoBehaviour
{
    public ItemData item;
    Image itemImage;
    int currentAmount=0;
    public bool SetItem(ItemData newItem)
    {
        if(item != null)
        {
            if (item.name == newItem.name)
            {
                item.itemAmount++;
                return true;
            }
            else
            {
                return false;
            }
        }
        else
        {
            item = newItem;
            itemImage.enabled = true;
            itemImage.sprite = item.itemImage;
            return true;
        }
    }

    public void UseItem()
    {
        item.itemAmount--;
        useAllItems();
    }

    void useAllItems()
    {
        if(item.itemAmount <= 0)
        {
            item = null;
            itemImage.sprite = null;
            itemImage.enabled =false;
        }
    }

    private void Start()
    {
        itemImage = GetComponent<Image>();
        
    }

    private void Update()
    {
        if (item == null) return;
        if(currentAmount < item.itemAmount)
        {
            currentAmount = item.itemAmount;
            Debug.Log(item.itemName + ": " + item.itemAmount);
        }
    }
}
