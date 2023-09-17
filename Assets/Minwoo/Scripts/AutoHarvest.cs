using JetBrains.Annotations;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;

public class AutoHarvest : MonoBehaviour
{
    public GameObject demoItem;
    public float harvestDelay;
    public Transform cage;
    public Transform cleaner;
    public AudioClip harvestSound;

    private bool isPulling = false;
    private Vector3 cageRange;
    private List<GameObject> detectedItems = new List<GameObject>();
    private bool isAdding = false;
    private StoredItemGetter storedItemGetter;

    
    private void Awake()
    {
        storedItemGetter = GetComponent<StoredItemGetter>();
        cage = transform.parent.parent;
        StartCoroutine(AddPoolDelay());
    }

    IEnumerator AddPoolDelay()
    {
        if (!isPulling)
        {
            isPulling = true;
            yield return new WaitForSeconds(harvestDelay);

            StartCoroutine(AddPool());
            isPulling = false;
        }
        else
        {
            yield return new WaitForSeconds(harvestDelay);
            StartCoroutine(AddPool());
            isPulling = false;
        }
    }
    public IEnumerator AddPool()
    {
        if (!isAdding)
        {
            isAdding = true;
            detectedItems.Clear();
            if(!SoundManager.Instance.IsPlaying(harvestSound))
                SoundManager.Instance.Play3DSoundAtLocation(harvestSound, cleaner.position);
            for (int i = 0; i < cage.childCount; i++)
            {
                if (cage.GetChild(i).name == "Floor")
                {
                    cageRange = cage.GetChild(i).localScale;
                }
            }
            Collider[] colliders = Physics.OverlapBox(cage.GetChild(0).position, new Vector3(cageRange.x, 10F, cageRange.z));
            foreach (Collider collider in colliders)
            {
                if (collider.CompareTag("Item") && collider.gameObject.activeInHierarchy)
                {
                    detectedItems.Add(collider.gameObject);
                }
            }

            if (detectedItems.Count > 0)
            {
                for (int i = 0; i < detectedItems.Count; i++)
                {
                    GameObject item = detectedItems[i];
                    print(item.name);
                    Vector3 targetPosition = cleaner.transform.position;
                    while (Vector3.Distance(item.transform.position, targetPosition) > 0.1f)
                    {
                        item.transform.position = Vector3.MoveTowards(item.transform.position, targetPosition, storedItemGetter.pullSpeed * Time.deltaTime * 3f);
                        yield return null;
                    }
                    if (item.activeInHierarchy)
                    {
                        item.SetActive(false);
                        if (storedItemGetter.itemPool0.Count < 1)
                        {
                            storedItemGetter.itemPool0.Add(item);
                            isAdding = false;
                        }
                        else
                        {
                            storedItemGetter.itemPool1.Add(item);
                            isAdding = false;
                        }
                    }
                }
            }
            isAdding = false;


        }
            StartCoroutine(AddPoolDelay());
    }
}
