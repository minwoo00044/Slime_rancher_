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
    public GameObject objectPrefab; // 저장할 오브젝트 프리팹
    private List<ObjectData> objectDataList = new List<ObjectData>();

    private void Start()
    {
        // 게임 시작 시 데이터 불러오기
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
        // 게임 오브젝트를 순회하면서 데이터 수집
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

        // 데이터를 JSON 형식으로 저장
        string json = JsonUtility.ToJson(objectDataList);
        File.WriteAllText("objectData.json", json);
    }

    public void LoadObjects()
    {
        // 저장된 데이터 불러오기
        if (File.Exists("objectData.json"))
        {
            string json = File.ReadAllText("objectData.json");
            
            objectDataList = JsonUtility.FromJson<List<ObjectData>>(json);

            // 저장된 데이터를 기반으로 오브젝트 생성
            foreach (ObjectData data in objectDataList)
            {
                Vector3 position = new Vector3(data.positionX, data.positionY, data.positionZ);
                GameObject newObj = Instantiate(objectPrefab, position, Quaternion.identity);
                newObj.name = data.name;
            }
        }
    }
}