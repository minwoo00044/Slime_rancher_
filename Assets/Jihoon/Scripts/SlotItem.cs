using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class SlotItem : MonoBehaviour
{
    public ItemData item;
    public TMP_Text itemAmountText;
    Image itemImage;
    int currentAmount=0;
    public bool SetItem(ItemData newItem)
    {
        if(item != null)
        {
            if (item.name == newItem.name)
            {
                item.itemQuantity++;
                itemAmountText.text = "x " + item.itemQuantity;
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
            item.itemQuantity++;
            itemAmountText.text = "x " + item.itemQuantity;
            itemImage.enabled = true;
            itemImage.sprite = item.itemImage;
            return true;
        }
    }

    public void UseItem()
    {
        item.itemQuantity--;
        itemAmountText.text = "x " + item.itemQuantity;
        Debug.Log(item.itemName + ": " + item.itemQuantity);
        useAllItems();
    }

    void useAllItems()
    {
        if(item.itemQuantity <= 0)
        {
            item = null;
            itemImage.sprite = null;
            itemImage.enabled =false;
            itemAmountText.text = "";
        }
    }

    private void Start()
    {
        itemImage = GetComponent<Image>();
    }

    private void Update()
    {
        //이하는 전부 테스트용 코드
        if (item == null) return;
        if(currentAmount != item.itemQuantity)
        {
            currentAmount = item.itemQuantity;
            //Debug.Log(item.itemName + ": " + item.itemAmount);
        }
    }
}
