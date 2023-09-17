using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class SlotItem : MonoBehaviour
{
    public ItemData item;
    public TMP_Text itemAmountText;
    public Sprite noItemImage; 
    Image itemImage;
    int currentAmount=0;
    public bool isWater;
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
            if(itemImage == null)
            {
                print("???????????");
            }
            if(itemImage.sprite == null)
            {
                print(item.itemName);
                print(1);
            }
            else if(item.itemImage == null)
            {
                print(item.itemName);
                print(2);
            }
            itemImage.sprite = item.itemImage;
            return true;
        }
    }

    public void UseItem()
    {
        if (item == null)
            return;
        item.itemQuantity--;
        if (item.itemQuantity < 0)
        {
            item.itemQuantity = 0;
        }
        itemAmountText.text = "x " + item.itemQuantity;
        useAllItems();
    }

    void useAllItems()
    {
        if (isWater) return;
        if(item.itemQuantity <= 0)
        {
            item = null;
            itemImage.sprite = null;
            itemAmountText.text = "";
        }
    }

    public void OnWaterTankUI()
    {
        if (item.itemQuantity <= 0) item.itemQuantity = 0;
        itemAmountText.text = "x " + item.itemQuantity;
    }

    private void Awake()
    {
        itemImage = GetComponent<Image>();
    }

    private void Update()
    {
        //이하는 전부 테스트용 코드
        if (item == null)
        {
            itemImage.sprite = noItemImage;
            return;
        }
        if(currentAmount != item.itemQuantity)
        {
            currentAmount = item.itemQuantity;
            //Debug.Log(item.itemName + ": " + item.itemAmount);
        }
    }
}
