using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.UI;
using TMPro;

public class AlterController : Building, DamageService, HealthService
{
    public GameObject whiteFreaksPref;
    public TextMeshProUGUI wFreaksCount;
    public TextMeshProUGUI mineCount;

    public float healthPoint = 2000.0f;
    public int essence = 1000;

    [SerializeField] private int whiteFreaks = 5;
    [SerializeField] private bool isAlterClicked = false;

    private List<GameObject> miningFreaks = new List<GameObject>();

    private int busyWhiteF = 0;

    private void Start() //오브젝트 풀링에서 알터를 설정해주는 함수입니다.
    {
        if(ObjectPooling.instance.Alter == null)
            ObjectPooling.instance.Alter_Setting(this.gameObject);
    }
    // Update is called once per frame
    void Update()
    {
        newMiningWorkshopChk();

        if (whiteFreaks - busyWhiteF >= 0)
        {
            wFreaksCount.text = string.Format("{0:D2} / {1:D2}", busyWhiteF, whiteFreaks);
        }
    }


    public bool CanBuild()
    {
        return whiteFreaks - busyWhiteF > 0;
    }


    public void GoBuild(GameObject building)
    {
        busyWhiteF++;
        GameObject whiteFreaks = ObjectPooling.instance.GetObject("WhiteFreaks");
        Vector3 po = new Vector3(transform.position.x + 1, transform.position.y + 2, transform.position.z);
        whiteFreaks.transform.position = po;
        //GameObject whiteFreaks = Instantiate(whiteFreaksPref, transform.position, transform.rotation);
        WhiteFreaksController whiteFreaksController = whiteFreaks.GetComponent<WhiteFreaksController>();
        miningFreaks.Add(whiteFreaks);

        if(building.GetComponent<SwitchTimer>())
        {
            Vector3 pos = GameObject.Find("SwitchController").GetComponent<SwitchController>().Getpos();
            whiteFreaksController.SetSwitch(pos);
        }
        else if(building.GetComponent<WorkshopController>())
        {
            whiteFreaksController.miningWorkshop = building;
            whiteFreaksController.SetMiningWorkShop();
            building.GetComponent<WorkshopController>().SetMiningFreeks(whiteFreaks);
        }
    }


    private void newMiningWorkshopChk()
    {
        if (Input.GetMouseButtonDown(0))
        {
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            RaycastHit hit;

            if (Physics.Raycast(ray, out hit))
            {
                GameObject hitGameObject = hit.transform.gameObject;

                if (hitGameObject == gameObject)
                {
                    isAlterClicked = true;
                }
                else
                {
                    isAlterClicked = false;
                }
            }
        }

        if(Input.GetMouseButtonDown(1))
        {
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            RaycastHit hit;

            if (Physics.Raycast(ray, out hit))
            {
                GameObject hitGameObject = hit.transform.gameObject;

                if (isAlterClicked && hitGameObject.GetComponent<WorkshopController>() != null)
                {
                    GoBuild(hitGameObject);
                }
            }
        }
    }


    public override void SetMaterial(bool isRed)
    {
        return;
    }


    public override void SetOpacity(bool isTransparent)
    {
        return;
    }


    public void DamageTaken(float damageTaken)
    {
         healthPoint -= damageTaken;
    } 


    public float GetCurrentHP()
    {
        return healthPoint;
    }


    public void returnedBusyFreeks()
    {
        busyWhiteF--;
    }
}