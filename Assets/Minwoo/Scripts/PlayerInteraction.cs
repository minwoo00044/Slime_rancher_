using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerInteraction : MonoBehaviour
{
    public float maxDistance;
    public LayerMask interactableObject;
    public GameObject Guidetext;
    public AudioClip UISound;

    [SerializeField]
    private GameObject _targetUI;
    private AutoHarvest autoFarmer;
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
                    if (hit.collider.gameObject.layer == 9)
                    {
                        if(hit.collider.gameObject.GetComponent<InteractableObject>() != null)
                        {
                            _targetUI = hit.collider.gameObject.GetComponent<InteractableObject>().targetUI;
                            autoFarmer = null;
                        }
                    }
                    else
                    {
                        autoFarmer = hit.collider.gameObject.gameObject.GetComponent<AutoHarvest>();
                        _targetUI = null;
                    }
                }
            }
            else
            {
                Guidetext.SetActive(false);
                _targetUI = null;
            }
        }

        if (Input.GetKeyDown(KeyCode.E))
        {

            if (_targetUI != null)
            {
                SoundManager.Instance.PlaySound(UISound);
                _targetUI.SetActive(true);
                Guidetext.SetActive(false);
                Player.Instance.isStop = true;
            }
            else if (autoFarmer != null)
            {
                SoundManager.Instance.PlaySound(UISound);
                StartCoroutine(autoFarmer.AddPool());
            }
        }
    }
}
