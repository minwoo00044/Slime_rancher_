using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BuildingConsole : MonoBehaviour
{
    public Transform center;
    public GameObject[] buildingList;
    public enum StructureList
    {
        SlimeCage,
        Farm
    }
    private StructureList _structureList;
    private int _structureIndex;
    public int structureIndex
    {
        set
        {
            _structureIndex = value;
            _structureList = (StructureList)value;
            Build();
        }
    }
    private Dictionary<StructureList, GameObject> _structureMap = new Dictionary<StructureList, GameObject>();


    private void Awake()
    {
        for(int i = 0; i < buildingList.Length; i++)
        {
            _structureMap.Add((StructureList)i, buildingList[i]);
        }
    }

    private void Build()
    {
        if(center.childCount < 1)
        {
            GameObject instance = Instantiate(_structureMap[_structureList], center);
            gameObject.GetComponent<InteractableObject>().targetUI = gameObject.GetComponent<InteractableObject>().targetUIs[_structureIndex];
        }
    }
}
