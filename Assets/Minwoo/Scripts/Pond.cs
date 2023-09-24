using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class Pond : MonoBehaviour
{
    private StoredItemGetter storedItemGetter;
    public GameObject tear;
    public float waveAmount;
    public float waveDepth;
    void Awake()
    {
        storedItemGetter = GetComponent<StoredItemGetter>();
        Invoke("AutoTearGenerate", 0.5f);
    }

    private void OnTriggerStay(Collider other)
    {
        if(other.gameObject.GetComponent<Rigidbody>() != null && other.gameObject.layer == 6)
        {
            print("!");
            Rigidbody rb = other.gameObject.GetComponent<Rigidbody>();
            if(other.transform.position.y < transform.position.y - waveDepth)
            {
                float depth =  transform.position.y - other.transform.position.y; 
                rb.velocity = Vector3.zero;
                rb.AddForce((waveAmount * Vector3.up) , ForceMode.Acceleration);
            }

        }
    }

    private void AutoTearGenerate()
    {
        if (storedItemGetter.itemPool0.Count < 2)
        {
            GameObject instance = Instantiate(tear, transform, true);
            instance.transform.position = transform.position;
            storedItemGetter.itemPool0.Add(instance);
            instance.SetActive(false);
        }
        Invoke("AutoTearGenerate", 0.5f);
    }
}
