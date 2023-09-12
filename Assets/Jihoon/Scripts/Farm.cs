using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Farm : MonoBehaviour
{
    public float timer;
    public GameObject product;
    bool isHarvested;

    GameObject newProduct;
    // Start is called before the first frame update
    void Start()
    {
            
    }

    // Update is called once per frame
    void Update()
    {
        if(isHarvested)
        {
            timer += Time.deltaTime * 360f;
            if(timer > 720f)
            {
                isHarvested = false;
                timer = 0f;
                if(product.GetComponentInChildren<Item>().itemData.itemType == ItemData.ItemType.Fruit)
                {
                    newProduct = Instantiate(product, transform.position, Quaternion.Euler(-90, 0, 0));
                }
                else if (product.GetComponentInChildren<Item>().itemData.itemType == ItemData.ItemType.Vegetable)
                {
                    newProduct = Instantiate(product, transform.position - new Vector3(0, 0.8f, 0), Quaternion.identity);
                }
                newProduct.transform.SetParent(this.transform);
            }
        }
        else
        {
            if (transform.childCount == 0)
            {
                isHarvested = true;
            }
            if(newProduct != null)
            {
                if(newProduct.activeSelf == false)
                {
                    newProduct.transform.SetParent(null);
                    newProduct = null;
                }
            }
        }
    }

}
