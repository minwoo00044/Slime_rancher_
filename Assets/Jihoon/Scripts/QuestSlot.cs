using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class QuestSlot : MonoBehaviour
{
    public ItemData item;
    int requiredAmount;
    int currentAmount= -1;
    TMP_Text quest;
    Image questImage;

    public bool isClear = false;

    private void Start()
    {
        quest = GetComponentInChildren<TMP_Text>();
        questImage  = GetComponent<Image>();
        questImage.enabled = false;
    }
    
    private void Update()
    {
        if(currentAmount == requiredAmount && !isClear)
        {
            isClear = true;
        }
    }

    public bool ReceiveItem(ItemData questItem)
    {
        if(item == questItem)
        {
            currentAmount++; 
            quest.text = currentAmount + "/" + requiredAmount;
            return true;
        }
        return false;
    }

    public void MakeQuest(ItemData questItem)
    {
        currentAmount = 0;
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
        currentAmount = -1;
        requiredAmount = 0;
        questImage.enabled = false;
        quest.text = "";
        isClear = false;
    }

    public void AddCurrentItem()
    {
        currentAmount++;
        quest.text = currentAmount + "/" + requiredAmount;
    }

}
