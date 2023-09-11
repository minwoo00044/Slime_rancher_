using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FarmMachine : MonoBehaviour
{
    public GameObject vegetableSpawners;
    public GameObject fruitSpawners;

    public GameObject fruit;
    public GameObject vegetable;

    bool isGrowing = false;


    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (isGrowing) return;
        if(collision.gameObject.GetComponent<Item>() != null)
        {
            ItemData.ItemType itemType = collision.gameObject.GetComponent<Item>().itemData.itemType;
            if (itemType == ItemData.ItemType.Vegetable)
            {
                vegetableSpawners.SetActive(true);
                Farm[] spawners = vegetableSpawners.gameObject.GetComponentsInChildren<Farm>();
                foreach(Farm farm in spawners)
                {
                    farm.product = vegetable;
                }
                isGrowing = true;
            }
            if(itemType == ItemData.ItemType.Fruit)
            {
                fruitSpawners.SetActive(true);
                Farm[] spawners = fruitSpawners.gameObject.GetComponentsInChildren<Farm>(); 
                foreach( Farm farm in spawners)
                {
                    farm.product = fruit;
                }
                isGrowing = true;
            }

        }
    }
}
