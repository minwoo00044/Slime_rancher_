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
                item.itemAmount++;
                itemAmountText.text = "x " + item.itemAmount;
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
            item.itemAmount++;
            itemAmountText.text = "x " + item.itemAmount;
            itemImage.enabled = true;
            itemImage.sprite = item.itemImage;
            return true;
        }
    }

    public void UseItem()
    {
        item.itemAmount--;
        itemAmountText.text = "x " + item.itemAmount;
        Debug.Log(item.itemName + ": " + item.itemAmount);
        useAllItems();
    }

    void useAllItems()
    {
        if(item.itemAmount <= 0)
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
        if(currentAmount != item.itemAmount)
        {
            currentAmount = item.itemAmount;
            //Debug.Log(item.itemName + ": " + item.itemAmount);
        }
    }
}
