using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

public class QuestManager : MonoBehaviour
{
    public static QuestManager instance;
    public QuestSlot[] quests;
    public RewardSlot[] rewards;

    ItemData[] questItems = new ItemData[4];
    ItemData[] rewardItems = new ItemData[4];

    List<ItemData> dataList;
    int totalQuests;
    int totalRewards;
    bool questInProgress;

    private void Awake()
    {
        if(instance == null)
        {
            instance = this;
        }
        else
        {
            Destroy(gameObject);
        }
    }
    // Start is called before the first frame update
    void Start()
    {
        quests = GetComponentsInChildren<QuestSlot>();
        rewards = GetComponentsInChildren<RewardSlot>();
        dataList = DataManager.instance.itemData;
    }

    IEnumerator Test()
    {
        yield return new WaitForSeconds(2);
        InitializeQuest();
    }

    public void Update()
    {
        QuestClear();
    }

    void QuestClear()
    {
        if (!questCheck()) return;
        questInProgress = false;
        for(int i = 0; i < totalQuests; i++)
        {
            quests[i].RemoveQuest();
            questItems[i] = null;
        }
        for(int i = 0; i< totalRewards; i++)
        {
            rewards[i].GiveReward();
            rewards[i].RemoveReward();
            rewardItems[i] = null;
        }
    }

    bool questCheck()
    {
        int count = 0;
        for(int i = 0; i < totalQuests; i++)
        {
            if (quests[i].isClear)
            {
                count++;
            }
        }
        return count == totalQuests;
    }

    public void InitializeQuest()
    {
        if(questInProgress) return;
        questInProgress = true;
        totalQuests = Random.Range(2, 5);
        totalRewards = Random.Range(2, 5);
        for(int i = 0; i < totalQuests; i++)
        {
            questItems[i] = MakeRandomQuestItem();
            quests[i].MakeQuest(questItems[i]);
            if (questItems[i] == null)
            {
                print("Error");
                return;
            }
        }
        for(int i = 0; i< totalRewards; i++)
        {
            rewardItems[i] = MakeRandomRewardItem();
            if (rewardItems[i] == null) print("err");
            rewards[i].MakeReward(rewardItems[i]);
            if (rewardItems[i] == null)
            {
                print("Error");
                return;
            }
        }
    }

    ItemData MakeRandomRewardItem()
    {
        int rand = Random.Range(0, dataList.Count);
        ItemData newRewardItem = dataList[rand];
        if (newRewardItem.isBig)
        {
            return MakeRandomRewardItem();
        }
        for(int i = 0;i < totalRewards; i++)
        {
            if (rewardItems[i] == newRewardItem)
            {
                
                return MakeRandomRewardItem();
            }
        }
        for(int i = 0; i<totalRewards; i++)
        {
            if (rewardItems[i] == null)
            {
                return newRewardItem;
            }
        }
        return null;
    }

    ItemData MakeRandomQuestItem()
    {
        ItemData newQuestItem = dataList[Random.Range(0, dataList.Count)];
        if (newQuestItem.isBig || newQuestItem.isGold)
        {
            return MakeRandomQuestItem();
        }
        for(int i = 0; i < totalQuests; i++)
        {
            if (questItems[i] == newQuestItem)
            {
                return MakeRandomQuestItem();
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

    private void OnCollisionEnter(Collision collision)
    {
        if(collision.gameObject.GetComponent<Item>() != null)
        {
            ItemData questItem = collision.gameObject.GetComponent<Item>().itemData;
            for(int i = 0; i < totalQuests; i++)
            {
                if (questItems[i] == questItem)
                {
                    if (quests[i].isClear) return;
                    //print("¿À¿¹");
                    quests[i].ReceiveItem(questItem);
                    Destroy(collision.gameObject);
                    return;
                }
            }
        }
    }
}
