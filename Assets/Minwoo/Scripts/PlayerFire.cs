using System;
using System.Collections;
using System.Collections.Generic;
using UnityEditor.Rendering;
using UnityEngine;
using static UnityEngine.GraphicsBuffer;

public class PlayerFire : MonoBehaviour
{
    [HideInInspector]
    public List<GameObject> itemPool0 = new List<GameObject>();
    [HideInInspector]
    public List<GameObject> itemPool1 = new List<GameObject>();
    [HideInInspector]
    public List<GameObject> itemPool2 = new List<GameObject>();
    [HideInInspector]
    public List<GameObject> itemPool3 = new List<GameObject>();

    //public List<GameObject> itemPool0 = new List<GameObject>();
    //public List<GameObject> itemPool1 = new List<GameObject>();
    //public List<GameObject> itemPool2 = new List<GameObject>();
    //public List<GameObject> itemPool3 = new List<GameObject>();

    public float maxDistance = 5f;
    public float radius = 2.0f;
    public float pullSpeed = 5f;
    public LayerMask pullableObjectsLayer;
    public LayerMask autoFarmObjectsLayer;
    public Transform gunPos;
    public GameObject pullEff;
    public float bulletForce;
    public float addDistance = 0.5f;

    private bool isPulling = false;
    [SerializeField]
    private GameObject objectToPull;

    public GameObject bingle;
    public int angle = 50;

    public Renderer[] render;
    public float intensity;
    private bool isShootCool = false;
    public float delayTime = 0.3f;

    Animator animator;
    public enum BulletState
    {
        Slot0,
        Slot01,
        Slot02,
        Slot03
    }
    public BulletState bulletState = BulletState.Slot0;
    public Dictionary<BulletState, List<GameObject>> bulletSlot = new Dictionary<BulletState, List<GameObject>>();
    private void Awake()
    {
        animator = GetComponent<Animator>();
        bulletSlot.Add(BulletState.Slot0, itemPool0);
        bulletSlot.Add(BulletState.Slot01, itemPool1);
        bulletSlot.Add(BulletState.Slot02, itemPool2);
        bulletSlot.Add(BulletState.Slot03, itemPool3);
    }
    private void Update()
    {
        if(!animator.GetBool("isRun"))
        {
            if (Input.GetMouseButton(1))
            {
                animator.SetBool("isShoot", true);
                EmissionChange(Color.green, angle, 1);
                BulletCheck(bulletState);
            }
            else
            {
                animator.SetBool("isShoot", false);
            }
            if (Input.GetMouseButton(0))
            {
                PullObject();
                pullEff.SetActive(true);
            }
            else
            {
                pullEff.SetActive(false);
            }
        }

    }

    private void BulletCheck(BulletState currentState)
    {
        if(bulletSlot.ContainsKey(currentState)) 
        {
            List<GameObject> currentPool = bulletSlot[currentState];
            if(currentPool.Count > 0)
            {
                if (currentPool[currentPool.Count - 1 ] == null)
                    return;
                StartCoroutine(Fire(currentPool[currentPool.Count - 1], currentPool));

            }
        }
    }

    IEnumerator Fire(GameObject bullet, List<GameObject> _currentPool)
    {
        if (bullet != null)
        {
            if(!isShootCool)
            {
                isShootCool = true;
               
                Rigidbody bulletRigidbody = bullet.GetComponent<Rigidbody>();

                if (bulletRigidbody != null)
                {
                    bullet.SetActive(true);
                    _currentPool.Remove(_currentPool[_currentPool.Count - 1]);
                    bullet.transform.position = gunPos.transform.position;
                    bullet.transform.rotation = gunPos.transform.rotation;
                    Vector3 forceDirection = Camera.main.transform.forward;
                    if (bullet != null)
                    {
                        bulletRigidbody.AddForce(forceDirection * bulletForce, ForceMode.Impulse);
                        if(Inventory.Instance.currentItem != null)
                            Inventory.Instance.currentItem.UseItem();
                    }
                }
                yield return new WaitForSeconds(delayTime);
                isShootCool = false;
            }

        }

    }
    private void EmissionChange(Color color, float angle, int dir)
    {
        bingle.transform.Rotate(0, 0, dir * angle);
        for (int i = 0; i < render.Length; i++)
        {
            render[i].material.SetColor("_EmissionColor", color * intensity);
        }
    }
    private void PullObject()
    {
        Ray ray = Camera.main.ScreenPointToRay(new Vector3(Screen.width / 2, Screen.height / 2, 0)); // Ray from the center of the camera view
        Vector3 center = Camera.main.ScreenToWorldPoint(new Vector3(Screen.width / 2, Screen.height / 2, 0)); // 화면 중심을 월드 좌표로 변환
                                                                                                              // Spherecast의 반지름 설정
        EmissionChange(new Color(0, 1, 1), angle, -1);
        if (Physics.SphereCast(center, radius, ray.direction, out RaycastHit hit, maxDistance, pullableObjectsLayer | autoFarmObjectsLayer))
        {
            objectToPull = hit.collider.gameObject;
            isPulling = true;
        }


        if (isPulling && objectToPull != null && objectToPull.layer == 6)
        {

            Vector3 targetPosition = gunPos.position;
            objectToPull.transform.position = Vector3.MoveTowards(objectToPull.transform.position, targetPosition, pullSpeed * Time.deltaTime);

            if (Vector3.Distance(objectToPull.transform.position, targetPosition) < addDistance && objectToPull.activeInHierarchy)
            {
                isPulling = false;
                if (bulletSlot.ContainsKey(bulletState))
                {
                    BulletState currentState = bulletState;

                    List<GameObject> currentPool = bulletSlot[currentState];
                    List<GameObject> outPool;
                    if (objectToPull.GetComponent<Item>() == null)
                        return;
                    string targetName = objectToPull.GetComponent<Item>().itemData.itemName;
                    //지금 먹은 아이템과 같은 아이템을 저장하고 있는 슬롯이 있나요?
                    if(isThereSameSlot(targetName, out outPool))
                    {
                        AddPool(outPool, objectToPull);
                    }
                    //지금 슬롯이 비었다면 저장
                    else if (currentPool.Count == 0)
                    {
                        AddPool(currentPool, objectToPull);
                    }
                    //그것도 아니라면 처음부터 순회하면서 0인 곳 찾아서 넣어라
                    else
                    {
                        BulletState[] enumValues = (BulletState[])Enum.GetValues(typeof(BulletState));

                        for (int i = 0; i < enumValues.Length; i++)
                        {
                            BulletState key = enumValues[i];
                            if (bulletSlot.ContainsKey(key))
                            {
                                List<GameObject> item = bulletSlot[key];
                                if (item.Count == 0)
                                {
                                    AddPool(item, objectToPull);
                                    break;
                                }
                            }
                        }
                    }
                }
            }

        }
        else if (isPulling && objectToPull != null && objectToPull.layer == 7)
        {
            AutoFarmer autoFarmer = objectToPull.GetComponent<AutoFarmer>();
            if (autoFarmer != null) 
            {
                StartCoroutine(autoFarmer.GenerateItem());
            }
        }

    }
    private void AddPool(List<GameObject> targetPool, GameObject targetObject)
    {

        targetPool.Add(targetObject);
        targetObject.SetActive(false);
        for (int i = 0; i < targetPool.Count; i++)
        {
            print(targetPool[i]);
        }
        Inventory.Instance.AddItemToInventory(targetObject.GetComponent<Item>().itemData);
    }
    private bool isThereSameSlot(string _targetName, out List<GameObject>sameSlot)
    {
        foreach (var item in bulletSlot.Values)
        {
            if (item.Count == 0)
                continue;
            string originName = item[0].GetComponent<Item>().itemData.itemName;
            if (_targetName == originName && objectToPull.activeInHierarchy)
            {
                sameSlot = item;
                return true;
            }
        }
        sameSlot = null;
        return false;
    }

}
