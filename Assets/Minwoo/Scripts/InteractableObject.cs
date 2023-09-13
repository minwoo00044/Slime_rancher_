using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class InteractableObject : MonoBehaviour
{
    public List<GameObject> targetUIs;
    public GameObject targetUI;

    public void ChangeState()
    {
        Player.Instance.isStop = false;
    }


}
