using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerFire : MonoBehaviour
{
    public List<GameObject> slimePool0 = new List<GameObject>();
    public GameObject slime;

    public enum BulletState
    {
        None,
        Slime0,
        Slime1,
        Carrot
    }
    public BulletState bulletState = BulletState.None; 
    private void Update()
    {
        if(Input.GetMouseButtonDown(1))
        {
            BulletCheck(bulletState);
        }

        //임시
        if(Input.GetKeyDown(KeyCode.Alpha1)) 
        {
            bulletState = BulletState.Slime0;
            GameObject instance = Instantiate(slime);
            instance.SetActive(false);
            slimePool0.Add(instance);
        }
    }
    private void BulletCheck(BulletState currentState)
    {
        switch (currentState)
        {
            case BulletState.None:
                print("없다");
                break;
            case BulletState.Slime0:
                if(slimePool0.Count > 0)
                {
                    Fire(slimePool0[slimePool0.Count - 1]);
                    slimePool0.Remove(slimePool0[slimePool0.Count - 1]);
                }
                break;
        }
    }

    private void Fire(GameObject bullet)
    {
        bullet.SetActive(true);
        bullet.transform.position = transform.position;
        bullet.transform.rotation = transform.rotation;

    }
}
