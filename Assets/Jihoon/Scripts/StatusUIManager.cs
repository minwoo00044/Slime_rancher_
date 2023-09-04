using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class StatusUIManager : MonoBehaviour
{
    public static StatusUIManager instance;
    public int gold;
    public TMP_Text goldText;

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
    }

    private void Update()
    {
        goldText.text = gold.ToString();
    }
}