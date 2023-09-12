using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CageConsole : MonoBehaviour
{
    public GameObject[] buildingList;

    public GameObject center;
    private int _buildingIndex;
    public int buildingIndex
    {
        get { return _buildingIndex; }
        set 
        {
            _buildingIndex = value;
            Build();
        }
    }

    private void Build()
    {
        GameObject instance = Instantiate(buildingList[buildingIndex]);
        instance.transform.position = center.transform.GetChild(0).transform.position;

    }
}
