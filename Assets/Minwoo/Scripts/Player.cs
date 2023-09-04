using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{

    private static Player instance;

    public static Player Instance
    {
        get
        {
            if (instance == null)
            {
                instance = FindObjectOfType<Player>();
                if (instance == null)
                {
                    GameObject singleton = new GameObject("Player");
                    instance = singleton.AddComponent<Player>();
                }
            }
            return instance;
        }
    }
    [SerializeField]
    private int _hp = 10;
    public int hp
    {
        get { return _hp; }
        set
        {
            if(value > maxHp)
            {
                value = maxHp;
            }
            _hp = value;
            if (_hp < 0)
            {
                Die();
            }

        }
    }
    private int maxHp = 10;
    [SerializeField]
    private float _stamina = 100;
    public float stamina
    {
        get { return _stamina; }
        set
        {
            if (value > maxStamina) 
            {
                value = maxStamina;
            }
            else if (value < 0) 
            {
                value = 0;
            }
            _stamina = value;
        }
    }
    public float staminaRegen;
    private float maxStamina = 100;
    private PlayerMove playerMove;
    private PlayerFire playerFire;
    public GameObject SpotLight;
    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
            DontDestroyOnLoad(gameObject);
        }

        else
        {
            Destroy(gameObject);
        }
        maxHp = _hp;
        maxStamina = stamina;
        playerMove = GetComponent<PlayerMove>();
        playerFire = GetComponent<PlayerFire>();
    }

    private void Update()
    {
        if(!playerMove.isStaminaReduce)
        {
            stamina += staminaRegen * Time.deltaTime;
        }
        if(Input.GetKeyDown(KeyCode.F))
        {
            SpotLight.SetActive(!SpotLight.activeInHierarchy);
        }
    }
    [ContextMenu("Die")]
    private void Die()
    {

        foreach(var item in playerFire.bulletSlot.Values)
        {
            item.Clear();
        }
    }
}
