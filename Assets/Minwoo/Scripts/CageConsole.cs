using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CageConsole : MonoBehaviour
{
    public GameObject[] buildingList;

    public Transform center;
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
        GameObject instance = Instantiate(buildingList[buildingIndex], center.GetChild(0));
        Vector3 instancePos = new Vector3(instance.transform.parent.GetChild(1).position.x + 1, instance.transform.parent.GetChild(1).position.y - 3, instance.transform.parent.GetChild(1).position.z);
        instance.transform.position = instancePos;

    }
}
