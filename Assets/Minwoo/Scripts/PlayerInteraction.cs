using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerInteraction : MonoBehaviour
{
    public float maxDistance;
    public LayerMask interactableObject;
    public GameObject Guidetext;

    [SerializeField]
    private  GameObject _targetUI;
    void Update()
    {
        Vector3 viewportCenter = new Vector3(0.5f, 0.5f, 0.0f);

        // Cast a ray from the center of the camera
        Ray ray = Camera.main.ViewportPointToRay(viewportCenter);

        RaycastHit hit;
        if (Guidetext != null)
        {
            if (Physics.Raycast(ray, out hit, maxDistance, interactableObject))
            {
                if (hit.collider != null)
                {
                    Guidetext.SetActive(true);
                    _targetUI = hit.collider.gameObject.GetComponent<InteractableObject>().targetUI;
                }
            }
            else
            {
                Guidetext.SetActive(false);
                _targetUI = null;
            }
        }

        if(_targetUI != null)
        {
            if(Input.GetKeyDown(KeyCode.E))
            {
                _targetUI.SetActive(true);
                Guidetext.SetActive(false);
                Player.Instance.isStop = true;
            }
        }
    }
}
