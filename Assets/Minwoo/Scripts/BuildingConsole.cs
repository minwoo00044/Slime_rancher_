using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

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
    private Dictionary<GameObject, int> _bill = new Dictionary<GameObject, int>();

    private void Awake()
    {
        for(int i = 0; i < buildingList.Length; i++)
        {
            _structureMap.Add((StructureList)i, buildingList[i]); 
        }
        _bill.Add(buildingList[0], 250);
        _bill.Add(buildingList[1], 250);
    }
    private void Build()
    {
        if(center.childCount < 1)
        {
            if(StatusUIManager.instance.gold >= _bill[_structureMap[_structureList]])
            {
                GameObject instance = Instantiate(_structureMap[_structureList], center);
                gameObject.GetComponent<InteractableObject>().targetUI = gameObject.GetComponent<InteractableObject>().targetUIs[_structureIndex];
                StatusUIManager.instance.gold -= _bill[_structureMap[_structureList]];
            }
            else
            {
                GameObject clickObject = EventSystem.current.currentSelectedGameObject;
                StartCoroutine(swtichButton(clickObject.GetComponent<Button>())); 
            }
        }
    }
    IEnumerator swtichButton(Button button)
    {
        yield return new WaitForSeconds(0.5f);
        print(button.name);
        button.interactable = true;
    }
}
