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
            Destroy(cage); // 이 내용은 추후에 SetActive를 통해 비활성화를 하든 등 수정 가능
            Destroy(gameObject);
            SoundManager.instance.PlayTargetSound(SoundManager.instance.KeySFX);
        }
    }
}
