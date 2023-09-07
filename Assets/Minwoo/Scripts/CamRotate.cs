using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CamRotate : MonoBehaviour
{
    public float speed = 200f;
    float mx = 0;
    float my = 0;

    public GameObject arm;
    void Update()
    {

        float mouseX = Input.GetAxis("Mouse X");
        float mouseY = Input.GetAxis("Mouse Y");

        mx += mouseX * speed * Time.deltaTime;
        my += mouseY * speed * Time.deltaTime;

        my = Mathf.Clamp(my, -90, 90);
        //���� 2 : ���콺 �Է¿� ���� ȸ�� ������ �����Ѵ�.
        Vector3 dir = new Vector3(-my, mx, 0);
        //���� 3 : ��ü�� ȸ����Ų��.
        // r = r0 + vt

        //arm.transform.eulerAngles = new Vector3(-my, mx, 0);
        transform.eulerAngles = dir;
    }
}
