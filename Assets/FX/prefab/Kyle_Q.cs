using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Kyle_Q : MonoBehaviour
{
    [SerializeField]
    float _bulletSpeed = 10f;
    GameObject Bullet_Obj;
    public ParticleSystem Bullet;
    GameObject Hit_Obj;
    public ParticleSystem Hit;
    Vector3 _dirVector;

    private bool One = false;
    //��ų ������ FX_KailQ_Emit : �ѱ����� ����Ʈ
    //Kail Script���� ���� ��ġ�� �ٶ󺸰� �ִ� ���⺤�͸� ���´�. - 1
    //�ٶ󺸰� �ִ� ������ ��� ���� ���ΰ�? - 2
    //���� ������ �������� Paticle�� ����� ��ġ�� �������ش�. - 3
    public void GetVectorInfo(Vector3 _nowPosition, Vector3 _dirVector) // -1 & 2
    {
        transform.position = _nowPosition; // -3
        this._dirVector = _dirVector;            // -3
        transform.LookAt(_dirVector);         // -3
    }
    //====================================//
    //��ų ������ FX_KailQ_Bullet : �Ѿ� ����Ʈ
    //��ų ���� �� Bullet �̵�
    private void Update()
    {
        transform.position += _dirVector.normalized * _bulletSpeed * Time.deltaTime;
    }
    private void Particle_Bullet()
    {
        Bullet.Play(true); 
    }
    //====================================//
    //�� �ǰ� �� FX_Kail_Hit : �� ��ġ�� ����Ʈ
    private void OnTriggerEnter(Collider other)
    {
        if(other.transform.CompareTag("BlackFreaks") && !One)
        {
            _bulletSpeed = 0f;
            Bullet.Play(false);
            Hit.Play(true);
            One = !One;
            StartCoroutine(DeleteThis());
        }
    }
    IEnumerator DeleteThis()
    {
        yield return new WaitForSeconds(Hit.GetComponent<ParticleSystem>().main.startLifetimeMultiplier);
        Destroy(this.gameObject);
    }
}
