using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObjectPooling : MonoBehaviour
{
    public static ObjectPooling Instance = null;

    [SerializeField]
    private int WhiteFreaksCountLimit = 100;                          //ȭ��Ʈ ������ �α��� ����
    [SerializeField]
    private GameObject WhiteFreaksObject;       //ȭ��Ʈ ������ ������
    private Queue<GameObject> WhiteFreaksQueue = new Queue<GameObject>();    //ȭ��Ʈ ������ ������Ʈ ť

    [SerializeField]
    private int BlackFreaksCountLimit = 100;                          // �� ������ �α��� ����
    [SerializeField]
    private GameObject BlackFreaksObject;       //�� ������ ������
    private Queue<GameObject> BlackFreaksQueue = new Queue<GameObject>();    //�� ������ ������Ʈ ť


    private void Awake()
    {
        if (Instance == null)
            Instance = this;
        else if (Instance != null)
            Destroy(this.gameObject);

        Initialize();
    }
    private void Initialize()                                             //�ʱ� ����
    { 
        for (int i = 0; i < WhiteFreaksCountLimit; i++)     //ȭ��Ʈ ������ �α��� ���� ��ŭ Instantiate�� �����ؼ� Queue�� Enqueue
        {
            WhiteFreaksQueue.Enqueue(CreateNewObject("WhiteFreaks")); 
        }
        for (int i = 0; i < BlackFreaksCountLimit; i++)     //�� ������ �α��� ���� ��ŭ Instantiate�� �����ؼ� Queue�� Enqueue
        {
            BlackFreaksQueue.Enqueue(CreateNewObject("BlackFreaks"));
        }
    }
    private GameObject CreateNewObject(string Obj)  //Instatiate�� ������Ʈ ����
    {
        GameObject newObj;
        switch (Obj)
        {
            case "WhiteFreaks":
                newObj = Instantiate(WhiteFreaksObject);
                //newObj.gameObject.transform.SetParent(this.gameObject.transform);
                newObj.SetActive(false);
                return newObj;
            case "BlackFreaks":
                Vector3 spawnPos = new Vector3(0, 0, 0);
                newObj = Instantiate(BlackFreaksObject, spawnPos, Quaternion.identity);
                newObj.SetActive(false);
                return newObj;
            default:
                break;
        }
        return null;
    }
    public GameObject GetObject(string objectName)               //�ش� ������Ʈ�� ����� ��ũ��Ʈ���� ȣ���ϸ� ��
    { 
        switch(objectName)
        {
            case ("WhiteFreaks"):
                if(Instance.WhiteFreaksQueue.Count > 0)                           //����� �� �ִ� ȭ��Ʈ�������� ������
                {
                    var obj = Instance.WhiteFreaksQueue.Dequeue();           //Dequeue�Ͽ� ��������
                    obj.transform.position = GameObject.Find("Alter").transform.position;
                    obj.SetActive(true);
                    return obj;
                }
                else
                {
                    return null;
                }

            case ("BlackFreaks"):
                if (Instance.BlackFreaksQueue.Count > 0)                         //����� �� �ִ� �� �������� ������
                {
                    var obj = Instance.BlackFreaksQueue.Dequeue();          //Dequeue�Ͽ� ��������
                    obj.SetActive(true);
                    return obj;
                }
                else
                {
                    return null;
                }
            default:
                return null;
        }
    }
    public void ReturnObject(GameObject obj)                     //�ı��� | ����� ������Ʈ ��ȯ & ����� ��ũ��Ʈ���� ȣ���ϸ� ��.
    { 
        obj.gameObject.SetActive(false);                                          //�ش� ������Ʈ OFF

        switch (obj.name)
        {
            case ("WhiteFreaks"):
                Instance.WhiteFreaksQueue.Enqueue(obj);                     //ȭ��Ʈ ������ Queue�� Enqueue
                break;

            case ("BlackFreaks"):
                Instance.BlackFreaksQueue.Enqueue(obj);                     //�� ������ Queue�� Enqueue
                break;

            default:
                break;
        }
    }



}
