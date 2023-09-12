using JetBrains.Annotations;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using static UnityEditor.Experimental.GraphView.GraphView;
using static UnityEditor.Progress;

public class AutoFarmer : MonoBehaviour
{
    public float delayTime;
    private bool isGenerating = false;
    private bool isPulling = false;
    public GameObject demoItem;
    public float harvestDelay;
    private List<GameObject> detectedItems = new List<GameObject>();
    public float pullSpeed;
    public Transform cage;
    public Transform cleaner;
    public Transform exit;
    private Vector3 cageRange;

    private List<GameObject> itemPool0 = new List<GameObject>();
    private List<GameObject> itemPool1 = new List<GameObject>();
    private void Awake()
    {
        cage = transform.parent.parent;
        StartCoroutine(AddPoolDelay());
    }
    public IEnumerator GenerateItem()
    {
        if (!isGenerating) // 이 조건을 추가하여 이미 생성 중인 경우 다시 생성하지 않도록 합니다.
        {

            isGenerating = true;
            yield return new WaitForSeconds(delayTime);

            if (itemPool0.Count > 0)
            {
                RemovePool(itemPool0);
            }
            else
            {
                RemovePool(itemPool1);
            }
            isGenerating = false;
        }
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

        detectedItems.Clear();
        for (int i = 0; i < cage.childCount; i++)
        {
            if (cage.GetChild(i).name == "Floor")
            {
                cageRange = cage.GetChild(i).localScale;
            }
        }
        Collider[] colliders = Physics.OverlapBox(cage.GetChild(0).position, new Vector3(cageRange.x / 2f, 10F, cageRange.z / 2f));
        for(int i = 0; i < colliders.Length; i++)
        {
            print(colliders[i].name);
        }

        foreach (Collider collider in colliders)
        {
            if (collider.CompareTag("Item") && collider.gameObject.activeInHierarchy)
            {
                detectedItems.Add(collider.gameObject);
            }
        }
        
        if (detectedItems.Count > 0)
        {
            foreach (GameObject item in detectedItems)
            {
                print(item.name);
                Vector3 targetPosition = cleaner.transform.position;
                while (Vector3.Distance(item.transform.position, targetPosition) > 0.1f)
                {
                    item.transform.position = Vector3.MoveTowards(item.transform.position, targetPosition, pullSpeed * Time.deltaTime);
                    yield return null;
                }
                if (item.activeInHierarchy)
                {
                    item.SetActive(false);
                    if (itemPool0.Count < 1)
                    {
                        itemPool0.Add(item);
                        break;
                    }
                    else
                    {
                        itemPool1.Add(item);
                        break;
                    }
                }
            }
        }
        // 검출된 아이템 리스트를 순회하거나 필요에 따라 처리

        StartCoroutine(AddPoolDelay());
    }

    private void RemovePool(List<GameObject> targetPool)
    {
        if (targetPool.Count < 1)
            return;
        targetPool[targetPool.Count - 1].SetActive(true);
        targetPool[targetPool.Count - 1].transform.position = new Vector3(exit.position.x, exit.position.y + 1, exit.position.z);
        StartCoroutine(pullToGun(targetPool[targetPool.Count - 1], Player.Instance.gunPos));
        targetPool.Remove(targetPool[targetPool.Count - 1]);
    }

    IEnumerator pullToGun(GameObject target, Transform gunPos)
    {
        while (Vector3.Distance(target.transform.position, gunPos.position) > 0.1f)
        {
            target.transform.position = Vector3.MoveTowards(target.transform.position, gunPos.position, pullSpeed * Time.deltaTime);
            yield return null;
        }
    }
}
