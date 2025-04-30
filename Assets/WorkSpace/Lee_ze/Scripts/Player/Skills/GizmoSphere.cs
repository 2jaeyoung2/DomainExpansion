using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(SphereCollider))]
public class GizmoSphere : MonoBehaviour
{
    private SphereCollider sphereCollider;

    private void OnDrawGizmos()
    {
        if (sphereCollider == null)
            sphereCollider = GetComponent<SphereCollider>();

        Gizmos.color = Color.green;

        // ���� �������� ������ ������ ���
        float worldRadius = sphereCollider.radius * Mathf.Max(transform.lossyScale.x, transform.lossyScale.y, transform.lossyScale.z);

        Gizmos.DrawSphere(transform.position + sphereCollider.center, worldRadius);
    }
}