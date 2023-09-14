using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;

public class DataManager : MonoBehaviour
{
    public string jsonFilePath = "itemData.json";
    public static DataManager instance = null;
    public string itemPath = "Items/";
    public List<ItemData> itemData;
    public List<ItemData> resourceItems;
    

    private void Awake()
    {
        //PlayerPrefs.DeleteAll();
        if (instance == null)
        {
            instance = this;
        }
        else
        {
            Destroy(gameObject);
        }

        jsonFilePath = Application.persistentDataPath + "/itemData.json"; // JSON 파일 경로 설정
        LoadData(); 
    }

    private void Start()
    {
        Object[] loadedItems = Resources.LoadAll(itemPath, typeof(ItemData));

        for (int i = 0; i < loadedItems.Length; i++)
        {
            //itemData.Add((ItemData)loadedItems[i]);
            //print(resourceItems[i].itemName + ": " + resourceItems[i].itemQuantity);
        }
        //LoadItemsFromJson();
    }

    private void Update()
    {
        if(Input.GetKey(KeyCode.P)&& Input.GetKey(KeyCode.O))
        {
            foreach(ItemData data in itemData)
            {
                data.itemQuantity = 0;
                PlayerPrefs.DeleteAll();
            }
        }
    }

    public ItemData FindDataByName(string dataName)
    {
        foreach(ItemData item in itemData)
        {
            if(item.itemName == dataName)
            {
                if (item.itemQuantity <= 0) return null;
                return item;
            }
        }
        return null;
    }

    public void SaveData()
    {
        int i = 0;
        foreach(ItemData item in itemData)
        {
            int newQuantity = item.itemQuantity;
            int newPrice = item.itemPrice;
            
            PlayerPrefs.SetInt("Quantity" + i, newQuantity);
            PlayerPrefs.SetInt("Price" + i, newPrice);
            i++;
        }
    }
    public void LoadData()
    {
        int i = 0;
        foreach(ItemData item in itemData)
        {
            if (PlayerPrefs.HasKey("Quantity" + i))
            {
                item.itemQuantity = PlayerPrefs.GetInt("Quantity" + i);
                item.itemPrice = PlayerPrefs.GetInt("Price" + i);
            }
            i++;
        }
    }

    private void OnApplicationQuit()
    {
        SaveData();
    }

    //void SaveItemsToJson()
    //{
    //    List<string> jsonDataList = new List<string>();

    //    foreach (ItemData Data in itemData)
    //    {
    //        jsonDataList.Add(Data.ToJson());
    //    }

    //    print("save");
    //    string jsonData = string.Join("\n", jsonDataList.ToArray());
    //    File.WriteAllText(jsonFilePath, jsonData);
    //    //string jsonData = JsonUtility.ToJson(itemData);
    //    //File.WriteAllText(jsonFilePath, jsonData);
    //    //print("Save: " + jsonFilePath);
    //}

    //void LoadItemsFromJson()
    //{
    //    itemData.Clear();

    //    if (File.Exists(jsonFilePath))
    //    {
    //        string[] jsonLines = File.ReadAllLines(jsonFilePath);
    //        foreach (string jsonLine in jsonLines)
    //        {
    //            ItemData loadedData = ItemData.FromJson(jsonLine);
    //            itemData.Add(loadedData);
    //        }
    //    }
    //    else
    //    {
    //        Debug.LogWarning("JSON 파일이 존재하지 않습니다.");
    //    }
    //    int i = 0;
    //    foreach (ItemData Data in itemData)
    //    {
    //        resourceItems[i].itemPrice = Data.itemPrice;
    //        resourceItems[i].itemQuantity = Data.itemQuantity;
    //        print(resourceItems[i].itemName + ": " + resourceItems[i].itemQuantity);
    //        i++;
    //    }
    //}

    //public void SaveItems()
    //{
    //    SaveItemsToJson();
    //}

    //public void LoadItems()
    //{
    //    LoadItemsFromJson();
    //}
    void LoadItemsInEditor()
    {
        itemData.Clear();

        Object[] loadedItems = Resources.LoadAll(itemPath, typeof(ItemData));

        for (int i = 0; i < loadedItems.Length; i++)
        {
            itemData.Add((ItemData)loadedItems[i]);
        }
    }
    private void OnValidate()
    {
        //LoadItemsInEditor();
    }
}
