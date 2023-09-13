using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

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
    private Dictionary<GameObject, int> _bill = new Dictionary<GameObject, int>();

    private void Awake()
    {
        _bill.Add(buildingList[0], 250);
    }
    private void Build()
    {
        if (StatusUIManager.instance.gold >= _bill[buildingList[_buildingIndex]])
        {
            GameObject instance = Instantiate(buildingList[_buildingIndex], center.GetChild(0));
            Vector3 instancePos = new Vector3(instance.transform.parent.GetChild(buildingIndex + 1).position.x + 1, instance.transform.parent.GetChild(1).position.y, instance.transform.parent.GetChild(1).position.z);
            instance.transform.position = instancePos;
        }
        else
        {
            GameObject clickObject = EventSystem.current.currentSelectedGameObject;
            StartCoroutine(swtichButton(clickObject.GetComponent<Button>()));
        }
    }

    IEnumerator swtichButton(Button button)
    {
        yield return new WaitForSeconds(0.5f);
        print(button.name);
        button.interactable = true;
    }
}
