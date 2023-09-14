using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

public class QuestManager : MonoBehaviour
{
    public QuestSlot[] quests;
    public RewardSlot[] rewards;

    ItemData[] questItems = new ItemData[4];

    List<ItemData> dataList;
    int totalQuests;
    // Start is called before the first frame update
    void Start()
    {
        quests = GetComponentsInChildren<QuestSlot>();
        rewards = GetComponentsInChildren<RewardSlot>();
        dataList = DataManager.instance.itemData;
        InitializeQuest();
        //StartCoroutine(Test());
    }
    
    IEnumerator Test()
    {
        while(true)
        {
            yield return 2f;
            InitializeQuest();
        }
    }
    public void InitializeQuest()
    {
        totalQuests = Random.Range(2, 5);
        for(int i = 0; i < totalQuests; i++)
        {
            questItems[i] = MakeRandomQuest();
            quests[i].MakeQuest(questItems[i]);
            if (questItems[i] == null)
            {
                print("Error");
                return;
            }
        }
    }

    ItemData MakeRandomQuest()
    {
        ItemData newQuestItem = dataList[Random.Range(0, dataList.Count)];
        if (newQuestItem.isBig)
        {
            return MakeRandomQuest();
        }
        for(int i = 0; i < totalQuests; i++)
        {
            if (questItems[i] == newQuestItem)
            {
                return MakeRandomQuest();
            }
        }
        for(int i = 0; i < totalQuests; i++)
        {
            if (questItems[i] == null)
            {
                return newQuestItem;
            }
        }
        return null;
    }
}
