using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static UnityEngine.GraphicsBuffer;

public class PlayerFire : MonoBehaviour
{
    [HideInInspector]
    public List<GameObject> slimePool0 = new List<GameObject>();
    [HideInInspector]
    public List<GameObject> slimePool1 = new List<GameObject>();
    [HideInInspector]
    public List<GameObject> slimePool2 = new List<GameObject>();
    [HideInInspector]
    public List<GameObject> slimePool3 = new List<GameObject>();

    public GameObject slime;
    public float maxDistance = 5f;
    public float pullSpeed = 5f;
    public LayerMask pullableObjectsLayer;
    public Transform gunPos;
    public GameObject pullEff;
    public float bulletForce;
    public float addDistance = 0.5f;

    private bool isPulling = false;
    private GameObject objectToPull;

    public enum BulletState
    {
        None,
        Slot0,
        Slot01,
        Slot02,
        Slot03
    }
    public BulletState bulletState = BulletState.None;

    public Dictionary<BulletState, List<GameObject>> bulletSlot = new Dictionary<BulletState, List<GameObject>>();

    private void Awake()
    {
        bulletSlot.Add(BulletState.Slot0, slimePool0);
        bulletSlot.Add(BulletState.Slot01, slimePool1);
        bulletSlot.Add(BulletState.Slot02, slimePool2);
        bulletSlot.Add(BulletState.Slot03, slimePool3);
    }
    private void Update()
    {

        if (Input.GetMouseButtonDown(1))
        {
            BulletCheck(bulletState);
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
        //임시
        if (Input.GetKeyDown(KeyCode.Alpha1))
        {
            bulletState = BulletState.Slot0;
            GameObject instance = Instantiate(slime);
            instance.SetActive(false);
            slimePool0.Add(instance);
        }
    }
    private void BulletCheck(BulletState currentState)
    {
        switch (currentState)
        {
            case BulletState.None:
                print("없다");
                break;
            case BulletState.Slot0:
                if (slimePool0.Count > 0)
                {
                    Fire(slimePool0[slimePool0.Count - 1]);
                    slimePool0.Remove(slimePool0[slimePool0.Count - 1]);
                }
                break;
        }
    }

    private void Fire(GameObject bullet)
    {

        Rigidbody bulletRigidbody = bullet.GetComponent<Rigidbody>();

        if (bulletRigidbody != null)
        {
            bullet.SetActive(true);
            bullet.transform.position = gunPos.transform.position;
            bullet.transform.rotation = gunPos.transform.rotation;
            Vector3 forceDirection = transform.forward;
            bulletRigidbody.AddForce(forceDirection * bulletForce, ForceMode.Impulse);
        }
    }

    private void PullObject()
    {
        Ray ray = Camera.main.ScreenPointToRay(new Vector3(Screen.width / 2, Screen.height / 2, 0)); // Ray from the center of the camera view

        if (Physics.Raycast(ray, out RaycastHit hit, maxDistance, pullableObjectsLayer))
        {
            objectToPull = hit.collider.gameObject;

            isPulling = true;
        }

        if (isPulling && objectToPull != null)
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
                    //int count = 1;
                    //bool isAddble = true;
                    int targetID = objectToPull.GetComponent<ID>().objectID;

                    

                    //지금 먹은 아이템과 같은 아이템을 저장하고 있는 슬롯이 있나요?
                    if(isThereSameSlot(targetID, out outPool))
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
                        foreach (var item in bulletSlot.Values)
                        {
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

    private void AddPool(List<GameObject> targetPool, GameObject targetObject)
    {
        targetPool.Add(targetObject);
        targetObject.SetActive(false);
    }

    private bool isThereSameSlot(int targetID, out List<GameObject>sameSlot)
    {
        foreach (var item in bulletSlot.Values)
        {
            if (item.Count == 0)
                continue;
            int originID = item[0].GetComponent<ID>().objectID;
            if (targetID == originID && objectToPull.activeInHierarchy)
            {
                sameSlot = item;
                return true;
            }
        }
        sameSlot = null;
        return false;
    }

}
