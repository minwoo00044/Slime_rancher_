using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StoredItemGetter : MonoBehaviour
{
    public float delayTime;
    private bool isGenerating = false;
    [HideInInspector]
    public List<GameObject> itemPool0 = new List<GameObject>();
    [HideInInspector]
    public List<GameObject> itemPool1 = new List<GameObject>();
    public Transform exit;
    public float pullSpeed;
    public IEnumerator GenerateItem()
    {
        print("!");
        if (!isGenerating) // �� ������ �߰��Ͽ� �̹� ���� ���� ��� �ٽ� �������� �ʵ��� �մϴ�.
        {
            print("!");
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
        target.layer = 6;
        while (Vector3.Distance(target.transform.position, gunPos.position) > 0.1f)
        {
            target.transform.position = Vector3.MoveTowards(target.transform.position, gunPos.position, pullSpeed * Time.deltaTime);
            yield return null;
        }
    }
}
