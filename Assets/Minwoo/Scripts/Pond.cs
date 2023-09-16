using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class Pond : MonoBehaviour
{
    private StoredItemGetter storedItemGetter;
    public GameObject tear;
    void Awake()
    {
        storedItemGetter = GetComponent<StoredItemGetter>();
        Invoke("AutoTearGenerate", 0.5f);
    }

    private void AutoTearGenerate()
    {
        if (storedItemGetter.itemPool0.Count < 2)
        {
            GameObject instance = Instantiate(tear, transform, true);
            instance.transform.position = transform.position;
            storedItemGetter.itemPool0.Add(instance);
            instance.SetActive(false);
            print(storedItemGetter.itemPool0.Count);
        }
        Invoke("AutoTearGenerate", 0.5f);
    }
}
