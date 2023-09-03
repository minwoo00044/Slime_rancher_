using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DataManager : MonoBehaviour
{
    public static DataManager instance = null;
    public string itemPath = "Items/";
    public ItemData[] itemData;
    // Start is called before the first frame update
    void Start()
    {
        if(instance == null)
        {
            instance = this;
        }
        else
        {
            Destroy(gameObject);
        }
        LoadItems();
        
    }

    void LoadItems()
    {
        Object[] loadedItems = Resources.LoadAll(itemPath, typeof(ItemData));

        itemData = new ItemData[loadedItems.Length];

        for(int i = 0; i < loadedItems.Length; i++)
        {
            itemData[i] = (ItemData)loadedItems[i];
        }
    }
}
