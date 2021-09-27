using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObjectPooling : MonoBehaviour
{
    public static ObjectPooling Instance;

    [SerializeField]
    private int WhiteFreaksCountLimit;                          //ȭ��Ʈ ������ �α��� ����
    [SerializeField]
    private GameObject WhiteFreaksObjectPrefab;       //ȭ��Ʈ ������ ������
    private Queue<GameObject> WhiteFreaksQueue;    //ȭ��Ʈ ������ ������Ʈ ť

    [SerializeField]
    private int BlackFreaksCountLimit;                          // �� ������ �α��� ����
    [SerializeField]
    private GameObject BlackFreaksObjectPrefab;       //�� ������ ������
    private Queue<GameObject> BlackFreaksQueue;    //�� ������ ������Ʈ ť


    private void Awake()
    {
        Instance = this;
        Initialize();
    }
    private void Initialize()                                             //�ʱ� ����
    { 
        for (int i = 0; i < WhiteFreaksCountLimit; i++)     //ȭ��Ʈ ������ �α��� ���� ��ŭ Instantiate�� �����ؼ� Queue�� Enqueue
        {
            WhiteFreaksQueue.Enqueue(CreateNewObject(WhiteFreaksObjectPrefab)); 
        }
        for (int i = 0; i < BlackFreaksCountLimit; i++)     //�� ������ �α��� ���� ��ŭ Instantiate�� �����ؼ� Queue�� Enqueue
        {
            BlackFreaksQueue.Enqueue(CreateNewObject(BlackFreaksObjectPrefab));
        }
    }
    private GameObject CreateNewObject(GameObject objectName)  //Instatiate�� ������Ʈ ����
    {
        var newObj = Instantiate(objectName);   //objectName�� �ش��ϴ� Prefab�� Instantiate�� ���� �� return
        newObj.SetActive(false);                         //�ش� Object�� SetActive(false)�� ����.
        return newObj; 
    }
    public static GameObject GetObject(string objectName)               //�ش� ������Ʈ�� ����� ��ũ��Ʈ���� ȣ���ϸ� ��
    { 
        switch(objectName)
        {
            case ("WhiteFreaks"):
                if(Instance.WhiteFreaksQueue.Count > 0)                           //����� �� �ִ� ȭ��Ʈ�������� ������
                {
                    var obj = Instance.WhiteFreaksQueue.Dequeue();           //Dequeue�Ͽ� ��������
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
    public static void ReturnObject(GameObject obj)                     //�ı��� | ����� ������Ʈ ��ȯ & ����� ��ũ��Ʈ���� ȣ���ϸ� ��.
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
