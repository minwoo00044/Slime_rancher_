using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MarketTrade : MonoBehaviour
{
    MarketItem[] sellingItems;
    // Start is called before the first frame update
    void Start()
    {
        sellingItems = GetComponentsInChildren<MarketItem>();
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.CompareTag("Item"))
        {
            for(int i = 0; i < sellingItems.Length; i++)
            {
                if (collision.gameObject.GetComponent<Item>().itemData.CompareData(sellingItems[i].item))
                {
                    sellingItems[i].item.SellItem();
                    Destroy(collision.gameObject);
                }
            }
        }
    }
}
