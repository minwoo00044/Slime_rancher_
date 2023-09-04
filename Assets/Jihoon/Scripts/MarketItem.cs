using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class MarketItem : MonoBehaviour
{
    Image itemImage;
    TMP_Text priceText;

    public ItemData item;

    // Start is called before the first frame update
    void Start()
    {
        itemImage = GetComponent<Image>();
        itemImage.sprite = item.itemImage;
        priceText = GetComponentInChildren<TMP_Text>();
    }

    // Update is called once per frame
    void Update()
    {
        priceText.text = item.itemPrice.ToString();
    }
}
