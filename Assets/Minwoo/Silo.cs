using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Silo : MonoBehaviour
{
    private StoredItemGetter storedItemGetter;
    private List<GameObject> items0;
    private List<GameObject> items1;


    private void Awake()
    {
        storedItemGetter = GetComponent<StoredItemGetter>();
        items0 = storedItemGetter.itemPool0;
        items1 = storedItemGetter.itemPool1;
    }

    private void OnCollisionEnter(Collision other)
    {

        if (other.gameObject.GetComponent<Item>() != null)
        {
            string itemName = other.gameObject.GetComponent<Item>().itemData.name;

            //ù ���� �ΰ���?
            if (items0.Count == 0)
            {
                AddPool(other.gameObject);
            }
            //�ƴϸ� ����Ǿ� �ִ� ������Ʈ�� �̰Ŷ� ������?
            else if (items0[0].GetComponent<Item>().itemData.name == itemName)
            {
                AddPool(other.gameObject);
            }
        }
    }

    private void AddPool(GameObject target)
    {
        items0.Add(target.gameObject);
        target.gameObject.SetActive(false);
        target.gameObject.layer = 0;
    }
}
