using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GunItemUI : MonoBehaviour
{
    Image image;

    // Start is called before the first frame update
    void Start()
    {
        image = GetComponent<Image>();
    }

    // Update is called once per frame
    void Update()
    {
        if(Inventory.Instance.currentItem == null)
        {
            image.enabled = false;
        }
        else
        {
            if (Inventory.Instance.currentItem.item == null) 
            {
                image.enabled = false;
                return;
            }
            image.enabled = true;
            image.sprite = Inventory.Instance.currentItem.item.itemImage;
        }
    }
}
