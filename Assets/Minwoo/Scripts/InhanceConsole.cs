using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class InhanceConsole : MonoBehaviour
{
    public GameObject targetUI;

    public void EqiupZetPack(int money)
    {
        if(StatusUIManager.instance.gold >= money)
        {
            Player.Instance.isEquipZetPack = true;
            StatusUIManager.instance.gold -= money;
            SuccesAction();
        }
    }
    public void EqiupImpact(int money)
    {
        if (StatusUIManager.instance.gold >= money)
        {
            Player.Instance.isEquipImpact = true;
            StatusUIManager.instance.gold -= money;
            SuccesAction();
        }
    }

    public void EqiupWaterTank(int money)
    {
        if (StatusUIManager.instance.gold >= money)
        {
            Player.Instance.isEquipWaterTank = true;
            StatusUIManager.instance.gold -= money;
            SuccesAction();
        }
    }

    public void InhanceHp(int money)
    {
        if (StatusUIManager.instance.gold >= money)
        {
            Player.Instance.maxHp += 50;
            StatusUIManager.instance.gold -= money;
            SuccesAction();
        }
    }
    public void InhanceStamina(int money)
    {
        if (StatusUIManager.instance.gold >= money)
        {
            Player.Instance.maxStamina += 50;
            StatusUIManager.instance.gold -= money;
            SuccesAction();
        }
    }

    public void EquipTankBooster(int money)
    {
        if (StatusUIManager.instance.gold >= money)
        {
            Player.Instance.staminaRegen *= 1.5f;
            StatusUIManager.instance.gold -= money;
            SuccesAction();
        }
    }
    private void SuccesAction()
    {
        Player.Instance.isStop = false;
        GameObject clickObject = EventSystem.current.currentSelectedGameObject;
        clickObject.GetComponent<Button>().interactable = false;
        targetUI = clickObject.transform.parent.parent.gameObject;
        targetUI.SetActive(false);
    }
}
