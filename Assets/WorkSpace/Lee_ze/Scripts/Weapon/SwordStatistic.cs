using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SwordStatistic : MonoBehaviour
{
    public int damage;

    public int downCount;

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Enemy") == true) // TODO:임시태그 설정. 변경 필요
        {
            other.GetComponent<IDamageable>()?.GetHit(damage, downCount);
        }
    }
}
