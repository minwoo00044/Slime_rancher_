using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;

public class ObjectData
{
    public string name;
    public float positionX;
    public float positionY;
    public float positionZ;
}

public class ObjectManager : MonoBehaviour
{
    public GameObject objectPrefab; // ������ ������Ʈ ������
    private List<ObjectData> objectDataList = new List<ObjectData>();

    private void Start()
    {
        // ���� ���� �� ������ �ҷ�����
        LoadObjects();
    }

    private void OnApplicationQuit()
    {
        print(1);
        SaveObjects();
    }

    public void SaveObjects()
    {
        objectDataList.Clear();
        int i = 0;
        // ���� ������Ʈ�� ��ȸ�ϸ鼭 ������ ����
        foreach (GameObject obj in GameObject.FindGameObjectsWithTag("Food"))
        {
            ObjectData data = new ObjectData
            {
                name = obj.name,
                positionX = obj.transform.position.x,
                positionY = obj.transform.position.y,
                positionZ = obj.transform.position.z
            };
            
            objectDataList.Add(data);
            i++;
        }

        // �����͸� JSON �������� ����
        string json = JsonUtility.ToJson(objectDataList);
        File.WriteAllText("objectData.json", json);
    }

    public void LoadObjects()
    {
        // ����� ������ �ҷ�����
        if (File.Exists("objectData.json"))
        {
            string json = File.ReadAllText("objectData.json");
            
            objectDataList = JsonUtility.FromJson<List<ObjectData>>(json);

            // ����� �����͸� ������� ������Ʈ ����
            foreach (ObjectData data in objectDataList)
            {
                Vector3 position = new Vector3(data.positionX, data.positionY, data.positionZ);
                GameObject newObj = Instantiate(objectPrefab, position, Quaternion.identity);
                newObj.name = data.name;
            }
        }
    }
}