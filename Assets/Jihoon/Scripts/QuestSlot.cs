using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class QuestSlot : MonoBehaviour
{
    public ItemData item;
    int requiredAmount;
    int currentAmount;
    TMP_Text quest;
    List<ItemData> dataArray;
    Image questImage;
    private void Start()
    {
        quest = GetComponentInChildren<TMP_Text>();
        dataArray = DataManager.instance.itemData;
        questImage  = GetComponent<Image>();
        questImage.enabled = false;
    }
    
    private void Update()
    {
    }

    public void MakeQuest(ItemData questItem)
    {
        print(gameObject.name);
        requiredAmount = Random.Range(5,10);
        item = questItem;
        if (item == null) print(gameObject.name);
        quest.text = currentAmount + "/" + requiredAmount;
        questImage.enabled = true;
        
        questImage.sprite = item.itemImage;
    }
    public void RemoveQuest()
    {
        item = null;
        currentAmount = 0;
        requiredAmount = 0;
        questImage.enabled = false;
        quest.text = "";
    }

    public void AddCurrentItem()
    {
        currentAmount++;
        quest.text = currentAmount + "/" + requiredAmount;
    }

}
