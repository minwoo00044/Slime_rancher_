using JetBrains.Annotations;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class AutoFarmer : MonoBehaviour
{
    public float delayTime;
    private bool isGenerating = false;
    private List<GameObject> haveItemPool = new List<GameObject>();
    public GameObject demoItem;
    private void Awake()
    {
        for(int i = 0; i < 5; i++)
        {
            GameObject instance = Instantiate(demoItem);
            instance.transform.position = transform.position;
            instance.transform.rotation = transform.rotation;
            instance.SetActive(false);
            haveItemPool.Add(instance);
        }
    }
    public IEnumerator GenerateItem()
    {
        if (!isGenerating) // �� ������ �߰��Ͽ� �̹� ���� ���� ��� �ٽ� �������� �ʵ��� �մϴ�.
        {
            isGenerating = true;
            yield return new WaitForSeconds(delayTime);
            print(haveItemPool.Count);
            if(haveItemPool.Count > 0)
            {
                RemovePool();
            }
            isGenerating = false;
        }
    }
    private void AddPool()
    {
    }
    private void RemovePool()
    {
        if (haveItemPool[haveItemPool.Count - 1] == null)
            return;
        haveItemPool[haveItemPool.Count - 1].SetActive(true);
        haveItemPool.Remove(haveItemPool[haveItemPool.Count - 1]);
    }
}
