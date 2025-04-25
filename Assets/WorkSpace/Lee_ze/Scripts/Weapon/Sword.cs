using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Sword : MonoBehaviour
{
    public int damage;

    public int downCount;

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("BLUE") == true) // TODO:�ӽ��±� ����. ���� �ʿ�
        {
            other.GetComponent<IDamageable>()?.GetDamage(damage, downCount);
        }
    }
}
