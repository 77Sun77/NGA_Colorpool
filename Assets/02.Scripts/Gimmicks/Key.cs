using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Key : MonoBehaviour
{
    public GameObject cage;
    
    public void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("BALL"))
        {
            Destroy(cage); // �� ������ ���Ŀ� SetActive�� ���� ��Ȱ��ȭ�� �ϵ� �� ���� ����
            Destroy(gameObject);
            SoundManager.instance.PlayTargetSound(SoundManager.instance.KeySFX);
        }
    }
}
