using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using static UnityEngine.GraphicsBuffer;

public class BuildingConsole : MonoBehaviour
{
    public Transform center;
    public GameObject[] structList;
    public GameObject[] UIList;
    public GameObject targetUI;

    private Dictionary<string, GameObject> _nameStructPair = new Dictionary<string, GameObject>();
    private Dictionary<string, GameObject> _nameUIPair = new Dictionary<string, GameObject>();
    private string[] names = { "Cage", "Farm", "Chicken", "Silo", "Incinerator", "Pond"};
    private InteractableObject interactableObject;
    private void Awake()
    {
        interactableObject = GetComponent<InteractableObject>();
        for(int i = 0; i < names.Length; i++)
        {
            _nameStructPair.Add(names[i], structList[i]);
            _nameUIPair.Add(names[i], UIList[i]);
        }
    }
    [Tooltip("&로 건물명과 가격을 구분")]
    public void Interaction(string BuildingAndMoney)
    {
        string[] splitString = BuildingAndMoney.Split('&');
        string structName = splitString[0];
        int cost = int.Parse(splitString[1]);
        Build(structName);
        SuccesAction(cost);
    }
    private void Build(string _structName)
    {
        Instantiate(_nameStructPair[_structName], center);
        interactableObject.targetUI = _nameUIPair[_structName];
    }

    private void SuccesAction(int _money)
    {
        StatusUIManager.instance.gold -= _money;
        Player.Instance.isStop = false;
        GameObject clickObject = EventSystem.current.currentSelectedGameObject;
        clickObject.GetComponent<Button>().interactable = false;
        targetUI = clickObject.transform.parent.parent.gameObject;
        targetUI.SetActive(false);
    }
}
