using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.VisualScripting;
using UnityEditor.UIElements;
using UnityEngine;
using UnityEngine.UI;

public class RewardSlot : MonoBehaviour
{
    public ItemData item;
    int rewardAmount;
    TMP_Text reward;
    Image rewardImage;

    public GameObject rewardSpawn;

    private void Start()
    {
        reward = GetComponentInChildren<TMP_Text>();
        rewardImage = GetComponent<Image>();
        rewardImage.enabled = false;
    }
    public void MakeReward(ItemData rewardItem)
    {
        item = rewardItem;
        if (item.isGold)
        {
            rewardAmount = Random.Range(100, 300);
        }
        else
            rewardAmount = Random.Range(5, 10);
        if (item == null) print(gameObject.name);
        reward.text = rewardAmount.ToString();
        rewardImage.enabled = true;
        rewardImage.sprite = item.itemImage;
    }

    public void RemoveReward()
    {
        item = null;
        rewardAmount = 0;
        rewardImage.enabled = false;
        reward.text = "";
    }

    public void GiveReward()
    {
        if (item.isGold)
        {
            StatusUIManager.instance.gold += rewardAmount;
            return;
        }
        for (int i = 0; i < rewardAmount; i++)
        {
            GameObject rewardItem = Instantiate(item.itemPrefab);
            rewardItem.transform.position = rewardSpawn.transform.position;
        }
    }
}
