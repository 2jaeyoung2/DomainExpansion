using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SwordStatistic : MonoBehaviour
{
    public int damage;

    public int downCount;

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Enemy") == true) // TODO:�ӽ��±� ����. ���� �ʿ�
        {
            other.GetComponent<IDamageable>()?.GetHit(damage, downCount);
        }
    }
}
