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

        // 월드 스케일을 적용한 반지름 계산
        float worldRadius = sphereCollider.radius * Mathf.Max(transform.lossyScale.x, transform.lossyScale.y, transform.lossyScale.z);

        Gizmos.DrawSphere(transform.position + sphereCollider.center, worldRadius);
    }
}