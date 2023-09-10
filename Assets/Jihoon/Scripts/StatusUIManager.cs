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
    public Image hpBar;
    public Image enBar;

    GameObject Player;
    int hp;
    int maxHp;

    float energy;
    float maxEnergy;


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
        hp = Player.GetComponent<Player>().hp;
        maxHp = hp;
        energy = Player.GetComponent<Player>().stamina;
        maxEnergy = energy;
    }

    private void Update()
    {
        hp = Player.GetComponent<Player>().hp;
        energy = Player.GetComponent<Player>().stamina;
        goldText.text = gold.ToString();
        hpBar.fillAmount = (float)hp / (float)maxHp;
        enBar.fillAmount = energy / maxEnergy;
    }
}