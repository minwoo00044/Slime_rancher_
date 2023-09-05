using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class StatusUIManager : MonoBehaviour
{
    public static StatusUIManager instance;
    public int gold;
    public TMP_Text goldText;
    public Image HpBar;

    GameObject Player;
    int hp;
    int maxHp;

    // Start is called before the first frame update
    void Start()
    {
        if(instance == null)
        {
            instance = this;
        }
        else
        {
            Destroy(gameObject);
        }
        Player = GameObject.FindWithTag("Player");
    }

    private void Update()
    {
        goldText.text = gold.ToString();

    }
}