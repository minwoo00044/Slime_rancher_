using System.Collections;
using System.Collections.Generic;
using System.Runtime.InteropServices.WindowsRuntime;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

public class Inventory : MonoBehaviour
{
    public static Inventory Instance;

    public Image[] inventorySlots;
    
    public Image currentSlot;
    [HideInInspector]
    public SlotItem currentItem;
    int currentIndex;
    private PlayerFire playerFire;
    public Sprite onSlotImg;
    public Sprite offSlotImg;

    SlotItem[] slotItems = new SlotItem[5];
    KeyCode key;

    // Start is called before the first frame update
    void Start()
    {
        if (Instance == null)
        {
            Instance = this;
        }
        else
        {
            Destroy(gameObject);
        }
        slotItems = GetComponentsInChildren<SlotItem>(true);

        playerFire = GameObject.FindGameObjectWithTag("Player").GetComponent<PlayerFire>();
        LoadInventory();

        key = KeyCode.Alpha1;
        SelectSlot();
    }
    // Update is called once per frame
    void Update()
    {
        if (MenuManager.instance.isPaused) return;
        
        if (Input.GetMouseButtonDown(0))
        {
            
        }
        else if (Input.anyKeyDown)
        {
            key = GetLastPressedKeyCode();
            SelectSlot();
        }

        if (Input.GetAxis("Mouse ScrollWheel") != 0f)
        {
            Debug.Log("Mouse Wheel Scrolled");
        }

        
    }

    public void UseItem()
    {
        if (currentItem != null)
        {
            currentItem.UseItem();
        }
        if (currentIndex == 4) return;
        if (slotItems[currentIndex] == null)
        {
            PlayerPrefs.DeleteKey("Slot" + currentIndex);
        }
        for (int i = 0; i < slotItems.Length -1; i++)
        {
            if (slotItems[i].item == null)
            {
                PlayerPrefs.DeleteKey("Slot" + i);
            }
        }
    }

    void LoadInventory()
    {
        for (int i =0; i<inventorySlots.Length; i++)
        {
            if(PlayerPrefs.HasKey("Slot" + i))
            {
                string name = PlayerPrefs.GetString("Slot" + i);
                ItemData savedItem = DataManager.instance.FindDataByName(name);
                if (name == "Water")
                    print(savedItem.itemName);
                if(savedItem == null)
                {
                    continue;
                }
                savedItem.itemQuantity--;
                slotItems[i].SetItem(savedItem);
               // AddItemToInventory(savedItem);
                playerFire.InitializePool(i, savedItem, savedItem.itemQuantity);
            }
        }
    }

    KeyCode GetLastPressedKeyCode()
    {
        foreach (KeyCode key in System.Enum.GetValues(typeof(KeyCode)))
        {
            if (Input.GetKeyDown(key))
            {
                currentItem = GetCurrentItem();
                return key;
            }
        }
        return KeyCode.None;
    }

    void ChangeSlotImage()
    {
        RectTransform rect = currentSlot.GetComponent<RectTransform>();
        Transform[] currentSlotChildren = currentSlot.GetComponentsInChildren<Transform>();
        foreach(var currentSlotChild in currentSlotChildren)
        {
            RectTransform rectChild = currentSlotChild.GetComponent<RectTransform>();
            rectChild.anchoredPosition = new Vector2(rectChild.anchoredPosition.x, 30);
            if(rectChild.GetComponent<TMP_Text>() != null)
            {
                rectChild.anchoredPosition = new Vector2(rectChild.anchoredPosition.x, -33);
            }
        }
        rect.anchoredPosition = new Vector2(rect.anchoredPosition.x, 160);
        rect.sizeDelta = new Vector2(150, 210);
        currentSlot.sprite = onSlotImg;
    }

    void DeselectSlot()
    {
        RectTransform rect = currentSlot.GetComponent<RectTransform>();
        Transform[] currentSlotChildren = currentSlot.GetComponentsInChildren<Transform>();
        foreach (var currentSlotChild in currentSlotChildren)
        {
            RectTransform rectChild = currentSlotChild.GetComponent<RectTransform>();
            rectChild.anchoredPosition = new Vector2(rectChild.anchoredPosition.x, 20);
            if (rectChild.GetComponent<TMP_Text>() != null)
            {
                rectChild.anchoredPosition = new Vector2(rectChild.anchoredPosition.x, -33);
            }
        }

        rect.anchoredPosition = new Vector2(rect.anchoredPosition.x, 130);
        rect.sizeDelta = new Vector2(150, 180);
        currentSlot.sprite = offSlotImg;

        currentSlot = null;
    }

    bool OnSlotSelected()
    {
        return currentSlot != null;
    }

    void SelectSlot()
    {
        if ((int)key < 49 || (int)key > 53) return;
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
    Image GetCurrentSlot()
    {
        int index = (int)key - (int)KeyCode.Alpha1;
        currentIndex = index;
        switch (index)
        {
            case 0:
                playerFire.bulletState = PlayerFire.BulletState.Slot0;
                break;
            case 1:
                playerFire.bulletState = PlayerFire.BulletState.Slot01;
                break;
            case 2:
                playerFire.bulletState = PlayerFire.BulletState.Slot02;
                break;
            case 3:
                playerFire.bulletState = PlayerFire.BulletState.Slot03;
                break;
            case 4:
                if (Player.Instance.isEquipWaterTank == true)
                    playerFire.bulletState = PlayerFire.BulletState.Slot04;
                break;
        }
        if (index >= 0 && index < inventorySlots.Length)
        {
            currentIndex = index;
            return inventorySlots[index];
        }
        return null;
    }

    SlotItem GetCurrentItem()
    {
        if (currentSlot == null)
        {
            return null;
        }
        currentItem = currentSlot.GetComponentInChildren<SlotItem>();
        if (currentItem != null)
        {
            return currentItem;
        }
        return null;
    }

    public void AddItemToInventory(ItemData newItem)
    {
        if(newItem.itemName == "Water")
        {
            if (Player.Instance.isEquipWaterTank)
            {
                print("water");
                slotItems[4].SetItem(newItem);
                PlayerPrefs.SetString("Slot" + 4, newItem.itemName);
            }
            return;
        }
        int slotIndex = CheckInventory(newItem);
        if (slotIndex != -1)
        {
            slotItems[slotIndex].SetItem(newItem);
            PlayerPrefs.SetString("Slot" + slotIndex, newItem.itemName);
            return;
        }
        else
        {
            for (int i = 0; i < inventorySlots.Length; i++)
            {
                if (slotItems[i].SetItem(newItem))
                {
                    PlayerPrefs.SetString("Slot" + i, newItem.itemName);
                    return;
                }
            }
        }
    }

    int CheckInventory(ItemData itemData)
    {
        for(int i = 0; i < slotItems.Length; i++)
        {
            if (slotItems[i].item == null) continue;
            if (slotItems[i].item.CompareData(itemData))
            {
                return i;
            }
        }
        return -1;
    }

    public void CreateWaterTank()
    {
        inventorySlots[4].gameObject.SetActive(true);
        slotItems[4].OnWaterTankUI();
        for(int i =0; i<4; i++)
        {
            RectTransform rect = inventorySlots[i].GetComponent<RectTransform>();
            rect.anchoredPosition = new Vector2(rect.anchoredPosition.x - 75, rect.anchoredPosition.y);
        }

    }

}
