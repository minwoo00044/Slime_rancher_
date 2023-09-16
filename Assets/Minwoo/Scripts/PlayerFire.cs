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

    public AudioClip eatSound;
    public AudioClip fireSound;
    public AudioClip emptySound;
    public AudioClip reloadSound;
    public AudioClip tornadoSound;

    public GameObject bingle;
    public int angle = 50;
    public float intensity;
    public Renderer[] render;
    public float delayTime = 0.3f;

    public GameObject addPoolEff;
    public GameObject firePoolEff;
    private bool isPulling = false;
    [SerializeField]
    private GameObject objectToPull;


    private bool isShootCool = false;

    private Animator animator;

    private bool isStick = false;
    private PlayerMove playerMove;
    private BulletState _bulletState = BulletState.Slot0;

    public enum BulletState
    {
        Slot0,
        Slot01,
        Slot02,
        Slot03
    }
    public BulletState bulletState
    {
        get
        {
            return _bulletState;
        }
        set
        {
            _bulletState = value;
            SoundManager.Instance.PlaySound(reloadSound);
            animator.SetTrigger("reload");
        }
    }
    public Dictionary<BulletState, List<GameObject>> bulletSlot = new Dictionary<BulletState, List<GameObject>>();

    private void Awake()
    {
        animator = GetComponent<Animator>();
        playerMove = GetComponent<PlayerMove>();
        bulletSlot.Add(BulletState.Slot0, itemPool0);
        bulletSlot.Add(BulletState.Slot01, itemPool1);
        bulletSlot.Add(BulletState.Slot02, itemPool2);
        bulletSlot.Add(BulletState.Slot03, itemPool3);
    }
    public void InitializePool(int slotNumber, ItemData savedItem, int amount)
    {
        List<GameObject> targetPool = bulletSlot[(BulletState)slotNumber];
        for (int i = 0; i < amount; i++)
        {
            GameObject poolItem = Instantiate(savedItem.itemPrefab);
            poolItem.SetActive(false);
            targetPool.Add(poolItem);
        }
    }
    private void Update()
    {
        if (Player.Instance.isStop)
            return;
        if (isStick)
        {
            if (Input.GetMouseButtonDown(0))
            {
                if (gunPos.childCount == 1)
                    return;
                Fire(gunPos.GetChild(1).gameObject);
                return;
            }
            else if (Input.GetMouseButtonDown(1) && isStick)
            {
                if (gunPos.childCount == 1)
                    return;
                StickOrDrop(gunPos.GetChild(1).gameObject, true);
                return;
            }
            EmissionChange(Color.green, angle, 1);
            pullEff.SetActive(false);
        }
        else
        {
            for (int i = 0; i < render.Length; i++)
            {
                render[i].material.SetColor("_EmissionColor", new Color(0, 1, 1) * intensity);
            }

        }
        if (!isStick)
        {
            if (Input.GetMouseButton(0))
            {
                animator.SetBool("isShoot", true);
                EmissionChange(new Color(1, 0, 0), angle, -1);
                BulletCheck(_bulletState);
            }
            else if (Input.GetMouseButtonUp(0))
            {
                animator.SetBool("isShoot", false);
            }
            if (Input.GetMouseButton(1))
            {
                EmissionChange(Color.green, angle, 1);
                animator.SetBool("isRun", false);
                if(!SoundManager.Instance.IsPlaying(tornadoSound))
                    SoundManager.Instance.PlaySound(tornadoSound);
                PullObject();
                pullEff.SetActive(true);
            }
            else if (Input.GetMouseButtonUp(1))
            {
                if (playerMove.isRunning)
                {
                    animator.SetBool("isRun", true);
                }
                if (SoundManager.Instance.IsPlaying(tornadoSound))
                    SoundManager.Instance.StopSound(tornadoSound);
                pullEff.SetActive(false);
            }

        }
    }
    private void BulletCheck(BulletState currentState)
    {
        if (bulletSlot.ContainsKey(currentState))
        {
            List<GameObject> currentPool = bulletSlot[currentState];
            if (currentPool.Count > 0)
            {
                if (currentPool[currentPool.Count - 1] == null)
                    return;
                StartCoroutine(Fire(currentPool[currentPool.Count - 1], currentPool));
            }
            else
            {
                if (!SoundManager.Instance.IsPlaying(emptySound))
                    SoundManager.Instance.PlaySound(emptySound);

            }
        }
    }
    IEnumerator Fire(GameObject bullet, List<GameObject> _currentPool)
    {
        if (bullet != null)
        {
            if (!isShootCool)
            {
                isShootCool = true;
                ParticleSystemManager.Instance.PlayParticle(firePoolEff, gunPos);

                Rigidbody bulletRigidbody = bullet.GetComponent<Rigidbody>();

                if (bulletRigidbody != null)
                {
                    bullet.SetActive(true);
                    SoundManager.Instance.PlaySound(fireSound);
                    _currentPool.Remove(_currentPool[_currentPool.Count - 1]);
                    bullet.transform.position = gunPos.transform.position;
                    bullet.transform.rotation = gunPos.transform.rotation;
                    Vector3 forceDirection = Camera.main.transform.forward;
                    if (bullet != null)
                    {
                        bulletRigidbody.AddForce(forceDirection * bulletForce, ForceMode.Impulse);
                        if (Inventory.Instance.currentItem != null)
                            Inventory.Instance.UseItem();
                    }
                }
                yield return new WaitForSeconds(delayTime);
                isShootCool = false;
            }

        }
    }
    private void Fire(GameObject bullet)
    {
        if (bullet != null)
        {
            StickOrDrop(bullet, true);
            Rigidbody bulletRigidbody = bullet.GetComponent<Rigidbody>();
            Vector3 forceDirection = Camera.main.transform.forward;
            bulletRigidbody.AddForce(forceDirection * bulletForce, ForceMode.Impulse);
            StartCoroutine(DelayFlag(true));
        }
    }
    private void PullObject()
    {
        Ray ray = Camera.main.ScreenPointToRay(new Vector3(Screen.width / 2, Screen.height / 2, 0)); // Ray from the center of the camera view
        Vector3 center = Camera.main.ScreenToWorldPoint(new Vector3(Screen.width / 2, Screen.height / 2, 0)); // 화면 중심을 월드 좌표로 변환
                                                                                                              // Spherecast의 반지름 설정

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
                if (bulletSlot.ContainsKey(_bulletState))
                {
                    BulletState currentState = _bulletState;

                    List<GameObject> currentPool = bulletSlot[currentState];
                    List<GameObject> outPool;
                    if (objectToPull.GetComponent<Item>() == null)
                        return;
                    Item targetItem = objectToPull.GetComponent<Item>();
                    string targetName = targetItem.itemData.itemName;
                    //지금 먹은 아이템과 같은 아이템을 저장하고 있는 슬롯이 있나요?
                    if (isThereSameSlot(targetName, out outPool))
                    {
                        if (!targetItem.itemData.isBig)
                        {
                            AddPool(outPool, objectToPull);
                        }
                        else
                        {
                            StickOrDrop(objectToPull, false);
                        }
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
                                    if (!targetItem.itemData.isBig)
                                    {
                                        AddPool(item, objectToPull);
                                        break;
                                    }
                                    else
                                    {
                                        StickOrDrop(objectToPull, false);
                                        break;
                                    }
                                }
                            }
                        }
                    }
                }
            }

        }
        else if (isPulling && objectToPull != null && objectToPull.layer == 7)
        {
            StoredItemGetter storedItemGetter = objectToPull.GetComponent<StoredItemGetter>();
            if (storedItemGetter != null)
            {
                StartCoroutine(storedItemGetter.GenerateItem());
            }
        }
    }
    private void AddPool(List<GameObject> targetPool, GameObject targetObject)
    {
        ParticleSystemManager.Instance.PlayParticle(addPoolEff, gunPos);
        targetPool.Add(targetObject);
        targetObject.SetActive(false);
        SoundManager.Instance.PlaySound(eatSound);
        Inventory.Instance.AddItemToInventory(targetObject.GetComponent<Item>().itemData);
    }
    private void StickOrDrop(GameObject targetObject, bool flag)
    {
        Rigidbody rb = targetObject.GetComponent<Rigidbody>();
        targetObject.GetComponent<Collider>().enabled = flag;
        rb.useGravity = flag;
        rb.freezeRotation = !flag;
        if (!flag)
        {
            rb.velocity = Vector3.zero;
            targetObject.transform.localEulerAngles = Vector3.zero;
            targetObject.transform.SetParent(gunPos.transform);
            isStick = !flag;
        }
        else
        {
            targetObject.transform.SetParent(null);
            StartCoroutine(DelayFlag(flag));
        }
    }
    IEnumerator DelayFlag(bool flag)
    {
        yield return new WaitForSeconds(0.35f);
        isStick = !flag;
    }
    private bool isThereSameSlot(string _targetName, out List<GameObject> sameSlot)
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
    private void EmissionChange(Color color, float angle, int dir)
    {
        bingle.transform.Rotate(0, 0, dir * angle);
        for (int i = 0; i < render.Length; i++)
        {
            render[i].material.SetColor("_EmissionColor", color * intensity);
        }
    }

}
