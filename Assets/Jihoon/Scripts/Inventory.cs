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
    public Item[] itemTest = new Item[4];
    private PlayerFire playerFire;
    public Sprite onSlotImg;
    public Sprite offSlotImg;
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
        //StartCoroutine(InventoryTest());
        playerFire = GameObject.FindGameObjectWithTag("Player").GetComponent<PlayerFire>();
        LoadInventory();
    }
    // Update is called once per frame
    void Update()
    {
        if (MenuManager.instance.isPaused) return;
        
        if (Input.GetMouseButtonDown(1))
        {
            //if (currentItem.item != null)
            //{
            //    currentItem.UseItem();
            //}
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

    void LoadInventory()
    {
        for(int i =0; i<4; i++)
        {
            if(PlayerPrefs.HasKey("Slot" + i))
            {
                string name = PlayerPrefs.GetString("Slot" + i);
                ItemData savedItem = DataManager.instance.FindDataByName(name);
                if(savedItem == null)
                {
                    return;
                }
                savedItem.itemQuantity--;
                AddItemToInventory(savedItem);
            }
            else
            {
                print("Key°¡ ¾øÀ½");
                return;
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


    Image GetCurrentSlot()
    {
        int index = (int)key - (int)KeyCode.Alpha1;
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
        }
        if (index >= 0 && index < inventorySlots.Length)
        {
            currentIndex = index;
            return inventorySlots[index];
        }
        return null;
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
        if ((int)key >= 49 && (int)key < 52) return;
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
        KeyCode key = GetLastPressedKeyCode();
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
        if (currentItem != null)
        {
            return currentItem;
        }
        return null;
    }

    public void AddItemToInventory(ItemData newItem)
    {
        SlotItem[] slotItems = GetComponentsInChildren<SlotItem>();
        foreach (ItemData data in DataManager.instance.itemData)
        {
            if (newItem.itemName == data.itemName)
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
    }

    void OnApplicationQuit()
    {
        for(int i = 0; i < inventorySlots.Length;i++)
        {

        }
    }

    IEnumerator InventoryTest()
    {
        while (true)
        {
            yield return new WaitForSeconds(1.5f);
            ItemData newItem = itemTest[Random.Range(0, 4)].itemData;
            AddItemToInventory(newItem);
        }
    }
}
