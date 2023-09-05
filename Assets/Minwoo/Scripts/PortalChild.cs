using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PortalChild : MonoBehaviour
{
    private Portal portalParent;
    private PortalChild[] portalChild;


    private GameObject mine;
    private GameObject otherOne;
    private PortalChild otherOneScript;

    public bool isEnter;

    private bool isTeleporting;
    private void Start()
    {
        portalParent = GetComponentInParent<Portal>();
        portalChild = portalParent.GetComponentsInChildren<PortalChild>();

        if(portalParent.pointA == gameObject)
        {
            mine = portalParent.pointA;
            otherOne = portalParent.pointB;
        }
        else
        {
            mine = portalParent.pointB;
            otherOne = portalParent.pointA;
        }
        otherOneScript = otherOne.GetComponent<PortalChild>();
    }
    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("Player") && !isTeleporting)
        {
            CharacterController _playerController = other.GetComponent<CharacterController>();
            StartCoroutine(portalAction(_playerController, portalParent.portalCoolTime));
        }
    }
    IEnumerator portalAction(CharacterController playerController, float timer)
    {
        isTeleporting = true;
        otherOneScript.isTeleporting = true;
        if (portalParent.isboth)
        {
            TeleportLogic(playerController);
        }
        else
        {
            if(isEnter && !otherOneScript.isEnter)
            {
                TeleportLogic(playerController);
            }
            else if(!isEnter && otherOneScript.isEnter)
            {
                print("이곳은 출구");
            }
            else if (!isEnter && !otherOneScript.isEnter)
            {
                print("둘 중 한 곳은 입구여야 합니다.");
            }
        }

        yield return new WaitForSeconds(timer);
        isTeleporting = false;
        otherOneScript.isTeleporting = false;
    }

    private void TeleportLogic(CharacterController playerController)
    {
        playerController.enabled = false;
        playerController.GetComponent<CharacterController>().transform.position = otherOne.transform.position;
        playerController.enabled = true;
    }
}
