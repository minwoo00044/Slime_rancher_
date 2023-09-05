using System.Collections;
using System.Collections.Generic;
using System.Runtime.InteropServices.WindowsRuntime;
using UnityEngine;
using UnityEngine.UI;

public class Inventory : MonoBehaviour
{
    public static Inventory Instance;

    public Image[] inventorySlots;
    [SerializeField]
    Image currentSlot;
    [HideInInspector]
    public SlotItem currentItem;

    public Item[] itemTest = new Item[4];

    int currentIndex = 0;
    // Start is called before the first frame update
    void Start()
    {
        if(Instance == null)
        {
            Instance = this;
        }
        else
        {
            Destroy(gameObject);
        }
        StartCoroutine(InventoryTest());
    }

    // Update is called once per frame
    void Update()
    {
        if (MenuManager.instance.isPaused) return;
        if (Input.GetKeyDown(KeyCode.Space))
        {
            if( currentItem.item != null)
            {
                currentItem.UseItem();
            }
        }
        else if (Input.anyKeyDown)
        {
            SelectSlot();
        }

        if (Input.GetAxis("Mouse ScrollWheel") != 0f)
        {
            Debug.Log("Mouse Wheel Scrolled");
        }
    }

    KeyCode GetLastPressedKeyCode()
    {
        foreach(KeyCode key in System.Enum.GetValues(typeof(KeyCode)))
        {
            if (Input.GetKeyDown(key))
            {
                currentItem = GetCurrentItem();
                return key;
            }
        }
        return KeyCode.None;
    }


    Image GetCurrentSlot()
    {
        KeyCode key = GetLastPressedKeyCode();
        int index = (int)key - (int)KeyCode.Alpha1;
        if (index >= 0 && index < inventorySlots.Length)
        {
            currentIndex = index;
            return inventorySlots[index];
        }
        return null;
    }



    void ChangeSlotImage()
    {
        currentSlot.gameObject.GetComponent<RectTransform>().sizeDelta = new Vector2(200, 200);
    }

    void DeselectSlot()
    {
        currentSlot.GetComponent<RectTransform>().sizeDelta = new Vector2(150, 150);
        
        currentSlot = null;
    }

    bool OnSlotSelected()
    {
        return currentSlot != null;
    }

    void SelectSlot()
    {
        if (currentSlot != null)
        {
            DeselectSlot();
        }
        currentSlot = GetCurrentSlot();
        currentItem = GetCurrentItem();
        if (currentSlot != null)
        {
            ChangeSlotImage();
        }
    }

    SlotItem GetCurrentItem()
    {
        if (currentSlot == null)
        {
            return null;
        }
        currentItem = currentSlot.GetComponentInChildren<SlotItem>();
        if(currentItem != null)
        {
            return currentItem;
        }
        return null;
    }

    public void AddItemToInventory(ItemData newItem)
    {
        SlotItem[] slotItems = GetComponentsInChildren<SlotItem>();
        foreach(ItemData data in DataManager.instance.itemData)
        {
            if(newItem.itemName == data.itemName)
            {
                for(int i = 0; i< inventorySlots.Length; i++)
                {
                    if (slotItems[i].SetItem(newItem))
                        return;
                }
                
            }
        }
    }
    IEnumerator InventoryTest()
    {
        while(true)
        {
            yield return new WaitForSeconds(1.5f);
            ItemData newItem = itemTest[Random.Range(0,4)].itemData;
            AddItemToInventory(newItem);
        }
    }
}
