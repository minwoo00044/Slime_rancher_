using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SlimeSpawner : MonoBehaviour
{
    public GameObject slime;
    public float spawnDelay = 300f;
    float spawnCountTime = 0f;
    public float spawnToSpawnTime = 1f;
    float countTime = 0f;
    public int countRange = 20;
    public int maxSlimeCount = 10;
    int slimeCount = 10;
    public float spawnPower = 2f;

    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        if (spawnDelay < spawnCountTime)
        {
            slimeCount = 0;
            Collider[] cols = Physics.OverlapSphere(transform.position, countRange);
            for (int i = 0; i < cols.Length; i++)
            {
                if (cols[i].gameObject.tag == "Slime")
                {
                    slimeCount++;
                }
            }

            spawnCountTime = 0;
        }
        else if (maxSlimeCount > slimeCount)
        {
            if (spawnToSpawnTime < countTime)
            {
                GameObject slimeGO = Instantiate(slime);
                slimeGO.transform.position = transform.position;
                Rigidbody rigidbody = slimeGO.GetComponent<Rigidbody>();
                rigidbody.AddForce(Vector3.forward * spawnPower, ForceMode.Impulse);
                countTime = 0;
                slimeCount++;
            }
            else countTime += Time.deltaTime;
        }
        else spawnCountTime += Time.deltaTime;
    }
}
