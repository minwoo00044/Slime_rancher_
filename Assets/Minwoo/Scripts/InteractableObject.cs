using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InteractableObject : MonoBehaviour
{
    public List<GameObject> targetUIs;
    public GameObject targetUI;

    public void ChangeState()
    {
        Player.Instance.isStop = false;
    }

}
