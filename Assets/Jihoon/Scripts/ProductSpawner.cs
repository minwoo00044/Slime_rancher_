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
            timer += Time.deltaTime * 60f;
            if(timer > 720f)
            {
                isHarvested = false;
                timer = 0f;
                newProduct = Instantiate(product, gameObject.transform);
                newProduct.transform.position = transform.position;
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
